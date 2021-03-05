using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BullfrogExplorer.Loaders
{
    class Background
    {
        private bool debug = false;
        private GraphicsDevice graphicsDevice;

        public int index;
        public string name;
        public Texture2D picture;
        private int width = 320;
        private int height = 200;

        
        public Background()
        {

        }

        public void ToggleDebug()
        {
            debug = !debug;
        }

        public void SetGraphicsDevice(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public bool LoadFile(string path, Palette palette)
        {

            if (!File.Exists(path))
            {
                return false;
            }
            else
            {

                Console.WriteLine(" - LoadContent() Reading file: " + path);
                using (FileStream fs = File.OpenRead(path))
                {


                    byte[] buffer = new byte[this.width * this.height];
                    fs.Read(buffer, 0, buffer.Length);

                    int red = 0;
                    int green = 1;
                    int blue = 2;
                    int mult = 4;
                    int offset = 0;

                    Color[] Picture = new Color[this.width * this.height];
                    // je parcours le tableau image et j'y met chaque pixel sous le format color en créant un enregistrement color via
                    //la palette. Je vais chercher dans la palette, les rgb correspondant à l'octet inscrit dans mon tableau buffer.
                    for (var i = 0; i < this.width * this.height; i++)
                    {

                        if (path.Contains("BUSTED")) offset = 25;
                        if (path.Contains("TAKOVER")) offset = 26;
                        if (i > offset)
                        {
                            // modoficated file for some reasons (palette hints I guess)
                            if (buffer[i] == 0)
                            {
                                Picture[i].A = 0;
                            }
                            else
                            {
                                Picture[i - offset] = new Color(palette.palette[buffer[i] * 3 + red] * mult, palette.palette[buffer[i] * 3 + green] * mult, palette.palette[buffer[i] * 3 + blue] * mult);
                            }
                        }
                    }

                    Texture2D pict = new Texture2D(this.graphicsDevice, this.width, this.height);
                    pict.SetData(Picture);
                    this.picture = pict;
                    string[] fn = fs.Name.Split('\\');
                    this.name = fn.Last();
                    
                }
                return true;
            }
        }
    }
}
