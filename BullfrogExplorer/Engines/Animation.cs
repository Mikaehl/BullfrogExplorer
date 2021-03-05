using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BullfrogExplorer.Loaders;
using BullfrogExplorer.Data;
using static BullfrogExplorer.Loaders.Animations;


namespace BullfrogExplorer.Engines
{
    class Animation
    {
        bool debug = false;


        int currentFrame = 1;
        private TimeSpan delay;
        private TimeSpan previousFrame;
        private TimeSpan delayStep = TimeSpan.FromSeconds(0.025f);
        private int animationIndex = 0;

        private bool replaceGuest = false; // temporary dirty thing

        GraphicsDevice graphicsDevice;
        bool drawIndex = false;
        bool drawElem = false;

        public bool play;
        public bool next = false;

        public Point Location;
        
        public struct Frame
        {
            public int width;
            public int height;
            public Frame (int w, int h)
            {
                this.width = w;
                this.height = h;
            }
        }

        public Frame frame;

        public struct Guest
        {
            public int index;

            public Guest(int i)
            {
                this.index = i;
            }

        }

        public List<Guest> guests = new List<Guest>();

        public List<SpriteFrame> spritesFrames = new List<SpriteFrame>();

        public Animation()
        {

        }

        public Animation(GraphicsDevice gd, Animations a, int i, List<Guest> lg, Point p)
        {

            this.LoadFrames(a.GetFrames(i));
            currentFrame = 1;
            if (this.spritesFrames.Count > 0)
            {
                this.graphicsDevice = gd;
                this.Delay(0.15f);
                this.guests = lg;
                this.Location = p;
                this.frame.width = this.spritesFrames[this.currentFrame-1].width;
                this.frame.height = this.spritesFrames[this.currentFrame-1].height;
                this.play = true;
                this.animationIndex = i;
            }
        }

        public Animation(GraphicsDevice gd, Animations a, int i)
        {
            this.LoadFrames(a.GetFrames(i));

            this.PrintFrames();
            currentFrame = 1;
            
            if (this.spritesFrames.Count > 0)
            {
                this.graphicsDevice = gd;
                this.Delay(0.15f);
                for (i = 0; i < 20; i++) { this.guests.Add(new Guest(1)); }
                this.frame.width = this.spritesFrames[this.currentFrame-1].width;
                this.frame.height = this.spritesFrames[this.currentFrame-1].height;
                this.Location = new Point(CONST.TARGETWIDTH / 2, (CONST.TARGETHEIGHT / 2) + spritesFrames[currentFrame - 1].height / 4);
                if (this.debug) Console.WriteLine("Frame.height:" + this.frame.height + " Frame.width: " + this.frame.width + " Location.Y: " + Location.Y);
                this.play = true;
                this.animationIndex = i;
            }
        }


        public Animation(GraphicsDevice gd, Animations a, int i, Point p)
        {

            this.LoadFrames(a.GetFrames(i));
            currentFrame = 1;
            if (this.spritesFrames.Count > 0)
            {
                this.graphicsDevice = gd;
                this.Delay(0.15f); // 15 millisecondes before next frame
                for (i = 0; i < 20; i++) { this.guests.Add(new Guest(1)); }
                this.Location = p;
                this.frame.width = this.spritesFrames[this.currentFrame-1].width;
                this.frame.height = this.spritesFrames[this.currentFrame-1].height;
                this.play = true;
                this.animationIndex = i;
            }
        }

        public bool ToggleDebug()
        {
            this.debug = !this.debug;
            return this.debug;
        }

        public void ToggleIndex()
        {
            drawIndex = !drawIndex;
        }

        public void ToggleElem()
        {
            drawElem = !drawElem;
        }

        public void AddGuest(int i)
        {
            guests.Add(new Guest(i));

        }

        public void RemGuest(int i)
        {
            if (guests.Count() > i)  guests.RemoveAt(i);
  
        }


        public void ClearGuests(int i)
        {
            guests.Clear();

        }

        public Point SetLocation(Point l)
        {
            this.Location = l;
            return l;
        }

        public void Delay(float rate)
        {
            this.delay = TimeSpan.FromSeconds(rate);
            this.previousFrame = TimeSpan.FromSeconds(0.0f);
        }

        public void Faster()
        {
            if (this.delay.TotalMilliseconds > 0) this.delay = this.delay - this.delayStep;
        }

        public void Slower()
        {
            this.delay = this.delay + this.delayStep;
        }


        public void Play()
        {
            this.play = true;
        }

        public void Stop()
        {
            this.play = false;
        }
        public void Pause()
        {
            this.play = false;
        }

        public void Next()
        {
            if (!this.play) this.next = true;
        }

