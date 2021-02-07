using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using System.IO;

namespace BullfrogExplorer.Loaders
{
    class Texts
    {
        private bool debug = true;
        public int index;
        public string name;
        public Vector2 position;

        public struct Sentence
        {
            public int index;
            public string text;

            public Sentence(int i, string s)
            {
                this.index = i;
                this.text = s;
            }

        }

        public List<Sentence> sentences;

        public Texts(int i, string n)
        {
            this.index = i;
            this.name = n;
            this.sentences = new List<Sentence>();
            this.position = new Vector2(0, 0);
        }

        public bool LoadFile(string fileName)
        {

            if (!File.Exists(fileName))
            {
                return false;
            }
            else
            {
                Console.WriteLine(" - LoadContent() Reading file: " + fileName);

                char c = (char)0x00;

                Encoding iso = Encoding.GetEncoding(437);

                FileStream fileStream = File.OpenRead(fileName);
                BinaryReader binaryReader = new BinaryReader(fileStream, iso);
                byte[] buffer3 = binaryReader.ReadBytes(54);
                buffer3 = binaryReader.ReadBytes( (int)fileStream.Length - 54);

                binaryReader.Close();

                Encoding.Convert(iso, Encoding.UTF8, buffer3);
                string[] s2 = iso.GetString(buffer3).Split(c);

                int i = 0;
                foreach (var sub in s2)
                {
                    sentences.Add(new Sentence(i++, sub));
                }
                return true;
            }
        }
    }
}
