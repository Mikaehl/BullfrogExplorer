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

        private Table table = new Table(1);

        public struct SoundElement
        {
            public int index;
            public string name;
            public SoundEffect soundEffect;


            public SoundElement(int i, string n, SoundEffect s)
            {
                index = i;
                name = n;
                soundEffect = s;
            }

        }

        public List<SoundElement> soundElements = new List<SoundElement>();

        public Sounds()
        {

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
                        SoundEffect sound = new SoundEffect(buffer, 44100, AudioChannels.Stereo);
                        //sound.Play();
                        this.soundElements.Add(new SoundElement(Elems++, tableElement.filename, sound));
                    }
                }
                return true;
            }
        }
    }
 }