        public void Previous()
        {
            this.currentFrame--;
            if (currentFrame < 1) currentFrame = spritesFrames.Count;
        }

        public void TPlay()
        {
            this.play = !this.play;
        }

        public void NewAnimation(Animations a, int i)
        {
            if (debug)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("NewAnimation(): " + i);
            }
            this.LoadFrames(a.GetFrames(i));

            currentFrame = 1;

            if (this.spritesFrames.Count > 0)
            {
                this.Location = new Point(CONST.TARGETWIDTH / 2, (CONST.TARGETHEIGHT / 2) + spritesFrames[currentFrame - 1].height / 4);
                this.animationIndex = i;
            }
        }


        public void LoadFrames(List<SpriteFrame> sf)
        {
            this.spritesFrames = sf;
        }


        public void PrintFrames()
        {
            int i = 1;
            
            foreach (SpriteFrame spriteFrame in spritesFrames)
            {
                if (this.debug) Console.WriteLine("Frame " + i + " height: " + spriteFrame.height + " width: " + spriteFrame.width);
                i++;

                int j = 1;
                foreach (SpriteElement spriteElement in spriteFrame.spritesElements)
                {
                    if (this.debug) Console.WriteLine(" SpriteElement: " + j + " sprite: " + spriteElement.sprite / 6 + " x: " + spriteElement.xOffset + " y: " + spriteElement.yOffset + " flipped: " + spriteElement.xFlipped);
                }

            }


        }


        public void Update(GameTime gameTime)
        {
            if (this.play || this.next)
            {
                if (gameTime.TotalGameTime - this.previousFrame > this.delay)
                {
                    currentFrame++;
                    if (this.currentFrame > this.spritesFrames.Count) currentFrame = 1;
                    this.previousFrame = gameTime.TotalGameTime;
                }

                if (!this.play) this.next = false;
            }

        }


