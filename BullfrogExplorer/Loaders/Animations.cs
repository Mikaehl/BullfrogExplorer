using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


namespace BullfrogExplorer.Loaders
{
    class Animations
    {
        private bool debug = false;
        public string name;

        public struct AnimLabel
        {
            public int index;
            public string label;

            public AnimLabel(int i, string s)
            { 
                index = i;
                label = s;
            }
        }

        public List<AnimLabel> AnimationsLabels = new List<AnimLabel>();

        // Les animations sont des frames. Chaque Animation envois vers une frame et la frame vers celle d'après
        //Toutes les animations sont donc les unes a la suite des autres dans le fichiers MSTA-0.ANI et le programme
        //sait qu'il doit exécuter l'animation numéro X. Ensuite ce qu'il y a dans l'animation est donc dans les
        //fichiers de data

        public struct SpriteAnim
        {
            public int index;

            public SpriteAnim(int i)
            {
                index = i;
            }

        }

        public List<SpriteAnim> spritesAnimations = new List<SpriteAnim>();

        public struct SpriteElement
        {
            public long sprite;
            public int xOffset;
            public int yOffset;
            public long xFlipped;
            public int next;


            public SpriteElement(long s, int x, int y, long xf, int n)
            {
                sprite = s;
                xOffset = x;
                yOffset = y;
                xFlipped = xf;
                next = n;
            }
        }

        public struct SpriteFrame
        {
            public int spriteElement;
            public int width;
            public int height;
            public long flags;
            public int nextFrame;
            public List<SpriteElement> spritesElements;

            public SpriteFrame(int s, int w, int h, long f, int n, List<SpriteElement> se)
            {
                spriteElement = s;
                width = w;
                height = h;
                flags = f;
                nextFrame = n;
                spritesElements = se;
            }

        }

        public List<SpriteFrame> spritesFrames = new List<SpriteFrame>();

        public List<SpriteElement> spritesElements = new List<SpriteElement>();

        public Animations()
        {
        }


        public void ToggleDebug()
        {
            debug = !debug;
        }

        // On récupère les éléments sprites depuis un premier sprite élément (puis on suit la chaîne, en fait).
        // on connait forcément le premier puisqu'il est indiqué dans une frame
        public List<SpriteElement> GetSpriteElements(int elem)
        {
            List<SpriteElement> elements = new List<SpriteElement>();

            int y = 0;
            while (elem != 0 && y < 50)
            {

                if (this.debug)
                {
                    Console.WriteLine("New Sprite Elem [" + elem + "], sprite: " + spritesElements[elem].sprite / 6);
                }
                elements.Add(spritesElements[elem]);

                elem = spritesElements[elem].next;

                y++;
                if (y == 50) {
                    if (this.debug) Console.WriteLine(" *** INFINITE LOOP spritesElements ***");
                    //elements.Clear();
                };
            }
            return elements;
        }

        // Récupération de la liste des frames d'une animation
        // On part du numéro d'animiation dans la liste des animations du fichier start
        public List<SpriteFrame> GetFrames(int anim)
        {
            // En gros, j'ai mon numéro d'animations, je vais dans le fichier start, je cherche le record correspondant au numéro
            // de l'animation, ça me renvois le point de départ des frames dans le fichier frame
            // Ensuite je parcours les frames jusqu'a ce que next = 0 ou le flag start soit poppé deux fois;
            List<SpriteFrame>  frames = new List<SpriteFrame>();
            int index = this.spritesAnimations[anim-1].index;
            
            if (this.debug)
            {
                Console.WriteLine("First frame to get: " + index);
            }
            bool loop = false;
            int i = 0;
            while (!loop && i < 250 && this.spritesFrames.Count() > index)
            {
                SpriteFrame currentFrame;
                currentFrame = this.spritesFrames[index];
                if (this.debug)
                {
                    Console.WriteLine("[Frame#" + index + "] Adding frame: " + index + " flag:" + spritesFrames[index].flags +
                        " [" + spritesFrames[index].width + "x" + spritesFrames[index].height + "]"
                        );
                    Console.WriteLine("[Frame#" + index + "] First Elem: " + spritesFrames[index].spriteElement);
                    Console.WriteLine("[Frame#" + index + "]  Sprite : " + spritesElements[spritesFrames[index].spriteElement].sprite / 6 +
                     " next sprite elem: " + spritesElements[spritesFrames[index].spriteElement].next);
                    Console.WriteLine("[Frame#" + index + "]  [" + spritesElements[spritesFrames[index].spriteElement].xOffset +
                        ":" + spritesElements[spritesFrames[index].spriteElement].yOffset +
                        "/" + spritesElements[spritesFrames[index].spriteElement].xFlipped + "]");
                }

                int se_index = spritesFrames[index].spriteElement;

                if (this.debug) Console.WriteLine("[Frame#" + index + "] GetSpriteElements: ");
                
                currentFrame.spritesElements = this.GetSpriteElements(se_index);
                if (this.debug) Console.WriteLine("[Frame#" + index + "] ----");

                frames.Add(currentFrame);

                index = spritesFrames[index].nextFrame;
                if (this.debug)
                {
                    Console.WriteLine("Got frame: " + index + " flag:" + spritesFrames[index].flags);
                    Console.WriteLine("First Elem: " + spritesFrames[index].spriteElement);
                    Console.WriteLine(" Sprite : " + spritesElements[spritesFrames[index].spriteElement].sprite/6 +
                        " next sprite elem: " + spritesElements[spritesFrames[index].spriteElement].next);
                }
                if (spritesFrames[index].flags == 256)
                {
                    loop = true;
                    if (this.debug)
                    {
                        Console.WriteLine("looping, stopping");
                    }
                }
                i++;
                if (i > 240) {
                    if (this.debug) Console.WriteLine(" *** INFINITE LOOP frames  ***");
                    frames.Clear();
                };
            }

            return frames;
        }

