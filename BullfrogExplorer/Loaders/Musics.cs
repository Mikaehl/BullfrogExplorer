using System;
using System.Collections.Generic;


using System.IO;

namespace BullfrogExplorer.Loaders
{
    class Musics
    {

        private bool debug = false;

        public int index;
        public string name;

        private Table table = new Table(1);

        public struct MusicElement
        {
            public int index;
            public string name;
            public long offset;
            public long lenght;


            public MusicElement(int i, string n, long o, long l)
            {
                index = i;
                name = n;
                offset = o;
                lenght = l;
            }

        }

        public List<MusicElement> musicElements = new List<MusicElement>();

        public Musics()
        {

        }

        public void ToggleDebug()
        {
            debug = !debug;
        }

        public void Extract(List<Musics> lm)
        {
            foreach (Musics mus in lm)
            {
                Console.WriteLine(mus.name);

                foreach (Musics.MusicElement elem in mus.musicElements)
                {
                    Console.WriteLine("Filename: " + elem.name + " offset: " + elem.offset + " lenght: " + elem.lenght);

                    using (FileStream fs = File.OpenRead(mus.name + ".DAT"))
                    {
                        Console.WriteLine("Reading... " + mus.name + ".DAT from " + elem.offset.ToString() + " to " + elem.lenght.ToString());
                        byte[] buffer = new byte[elem.lenght];
                        fs.Seek(elem.offset, 0);
                        fs.Read(buffer, 0, (int)elem.lenght);

                        Console.WriteLine(" Wrinting... " + elem.name);
                        using (var stream = new FileStream(
                            elem.name, FileMode.Create, FileAccess.Write, FileShare.Write, (int)elem.lenght))
                        {
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }

        public void Initialize(int i, string n)
        {
            index = i;
            name = n;
        }

        public bool LoadFile(string path, Table table)
        {

            int Elems = 0;

            this.table = table;

            if (!File.Exists(path))
            {
                return false;
            }
            else
            {
                Console.WriteLine(" - LoadContent() Reading file: " + path);

                using (FileStream fs = File.OpenRead(path))
                {
                    int iii = 0;
                    foreach (Table.MusicTableElement tableElement in this.table.musicTableElements)
                    {
                        iii++;
                        if (this.debug)
                        {
                            Console.WriteLine("[" + iii + "] " + tableElement.filename + " off = " + tableElement.offset + " lenght=" + tableElement.lenght);
                        }

                        byte[] buffer = new byte[tableElement.lenght];
                        fs.Read(buffer, 0, (int)tableElement.lenght);
                        this.musicElements.Add(new MusicElement(Elems++, tableElement.filename, tableElement.offset, tableElement.lenght));
                    }
                }
                return true;
            }
        }
    }
}
