using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace BullfrogExplorer.Loaders
{
    class Palette
    {
        bool debug = false;

        public int index;
        public string name;
        public byte[] palette = new byte[768];

        public Palette()
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

        // just to create a dummy all white palette
        public void Generate()
        {
            byte[] palette = new byte[768];
            for (int i = 0; i<768; i++)
            {
                palette[i] = 255;
            }
            this.palette = palette;
        }


        public bool LoadFile(string path)
        {
            byte[] palette = new byte[768];
            if (!File.Exists(path))
            {
                return false;
            }
            else
            {
                Console.WriteLine(" - LoadContent() Reading file: " + path);
                using (FileStream fs = File.OpenRead(path))
                {
                    fs.Read(palette, 0, palette.Length);
                    this.palette = palette;

                    return true;
                }
            }
        }
    }
}