        // type: 0: start, 1: frames, 2: sprite elems
        public bool LoadFiles(string startFile, string framesFile, string elementsFile, string labelsFile)
        {
            if (!File.Exists(startFile) && !File.Exists(framesFile) && !File.Exists(elementsFile) && !File.Exists(labelsFile))
            {
                return false;
            }
            else
            {
                {
                    /* Labels */
                    Console.WriteLine(" - LoadContent() Reading file: " + labelsFile);
                    using (FileStream fs = File.OpenRead(labelsFile))
                    {
                        int start = 53010;

                        char c = (char)0x00;

                        int i=0;
                        byte[] b = new byte[60];
                        fs.Seek(start,0);
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            string s = Encoding.Default.GetString(b).Split(c)[0];
                            
                            AnimationsLabels.Add(new AnimLabel(i++, s));
                                                        
                            if (this.debug) Console.WriteLine("         - Index: " + i + " Label: " + s);
                        }
                    }

                    Console.WriteLine(" - LoadContent() Reading file: " + startFile);
                    using (FileStream fs = File.OpenRead(startFile))
                    {
                        int counter = 0;
                        byte[] b = new byte[2];
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            counter++;
                            int framepos = b[0] + (b[1] << 8);

                            spritesAnimations.Add(new SpriteAnim(framepos));
                            if (this.debug)
                            {
                                Console.WriteLine("Frame [" +  counter + "] start: " + framepos + " seek position: " + framepos * 8);
                            }

                        }
                        string[] fn = fs.Name.Split('\\');
                        this.name = fn.Last();

                    }
                }
                // frames
                //if (type ==1)
                {
                    Console.WriteLine(" - LoadContent() Reading file: " + framesFile);
                    using (FileStream fs = File.OpenRead(framesFile))
                    {
                        int counter = 0;
                        byte[] b = new byte[8];
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            counter++;
                            int spriteelem = b[0] + (b[1] << 8);
                            int width = b[2];
                            int height = b[3];
                            long flags = b[4] + (b[5] << 8);
                            int next = b[6] + (b[7] << 8);
                            // c'est pas fou mais au moins je garde la compatibilité
                            // #TODO améliorer le truc
                            List<SpriteElement> se = new List<SpriteElement>();

                            spritesFrames.Add(new SpriteFrame(spriteelem, width, height, flags, next, se));

                            if (this.debug)
                            {
                                Console.WriteLine("Frame: " + (counter-1) + " Sprite Element Index: " + spriteelem + " width=" + width + " height= " + height +
                                    " flags: " + flags.ToString("X") + " next frame index: " + next);
                            }

                        }

                    }

                }

                //sprite element
                //if (type == 2)
                {
                    Console.WriteLine(" - LoadContent() Reading file: " + elementsFile);
                    using (FileStream fs = File.OpenRead(elementsFile))
                    {
                        int counter = 0;
                        byte[] b = new byte[10];
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            counter++;
                            int sprite = b[0] + (b[1] << 8);
                            int xoff = ((byte)b[2] + ((byte)b[3] << 8));
                            int yoff = ((byte)b[4] + ((byte)b[5] << 8));

                            if (xoff > 65536 / 2)
                            {
                                xoff = 65536 - xoff;
                            }
                            else
                            {
                                xoff = -xoff;
                            }

                            if (yoff > 65536 / 2)
                            {
                                yoff = 65536 - yoff;
                            }
                            else
                            {
                                yoff = -yoff;
                            }
                            

                            long xflip = b[6] + (b[7] << 8);
                            int next = b[8] + (b[9] << 8);

                            // sprite indique l'adresse de l'entrée dans le fichier tab, pas l'index du sprite, mais son entrée, donc on divise par 6 car une entrée prend 6 bytes.

                            spritesElements.Add(new SpriteElement(sprite, xoff, yoff, xflip, next));


                            if (this.debug)
                            {
                                Console.WriteLine("SpriteElem: " + (counter-1) + 
                                    " Sprite Index: " + (sprite/6) + "(" + sprite.ToString("X") + ")" +
                                    " xoffset=" + xoff + " yoffset= " + yoff +
                                    " xflipped: " + xflip + " next sprite elem index: " + next);
                            }

                        }

                    }

                }

                return true;
            }
        }
    }
}
