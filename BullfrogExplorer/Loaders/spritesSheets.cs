using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using System.IO;

namespace BullfrogExplorer.Loaders
{
    class SpritesSheet
    {

        private bool debug = false;

        public int index;
        public string name;
        public string filename;

        public Point viewport;

        private GraphicsDevice graphicsDevice;

        private Table table = new Table(0);

        public struct SpriteElement
        {
            public int index;
            public string name;

            public Texture2D sprite;


            public SpriteElement(int i, string n, Texture2D s)
            {
                index = i;
                name = n;
                sprite = s;
            }

        }

        public List<SpriteElement> spriteElements = new List<SpriteElement>();


        public SpritesSheet()
        {
            this.viewport = new Point(0, 0);
        }

        public void ToggleDebug()
        {
            debug = !debug;
        }


        public void Initialize(int i, string s)
        {
            index = i;
            name = s;
        }

        public void SetGraphicsDevice(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public bool LoadFile(string path, Table table, Palette palette)
        {

            int Elems = 0;

            this.table = table;

            

            if (!File.Exists(path))
            {
                return false;
            }
            else
            {


                if (this.debug)  Console.Write(" - LoadContent() Reading file: " + path);
                using (FileStream fs = File.OpenRead(path))
                {
                    int iii = 0;
                    foreach (Table.TableElement tableElement in this.table.tableElements)
                    {
                        iii++;
                        if (this.debug)
                        {
                            Console.WriteLine("[" + iii + "] off=" + tableElement.offsetElement + " height=" + tableElement.height + " width=" + tableElement.width);
                        }

                        if (tableElement.height != 0 && tableElement.width != 0)
                        {
                            byte[] b = new byte[1];
                            fs.Seek(tableElement.offsetElement, 0);

                            int c, r;
                            char g;
                            r = 0;
                            c = 0;
                            long off = tableElement.offsetElement;
                            int height = tableElement.height;
                            int width = tableElement.width;

                            int value;
                            byte[] buffer = new byte[width * height];

                            /*
                                Chunks
                                The pixels are stored in chunks of variable length. Each chunk begins with a byte that determines this chunks type.
                                b denotes the byte's value
                                Value Chunk type
                                b = 0   end of current row
                                b > 0   read the following b bytes, which are opaque pixels(indices in palette)
                                b < 0   leave the following - b pixels transparent
                            */

                            while (r < height)
                            {

                                off++;
                                fs.Read(b, 0, 1);
                                g = Convert.ToChar(b[0]);

                                if ((int)g > 128)
                                {
                                    value = (int)g - 256;
                                    c = c + (-value);

                                }
                                else if ((int)g == 0)
                                {
                                    c = 0;
                                    r++;
                                }
                                else
                                {
                                    value = (int)g;
                                    for (int i = 0; i < value; i++)
                                    {
                                        if (r >= height || c >= width)
                                        {
                                            if (this.debug)
                                            {
                                                Console.WriteLine("\nError - colour leak - off=" + off + " r=" + r + " c= " + c);
                                            }
                                            return false;
                                        }
                                        fs.Read(b, 0, 1);

                                        buffer[(width * r) + c] = b[0];
                                        c++;
                                        off++;
                                    }


                                }

                            }

                            int red = 0;
                            int green = 1;
                            int blue = 2;
                            int mult = 4;

                            Color[] Picture = new Color[width * height];
                            for (var i = 0; i < width * height; i++)
                            {

                                if (buffer[i] == 0)
                                {
                                    Picture[i].A = 0;
                                }
                                else
                                {
                                    Picture[i] = new Color(palette.palette[buffer[i] * 3 + red] * mult, palette.palette[buffer[i] * 3 + green] * mult, palette.palette[buffer[i] * 3 + blue] * mult);
                                }
                            }

                            Texture2D sprite = new Texture2D(this.graphicsDevice, width, height);
                            sprite.SetData(Picture);
                            this.spriteElements.Add(new SpriteElement(Elems++, "", sprite));
                        } else
                        {
                            Texture2D sprite = new Texture2D(this.graphicsDevice, 1, 1);
                            this.spriteElements.Add(new SpriteElement(Elems++, "DUMMY", sprite));
                        }
                    }

                    if (this.debug) Console.WriteLine(" " + Elems + " Sprites");

                    string[] fn = fs.Name.Split('\\');
                    this.filename = fn.Last();

                    return true;
                }
            }
        }
    }
}
