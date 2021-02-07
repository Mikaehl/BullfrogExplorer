using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace BullfrogExplorer.Loaders
{
    class Table
    {
        private bool debug = false;

        public int type = 0;

        public struct TableElement
        {
            public long offsetElement;
            public int width;
            public int height;

            public TableElement(long off, int w, int h)
            {
                offsetElement = off;
                width = w;
                height = h;
            }

        }
        public List<TableElement> tableElements = new List<TableElement>();

        public struct SoundTableElement
        {
            public long offset;
            public string filename;
            public long lenght;

            public SoundTableElement(long o, string f, long l)
            {
                offset = o;
                filename = f;
                lenght = l;
            }

        }

        public List<SoundTableElement> soundTableElements = new List<SoundTableElement>();

        public struct MusicTableElement
        {
            public long offset;
            public string filename;
            public long lenght;

            public MusicTableElement(long o, string f, long l)
            {
                offset = o;
                filename = f;
                lenght = l;
            }
        }

        public List<MusicTableElement> musicTableElements = new List<MusicTableElement>();

        public Table(int t)
        {
            type = t;
        }

        public void ToggleDebug()
        {
            debug = !debug;
        }

        public bool LoadFile(string path)
        {
            if (this.type == 0)
            {
                /** PICTURES ***/
                if (!File.Exists(path))
                {
                    return false;
                }
                else
                {
                    if (this.debug) Console.Write(" - LoadContent() Reading file: " + path);
                    using (FileStream fs = File.OpenRead(path))
                    {
                        int counter = 0;
                        byte[] b = new byte[6];
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            counter++;

                            if (this.debug)
                            {
                                Console.WriteLine(b[0].ToString("X") + " " + b[1].ToString("X") + " " + b[2].ToString("X") + " " + b[3].ToString("X") + " " +
                                b[4].ToString("X") + " " + b[5].ToString("X"));
                            }

                            long seekpos = b[0] + (b[1] << 8) + (b[2] << 16) + (b[3] << 24);

                            if (this.debug)
                            {
                                Console.WriteLine(" - Picture " + counter + " Seekpos=" + seekpos + " Width=" + b[4] + " Height=" + b[5]);
                            }

                            //Unfortunatly I need the empty records because the animation file is indexed with the empty records
                            /*if (b[4] != 0 && b[5] != 0)
                            {*/
                            if (this.debug) Console.WriteLine("Adding element seekpos: " + seekpos);
                            this.tableElements.Add(new TableElement(seekpos, b[4], b[5]));
                            //}
                        }
                        if (this.debug) Console.WriteLine(" " + counter + " Elements");
                    }
                    return true;
                }

            }
            else if (type == 1)
            {
                /*** SOUND ****/
                if (!File.Exists(path))
                {
                    return false;
                }
                else
                {
                    if (this.debug) Console.Write(" - LoadContent() Reading file: " + path);
                    using (FileStream fs = File.OpenRead(path))
                    {
                        int counter = 0;
                        byte[] b = new byte[32];
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            counter++;

                            if (this.debug)
                            {
                                Console.WriteLine(b[0].ToString("X") + " " + b[1].ToString("X") + " " + b[2].ToString("X") + " " + b[3].ToString("X") + " " +
                                b[4].ToString("X") + " " + b[5].ToString("X"));
                            }

                            int i = 0;
                            string filename = "";
                            while (b[i] > 0)
                            {
                                filename = filename + (char)b[i];
                                i++;
                            }

                            long seekpos = b[18] + (b[19] << 8) + (b[20] << 16) + (b[21] << 24);
                            long lenght = b[26] + (b[27] << 8) + (b[28] << 16) + (b[29] << 24);

                            if (this.debug)
                            {
                                Console.WriteLine(" - Sound " + filename + " Seekpos=" + seekpos + " lenght=" + lenght);
                            }

                            if (seekpos != 0 && lenght != 0)
                            {
                                this.soundTableElements.Add(new SoundTableElement(seekpos, filename, lenght));
                            }

                        }
                        if (this.debug) Console.WriteLine(" " + counter + " Elements");
                    }
                    return true;
                }
            }
            else if (type == 2)
            {
                /**** MUSIC ****/
                if (!File.Exists(path))
                {
                    return false;
                }
                else
                {
                    if (this.debug) Console.Write(" - LoadContent() Reading file: " + path);
                    using (FileStream fs = File.OpenRead(path))
                    {
                        int counter = 0;
                        byte[] b = new byte[32];
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            counter++;

                            if (this.debug)
                            {
                                Console.WriteLine(b[0].ToString("X") + " " + b[1].ToString("X") + " " + b[2].ToString("X") + " " + b[3].ToString("X") + " " +
                                b[4].ToString("X") + " " + b[5].ToString("X"));
                            }

                            int i = 0;
                            string filename = "";
                            while (b[i] > 0)
                            {
                                filename = filename + (char)b[i];
                                i++;
                            }

                            long seekpos = b[18] + (b[19] << 8) + (b[20] << 16) + (b[21] << 24);
                            long lenght = b[26] + (b[27] << 8) + (b[28] << 16) + (b[29] << 24);

                            if (this.debug)
                            {
                                Console.WriteLine(" - Music " + filename + " Seekpos=" + seekpos + " lenght=" + lenght);
                            }

                            if (seekpos != 0 && lenght != 0)
                            {
                                this.musicTableElements.Add(new MusicTableElement(seekpos, filename, lenght));
                            }

                        }
                        if (this.debug) Console.WriteLine(" " + counter + " Elements");
                    }
                    return true;
                }
            }
            return false;
        }




    }
}
