using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BullfrogExplorer.Loaders;


namespace BullfrogExplorer.Engines
{
    class Text
    {

        bool debug = false;

        SpriteBatch spriteBatch;

        private int interline = 1;
        private int approche = 1;

        public Vector2 position;
        public int height;
        Color Color;

        Dictionary<char, int> fontDict = new Dictionary<char, int>();

        public struct Sentence
        {
            public int index;
            public string Text;
            public Color Color;
            public string Position; // Centré/alignement à droite/gauche
            

            public Sentence(int i, string t, Color c, string p)
            {
                index = i;
                Text = t;
                Color = c;
                Position = p;
            }

            public Sentence(string t, Color c, string p)
            {
                index = 0;
                Text = t;
                Color = c;
                Position = p;
            }

        }

        private SpritesSheet spritesSheet;

        public Text(SpritesSheet ss)
        {
            position.X = 0;
            position.Y = 0;
            height = 5;
            Color = Color.White;

            spritesSheet = ss;

            #region * FONTDICT (lot of adds) * 
            int i = 1;
            fontDict.Add('%', i++);
            fontDict.Add('\'', i++);
            fontDict.Add('"', i++);
            fontDict.Add('0', i++);
            fontDict.Add('1', i++);
            fontDict.Add('2', i++);
            fontDict.Add('3', i++);
            fontDict.Add('4', i++);
            fontDict.Add('5', i++);
            fontDict.Add('6', i++);
            fontDict.Add('7', i++);
            fontDict.Add('8', i++);
            fontDict.Add('9', i++);
            fontDict.Add(':', i++);
            fontDict.Add('A', i++);
            fontDict.Add('B', i++);
            fontDict.Add('C', i++);
            fontDict.Add('D', i++);
            fontDict.Add('E', i++);
            fontDict.Add('F', i++);
            fontDict.Add('G', i++);
            fontDict.Add('H', i++);
            fontDict.Add('I', i++);
            fontDict.Add('J', i++);
            fontDict.Add('K', i++);
            fontDict.Add('L', i++);
            fontDict.Add('M', i++);
            fontDict.Add('N', i++);
            fontDict.Add('O', i++);
            fontDict.Add('P', i++);
            fontDict.Add('Q', i++);
            fontDict.Add('R', i++);
            fontDict.Add('S', i++);
            fontDict.Add('T', i++);
            fontDict.Add('U', i++);
            fontDict.Add('V', i++);
            fontDict.Add('W', i++);
            fontDict.Add('X', i++);
            fontDict.Add('Y', i++);
            fontDict.Add('Z', i++);
            fontDict.Add('.', i++);
            fontDict.Add('`', i++);
            fontDict.Add('(', i++);
            fontDict.Add(')', i++);
            fontDict.Add('/', i++);
            fontDict.Add('Ü', i++);
            fontDict.Add('Ä', i++);
            fontDict.Add('Ö', i++);
            fontDict.Add('§', i++);
            fontDict.Add('$', i++);
            fontDict.Add('ò', i++);
            fontDict.Add('Ì', i++);
            fontDict.Add('È', i++);
            fontDict.Add('À', i++);
            fontDict.Add('Ñ', i++);
            fontDict.Add(' ', i++);
            #endregion
        }