        public void Draw(SpriteBatch sb, SpritesSheet ss, SpriteFont _font)
        {

            int drawnPeons = 0;
            int elemIndex = 0;


            

            if (currentFrame - 1 < spritesFrames.Count())
            {

                //  ******  FRAME RECTANGLE ***
                /*
                if (spritesFrames[currentFrame - 1].width > 0 && spritesFrames[currentFrame - 1].height > 0)
                {
                    Rectangle FrameRect = new Rectangle(0, 0, spritesFrames[currentFrame - 1].width, spritesFrames[currentFrame - 1].height);

                    Color[] data = new Color[spritesFrames[currentFrame - 1].width * spritesFrames[currentFrame - 1].height];

                    for (int i = 0; i < data.Length; i++)//loop through all the colors setting them to whatever values we want
                    {
                        data[i] = new Color(255, 255, 255, 255); ;
                    }
                    Texture2D tex = new Texture2D(graphicsDevice, spritesFrames[currentFrame - 1].width, spritesFrames[currentFrame - 1].height);
                    tex.SetData(data);
                    sb.Draw(tex, new Vector2(Location.X - spritesFrames[currentFrame - 1].width / 2, Location.Y - spritesFrames[currentFrame - 1].height), null, Color.White);
                }
                */

                int j = 1;
                int elementCounter = 1;
                foreach (SpriteElement spriteElement in spritesFrames[currentFrame - 1].spritesElements)
                {


                    elemIndex++;
                    long elem = (spriteElement.sprite / 6);

                    int xoff = Location.X - (spriteElement.xOffset) / 2;
                    int yoff = Location.Y - (spriteElement.yOffset) / 2;
                    Vector2 xy = new Vector2(xoff, yoff);

                    Rectangle rectangle = new Rectangle(1, 1, 1, 1);

                    // Dans l'idée, si le flag est à 128, alors on a un sprite modifiable, c'est à dire un personnage.
                    // On doit remplir une table dans l'animation avec un nombre de personnage à utiliser, c'est la 
                    // population de l'attraction puis on l'utilise.

                    // Il faudra quand même revoir l'algo afin que les spectateurs ne soient pas nécessairement tous au même
                    // endroit sur chaque animation. Après c'est peut être pas hyper génant.


                    // En gros l'idée pour les peons standards est d'aller chercher leur sprite index dans la frame dédié (604-605...)
                    // a la position "standard", gueule du serpent en 24, le banzai en 30 etc
                    // Je ne comprend pas pourquoi il y a plusieurs style de frame mais je présume que les animations ne sont pas 
                    // les mêmes pour une raison inconnu
                    // En vrai toutes les anims sont différentes, on ne s'en sortira pas sans une indexation frame par frame

                    // The element is a guest.
                    if (spriteElement.xFlipped > 127)
                    {
                        //elem = 2129;

                        #region - Guest Replacement - CURRENTLY NON WORKING NEED TO REDO WITH AN INDEX
                        if (this.replaceGuest)
                        {
                            if (guests.Count == 0)
                            {
                                //nothing
                                elem = 0;
                            }
                            else
                            {
                                if (drawnPeons < guests.Count)
                                {
                                    //en théorie j'affecte le type de peon a elem ici.
                                    //Console.Write("current elem is: " + elem);
                                    //elem =CONST.ADULTGIRL_GREENDRESS_INDEXES[elem - 65];

                                    // okok, c'est pas très simple mais c'est rudement efficace
                                    int newelem;
                                    newelem = Guests.guests.Find(x => x.Index == Guests.currentGuestIndex).animationIndexes.Find(x => x.Index == elem).Sprite;

                                    //newelem = Peons.peons.Find(x => x.Name == "girl green dress").animationIndexes.Find(x => x.Index == elem).Sprite;
                                    //newelem = Peons.peons.Find(x => x.Name == "child guy white shirt").animationIndexes.Find(x => x.Index == elem).Sprite;
                                    //newelem = Peons.peons.Find(x => x.Name == "child girl red pants").animationIndexes.Find(x => x.Index == elem).Sprite;
                                    //newelem = Peons.peons.Find(x => x.Name == "guy yellow pants").animationIndexes.Find(x => x.Index == elem).Sprite;
                                    //newelem = Peons.peons.Find(x => x.Name == "girl purple dress").animationIndexes.Find(x => x.Index == elem).Sprite;
                                    //newelem = Peons.peons.Find(x => x.Name == "child guy blue suits").animationIndexes.Find(x => x.Index == elem).Sprite;
                                    //newelem = Peons.peons.Find(x => x.Name == "child girl white shirt").animationIndexes.Find(x => x.Index == elem).Sprite;





                                    //Console.WriteLine(" new elem is: " + newelem.ToString());

                                    if (newelem != 0) elem = newelem;



                                    drawnPeons++;
                                }
                                else
                                {
                                    elem = 0;
                                }
                            }
                        }
                        #endregion


                    }

                    //Console.WriteLine("Current Animation: " + this.animationIndex + " spriteElement: " + elem);

                    // This is a Guest
                    if (elem != 0 && spriteElement.xFlipped > 127)
                    {   
                        if (j < this.guests.Count+1) 
                        {

                            
                            
                            //Console.WriteLine(j);
                            j++;
                            if (spriteElement.xFlipped == 129)
                            {
                                // Just a dummy text for the xflipped element                            
                                //if (this.hilight) col = Color.Red; else col = Color.White;


                                rectangle = new Rectangle(xoff, yoff, ss.spriteElements[(int)elem].sprite.Width, ss.spriteElements[(int)elem].sprite.Height);

                                sb.Draw(ss.spriteElements[(int)elem].sprite, rectangle, null, Color.White, 0f,
                                    Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

                            }
                            else
                            {

                                sb.Draw(ss.spriteElements[(int)elem].sprite, xy, Color.White);

                            }

                            if (drawIndex)
                            {
                                sb.DrawString(_font, elem.ToString() + " / " + spriteElement.xFlipped.ToString(), xy, Color.Black, 0f, Vector2.One, 0.26f, SpriteEffects.None, 0f);
                                sb.DrawString(_font, elem.ToString() + " / " + spriteElement.xFlipped.ToString(), xy, Color.White, 0f, Vector2.One, 0.25f, SpriteEffects.None, 0f);
                            }
                            if (drawElem)
                            {
                                sb.DrawString(_font, elemIndex.ToString() + " / " + spriteElement.xFlipped.ToString(), xy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                                sb.DrawString(_font, elemIndex.ToString() + " / " + spriteElement.xFlipped.ToString(), xy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);
                            }
                        }
                    }
                    else
                    // This is not a guest, it's an structure element
                    {

                        sb.Draw(ss.spriteElements[(int)elem].sprite, xy, Color.White);

                        if (drawIndex)
                        {
                            sb.DrawString(_font, "[" + elementCounter + "]" + elem.ToString() + " // " + spriteElement.xFlipped.ToString(), xy, Color.Black, 0f, Vector2.One, 0.21f, SpriteEffects.None, 0f);
                            sb.DrawString(_font,"[" + elementCounter + "]" + elem.ToString() + " // " + spriteElement.xFlipped.ToString(), xy, Color.White, 0f, Vector2.One, 0.20f, SpriteEffects.None, 0f);
                        }


                    }

                    elementCounter++;

                }
            }
        }
    }
}
