using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using System.IO;

namespace BullfrogExplorer.Loaders
{
    class Sounds
    {
        private bool debug = false;

        public int index;
        public string name;
        public int audiosampleRate;
        public AudioChannels audioChannels;

        private Table table = new Table(1);

        public struct SoundElement
        {
            public int index;
            public string name;
            public SoundEffect soundEffect;
            //public long offset;
            //public long lenght;


            public SoundElement(int i, string n, SoundEffect s)
            {
                index = i;
                name = n;
                soundEffect = s;
                //offset = o;
                //lenght = l;
            }

        }

        public List<SoundElement> soundElements = new List<SoundElement>();

        public Sounds(int audiosampleRate, AudioChannels audioChannels)
        {
            this.audiosampleRate = audiosampleRate;
            this.audioChannels = audioChannels;
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
                    foreach (Table.SoundTableElement tableElement in this.table.soundTableElements)
                    {
                        iii++;
                        if (this.debug)
                        {
                            Console.WriteLine("[" + iii + "] off=" + tableElement.offset + " lenght=" + tableElement.lenght);
                        }

                        byte[] buffer = new byte[tableElement.lenght];
                        fs.Read(buffer, 0, (int)tableElement.lenght);
                        SoundEffect sound = new SoundEffect(buffer, this.audiosampleRate, AudioChannels.Mono);
                        //if (tableElement.filename == "CASH.RAW") sound.Play();
                        //this.soundElements.Add(new SoundElement(Elems++, tableElement.filename, tableElement.offset, tableElement.lenght));
                        this.soundElements.Add(new SoundElement(Elems++, tableElement.filename, sound));
                    }
                }
                return true;
            }
        }

        // For this to work, the above comment needs to be changed, offset and lenght value needs to be added instead of the SoundEffect.
        // Or anyway a buffer element should do the trick too.
        // I'll not fix it because I don't plan to allow data extraction.

        public void Extract(List<Sounds> soundsList)
        {/*
            foreach (Sounds sounds in soundsList)
            {
                Console.WriteLine(sounds.name);

                foreach (Sounds.SoundElement elem in sounds.soundElements)

                {
                    Console.WriteLine("Filename: " + elem.name + " offset: " + elem.offset + " lenght: " + elem.lenght);

                    using (FileStream fs = File.OpenRead(sounds.name + ".DAT"))
                    {
                        Console.WriteLine("Reading... " + sounds.name + ".DAT from " + elem.offset.ToString() + " to " + elem.lenght.ToString());
                        byte[] buffer = new byte[elem.lenght];
                        fs.Seek(elem.offset, 0);
                        fs.Read(buffer, 0, (int)elem.lenght);

                        Console.WriteLine(" Writing... " + elem.name);
                        using (var stream = new FileStream(
                            sounds.name+"-"+elem.name, FileMode.Create, FileAccess.Write, FileShare.Write, (int)elem.lenght))
                        {
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
            */
        }



    }
}