        public void SetSpriteBatch(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        public void SetColor(Color Color)
        {
            this.Color = Color;
        }

        public void SetPosition(float setX, float setY)
        {
            this.position.X = setX;
            this.position.Y = setY;
        }

        public void SetPosition(Vector2 p)
        {
            this.SetPosition(p.X, p.Y);
        }

        public void SetInterline(int i)
        {
            this.interline = i;
        }

        public Rectangle SizeOf(string s)
        {
            Rectangle area = new Rectangle();
            if (s != null)
            {

                for (int i = 0; i < s.Length; i++)
                {
                    int sprite = 0;
                    try
                    {
                        sprite = fontDict[s[i]];
                    }
                    catch (KeyNotFoundException)
                    {
                        sprite = 0;
                    }

                    if (sprite < fontDict.Count)
                    {
                        if (spritesSheet.spriteElements[sprite].sprite != null)
                        {
                            if (spritesSheet.spriteElements[sprite].sprite.Height > area.Height)
                                area.Height = spritesSheet.spriteElements[sprite].sprite.Height;
                        }
                    }
                    area.X = area.X + spritesSheet.spriteElements[sprite].sprite.Width + approche;
                }
            }
            area.Width = area.X;

            return area;
        }

        public Rectangle Write(SpriteBatch sb, string s, float setX, float setY)
        {
            Rectangle area = new Rectangle();

            spriteBatch = sb;

            this.position.X = setX;
            this.position.Y = setY;

            area.X = (int)Math.Round(setX);
            area.Y = (int)Math.Round(setY);

            if (debug) Console.WriteLine("Writing : "+ s);
            if (s != null)
            {

                for (int i = 0; i < s.Length; i++)
                {
                    int sprite = 0;
                    try
                    {
                        sprite = fontDict[s[i]];
                    }
                    catch (KeyNotFoundException)
                    {
                        sprite = 0;
                    }

                    Vector2 xy = new Vector2(setX, setY);
                    if (debug) Console.WriteLine("Char: " + s[i] + " Index: " + sprite.ToString());

                    if (sprite < fontDict.Count)
                    {
                        if (spritesSheet.spriteElements[sprite].sprite != null)
                        {
                            sb.Draw(spritesSheet.spriteElements[sprite].sprite, xy, this.Color);

                            if (spritesSheet.spriteElements[sprite].sprite.Height > area.Height)
                                area.Height = spritesSheet.spriteElements[sprite].sprite.Height;
                        }


                    }
                    setX = setX + spritesSheet.spriteElements[sprite].sprite.Width + approche;
                }
            }
            area.Width = (int)Math.Round(setX) - area.X;

            this.position.X = setX;
            return area;
        }

        public Rectangle Write(SpriteBatch sb, string s)
        {
            spriteBatch = sb;
            return Write(sb, s, this.position.X, this.position.Y);
        }

        public Rectangle Write(string s)
        {
            return Write(spriteBatch, s, this.position.X, this.position.Y);
        }

        public Rectangle WriteLine(SpriteBatch sb, string s, float setX, float setY, Color c)
        {
            this.Color = c;
            Rectangle area = new Rectangle();
            float originX = setX;
            this.position.X = setX;
            this.position.Y = setY;
            spriteBatch = sb;
            area = Write(sb, s, this.position.X, this.position.Y);

            // je descend d'une ligne donc sur y
            this.position.Y = this.position.Y + this.height+this.interline;

            // je remet X à l'origine
            this.position.X = originX;
            return area;
        }

        public Rectangle WriteLine(SpriteBatch sb, string s, float setX, float setY)
        {
            spriteBatch = sb;
            return WriteLine(sb, s, setX, setY, this.Color);
        }

        public Rectangle WriteLine(SpriteBatch sb, string s, Color c)
        {
            spriteBatch = sb;
            return WriteLine(sb, s, this.position.X, this.position.Y, c);
        }

        public Rectangle WriteLine(SpriteBatch sb, string s)
        {
            spriteBatch = sb;
            return WriteLine(sb, s, this.position.X, this.position.Y, this.Color);
        }

        public Rectangle WriteLine(string s)
        {
            // #TODO check if spriteBatch has been defined
            return WriteLine(spriteBatch, s, this.position.Y, this.position.X, this.Color);
        }


        public void WritePage(Vector2 Position, int Interline, List<Sentence> text)
        {
            this.SetPosition(Position);
            this.SetInterline(Interline);
            
            foreach (Sentence sentence in text)
            {
                WriteLine(spriteBatch, sentence.Text, sentence.Color);
            }
            

        }


            

    }
}
