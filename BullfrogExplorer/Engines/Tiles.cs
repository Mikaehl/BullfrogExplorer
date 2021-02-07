using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using BullfrogExplorer.Loaders;
using BullfrogExplorer.Data;

namespace BullfrogExplorer.Engines
{

    // Working but still some annoying bugs.

    class Tiles
    {
        bool debug = false;
        bool debugOnScreen = false;

        // L'offset a chaque rangé pour avoir un affichage penché


        //Type position à l'intérieur du tableau.
        public struct Position
        {
            public int TileIndex;
            public int X;
            public int Y;

            public Position(int ti, int x, int y)
            {
                TileIndex = ti;
                X = x;
                Y = y;
            }
            
        }

        public struct Viewport
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public Viewport(int x, int y, int w, int h)
            {
                X = x;
                Y = y;
                Width = w;
                Height = h;
            }

        }

        

        public struct Tile
        {
            public int Type;
            public int SpriteIndex;
            public int Offset;
            public bool Visible;

            public Tile(int t, int s, int o, bool v)
            {
                Type = t;
                SpriteIndex = s;
                Offset = o;
                Visible = v;
            }
        }

       

        public struct Map
        {
            private bool debugOnScreen;

            public string Name;
            public int Width;
            public int Height;
            public int TileWidth;
            public int TileHeight;
            Tile[] tiles;
            public Viewport viewport;

            public int offset;

            public Map(string name, int width, int height, int tw, int th, int offset)
            {
                debugOnScreen = false;
                Name = name;
                Width = width;
                Height = height;
                TileWidth = tw;
                TileHeight = th;
                tiles = new Tile[width*height];
                viewport = new Viewport(0, 0, Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT);
                this.offset = offset;
            }

            public void SetViewport(int x, int y)
            {
                viewport.X = x;
                viewport.Y = y;
            }


            public Position TilePicker(int x, int y)
            {
                
                //ma case contient une zone autour des largeurs et hauteurs de la tile
                // mais attention, avec les tiles de TP, il faut enlever la zone de recouvrement, donc mes tiles sont sur une largeur
                // brute et non totale
                // Ensuite la colonne est penché, chaque rangé à un offset de 8 pixels. Donc il faut prendre en compte cet offset pour la tile en X

                //Y: Comme on a un mapage logique, donc pour les x/y du tableau de tile, on commence par le haut, y=0. De y=0 à y=TileHeight (la encore 
                // on a un léger recouvrement donc on parle bien de tileHeight et pas Texture2D.Height) on est sur la rangé 1.
                // Puis de y=TileHeight à y=TileHeight*2, rangé 2. Puis de TileHeight*2 à TileHeight*3 = rangé 3.
                // Donc, cas contcret, TileHeight = 16. y = 12, rangée = 1. y = 22, rangée=2.
                // 12/16 = 0 reste 12.  22/16 = 1 reste 6. Donc ma rangée est de y DivRem TileHeigh + 1 et mon offset est le reste.
                // Effectivement, si y est à 12, je suis dans la première tile et je suis à 12 pixels en y du haut de la tile. Tout va bien.
                // Si y est à 22, je suis dans la deuxième rangée (elle commence à 16) et donc à 6 pixels du haut de la deuxième rangée.
                int Yreste;
                int Yquotient = Math.DivRem(y, this.TileHeight, out Yreste);

                // Donc ma rangée est Yquotient + 1  et mon offset sur y est Yreste

                //Pareil sur X mais oulala, le problème, c'est que la rangée est penché parce que leur système est pénible
                // Donc autant sur la première rangée je n'aurais pas de problème, le calcul est le même, autant sur la deuxième rangé
                // à cause de l'offset de 8 a chaque nouvelle rangée, je serais décalé par rapport à l'affichage (et la réalité voulu).
                // Après, ce n'est pas grave, je connais mon numéro de rangée et je sais qu'a chaque rangé j'ai un offset de +8
                // Donc rangée 1 je suis à 0, rangée 2 à 8, rangée 3 à 16 etc
                // Donc mon algo est Yquotient * 8 - 8. Et en théorie ça fonctionne.
                int onsenfout;
                int TileXOffset = Math.DivRem(Yreste, 2, out onsenfout); // Explication ci-dessous mais en gros debut de la tile, pas d'offset, fin de la tile, offset de 8
                int Xreste;
                


                int Xquotient = Math.DivRem(x - (Yquotient) * this.offset - TileXOffset, this.TileWidth, out Xreste);

                // Donc ma colonne est Xquotient + 1

                // Donc si ma Tile est à la quatrième ligne, 3 rangés, s'il y a des rangées de 10, c'est la Tile 
                // rangées (10) * 4 = 40 (la 40 ième tile est la première de la 3ième rangée) + 3 = 43ième Tile.


                // Mais en vrai tout devient plus compliqué car les tiles sont penchées. Avec overlap de la tile adjacente.
                // ça veut dire tout simplement que le pixel 1/1 appartient à la Tile mais que le pixel 1/8 par exemple appartient à la tile adjacente.
                //      ________ 
                //      \       \ .  Typiquement, les deux points du schéma ci contre sont dans le rectangle de la tile mais ne sont pas dans la tile penchée.
                //       \       \   Il faut donc tracer une droite penchée servant de délimitation
                //       .\_______\
                //     
                //  On est donc a 0,0 coin haut gauche, début de la tile et y=TileHeight x=offset pour le coin en bas à gauche
                //  On va prendre des valeurs, offset=8, TileHeight=16
                //  Début x=0, y=0 fin, x=8, y=16 y = z * x  donc 16=8z (je prend "z" pour bien différencier la variable de mon équation et les x, y des abscisses.
                //  16/8=z z=2   donc y = 2 * x ou à l'inverse x = y/2. Et donc on vérifie, quand y=16, le bas de la tile, x=16/2 = 8.
                //  Et à l'inverse, x=8, y = 2*8 = 16.
                //
                //  Donc j'ai mon équation. Donc je retrie non pas l'offset entier de mon x, mais l'équation soit y/2 mais attention, y est le reste de la
                //  hauteur de la tile, pas le y des coordonnées, sinon problème.
                //  Ensuite c'est pareil de l'autre côté. Donc sur x, la limite de ma tile est à x-8 (x étant la largeur de la tile.
                //  Mais attention. Comme les tiles se recouvrent dans mon système de tile, j'ai mis la largeur à 24 et non 32. Les images font 32, mais la
                //  partie visible de la tile fait 24. Ensuite, je recouvre.
                //  Donc en x, j'ai bien un +24.


                /*********** PERFECT TILE PICKER - MiK 10/21/2018 ****************/

                
                return new Position((this.Width * (Yquotient) + Xquotient)+1, Xreste, Yreste);
                    

            }


            public void AddTile(int index, Tile tile)
            {
                tiles[index] = tile;
            }

            public void UpdateTile(int x, int y, int index)
            {
                Console.WriteLine("X=" + x + " Y=" + y);

                Console.WriteLine("TileWidth=" + this.TileWidth + " TileHeight=" + this.TileHeight);
                int Xreste;
                int offset = y / 2 - 26;
                

                int Xquotient = Math.DivRem(this.viewport.X+x-offset, this.TileWidth, out Xreste);
                Console.WriteLine("viewport.X=" + viewport.X + " Xquotient=" + Xquotient + " Xreste=" + Xreste);


                int Yreste;
                int Yquotient = Math.DivRem(this.viewport.Y+y, this.TileHeight, out Yreste);
                Console.WriteLine("viewport.Y=" + viewport.Y + " Yquotient=" + Yquotient + " Yreste=" + Yreste);

                Console.WriteLine("row should be: " + this.Width * (Yquotient+1));
                //Console.WriteLine("Tile is at: " + (Xquotient - 1));
                Console.WriteLine("Tile is at: " + (Xquotient));
                //int startTile = this.Width * Yquotient + Xquotient - 1;
                
                int tileIndex = this.Width * (Yquotient) + Xquotient;// + 1;
                Console.WriteLine("Tile index=" + tileIndex + " index (New tile)=" + index);
                tileIndex++;
                Console.WriteLine("Tile index=" + tileIndex + " index (New tile)=" + index);

                //TilePicker TMP
                int tp_Yreste;
                int tp_Yquotient = Math.DivRem(y, this.TileHeight, out tp_Yreste);

                int tp_TileXOffset = Math.DivRem(tp_Yreste, 2, out int onsenfout); // Explication ci-dessous mais en gros debut de la tile, pas d'offset, fin de la tile, offset de 8
                int tp_Xreste;

                int tp_Xquotient = Math.DivRem(x - (tp_Yquotient) * this.offset - tp_TileXOffset, this.TileWidth, out tp_Xreste);

                Console.WriteLine("TilePicker(Position)=" + (this.Width * (tp_Yquotient) + tp_Xquotient)+1);

                //this.tiles[tileIndex].SpriteIndex = index;
                //this.tiles[TilePicker(this.viewport.X + x, viewport.Y + y).TileIndex].SpriteIndex = index;
                //this.tiles[TilePicker(this.viewport.X, viewport.Y + y).TileIndex].SpriteIndex = index;
                this.tiles[tileIndex].SpriteIndex = index;

                /*** TODO Corriger le tiler picker sa mère ****/

            }


            public void Generate(int index)
            {
                int offset = 0;
                int Counter = 0;
                for (int i = 0; i < this.Width*this.Height; i++)
                {

                    if (Counter == this.Width)
                    {
                        offset = offset + 8;
                        Counter = 0;
                    }
                    if (offset == 48) offset = 0;
                    AddTile(i, new Tile(1, index, offset, true));
                    Counter++;

                }

            }


            public void Update(GameTime gt)
            {
                // Should update anything there.
            }


            public void Draw(SpriteBatch sb, SpritesSheet ss, SpriteFont font)
            {
                int currentRow = 1;

                // Ok, c'est pas mal mais en fait il faut que je déplace le view port dans le tableau, pas pour l'écran


                int tileX = Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO;// + this.TileWidth;
                //int tileY = -this.TileHeight/2;// Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO;
                int tileY = 0;

                //ok, v2
                int Xreste;
                int Xquotient = Math.DivRem(this.viewport.X, this.TileWidth, out Xreste);

                int Yreste;
                int Yquotient = Math.DivRem(this.viewport.Y, this.TileHeight, out Yreste);
                int offset = 8 * Yquotient;

                // En gros dans quotient, j'ai le nombre exacte de tile à partir de la gauche, donc j'affiche à partir de cette Tile
                // dans reste, j'ai les pixel restant donc j'affiche à partir de la.

                // Et pareil pour Y



                int row;
                int onsenfout;
                row = Math.DivRem((Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO), this.TileWidth, out onsenfout);

                //int startTile = this.Width * Yquotient + Xquotient-1;

                int startTile = this.TilePicker(this.viewport.X, this.viewport.Y).TileIndex;
                /*
                Console.WriteLine("startTile= " + (startTile+1));
                Console.WriteLine("viewport.X= " + viewport.X + " viewport.Y=" + viewport.Y);
                */
                row++;
                //row++;
                //Console.WriteLine("row= " + row);

                int currentTile = row;
                int index;


                int elem = startTile + currentTile;
                /*
                Console.WriteLine("currentTile= " + currentTile + " row=" + row);
                Console.WriteLine("Xquotient= " + Xquotient + " Yquotient=" + Yquotient);
                Console.WriteLine("Xreste= " + Xreste + " Yreste=" + Yreste);
                */

                //while (tileX > -this.TileWidth && tileY < Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO)
                while (tileX > 0 && tileY < Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO)
                {
                    //Vector2 coords = new Vector2(tileX - Xreste, tileY - Yreste);

                    index = startTile + currentTile;

                    

                    if (index > -1 && index < this.tiles.Length)
                    {
                        int spriteIndex = this.tiles[index].SpriteIndex;
                        elem = startTile + currentTile;

                        int offsetY = this.TileHeight - ss.spriteElements[spriteIndex].sprite.Height;
                        int offsetX = this.TileWidth - ss.spriteElements[spriteIndex].sprite.Width;


                        
                        Vector2 coords = new Vector2(tileX - Xreste + offsetX + offset, tileY - Yreste/2 + offsetY);

                        if (!debugOnScreen)
                        {
                            if (TilePicker(216, 76).TileIndex == index)
                            {
                                TilePicker(216, 76);
                                sb.Draw(ss.spriteElements[spriteIndex].sprite, coords, Color.Red);
                                Vector2 xy = new Vector2(tileX + 2 - Xreste + offsetX + offset, tileY + 2 - Yreste / 2 + offsetY);
                                sb.DrawString(font, index.ToString(), xy, Color.Black, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
                                sb.DrawString(font, index.ToString(), xy, Color.Gray, 0f, Vector2.One, 0.27f, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                sb.Draw(ss.spriteElements[spriteIndex].sprite, coords, Color.White);
                            }
                        }
                        else
                        {

                            if (index % 2 > 0)
                            {
                                sb.Draw(ss.spriteElements[spriteIndex].sprite, coords, Color.Gray);


                                Vector2 xy = new Vector2(tileX + 2 - Xreste + offsetX + offset, tileY + 2 - Yreste / 2 + offsetY);
                                sb.DrawString(font, index.ToString(), xy, Color.Black, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
                                sb.DrawString(font, index.ToString(), xy, Color.Gray, 0f, Vector2.One, 0.27f, SpriteEffects.None, 0f);
                                xy = new Vector2(tileX + 2 - Xreste + offsetX + offset, tileY + 5 - Yreste/ 2 + offsetY);
                                sb.DrawString(font, offset.ToString(), xy, Color.Black, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
                                sb.DrawString(font, offset.ToString(), xy, Color.Gray, 0f, Vector2.One, 0.27f, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                sb.Draw(ss.spriteElements[spriteIndex].sprite, coords, Color.White);
                                Vector2 xy = new Vector2(tileX + 2 - Xreste + offsetX + offset, tileY + 2 - Yreste / 2 + offsetY);
                                sb.DrawString(font, index.ToString(), xy, Color.Black, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
                                sb.DrawString(font, index.ToString(), xy, Color.White, 0f, Vector2.One, 0.27f, SpriteEffects.None, 0f);
                                xy = new Vector2(tileX + 2 - Xreste + offsetX + offset, tileY + 5 - Yreste/ 2 + offsetY);
                                sb.DrawString(font, offset.ToString(), xy, Color.Black, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
                                sb.DrawString(font, offset.ToString(), xy, Color.White, 0f, Vector2.One, 0.27f, SpriteEffects.None, 0f);

                            }

                            Vector2 zixy = new Vector2(tileX + 2 - Xreste + offsetX + offset, tileY + 8 - Yreste / 2);
                            sb.DrawString(font, Yreste.ToString(), zixy, Color.Black, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
                            sb.DrawString(font, Yreste.ToString(), zixy, Color.White, 0f, Vector2.One, 0.27f, SpriteEffects.None, 0f);

                        }

                    }



                    /*xy = new Vector2(tileX + 10 - Xreste, tileY + 10 - Yreste);
                    sb.DrawString(font, this.tiles[index].Offset.ToString(), xy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                    sb.DrawString(font, this.tiles[index].Offset.ToString(), xy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);
                    */

                    currentTile--;
                    tileX = tileX - this.TileWidth;

                    if (currentTile == 0)
                    {
                        currentRow++;
                        tileX = Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO;
                        tileY = tileY + this.TileHeight;// * Settings.PIXEL_RATIO);
                                                        // Je remet au max mon compteur de décalage de row
                        startTile = startTile + this.Width;
                        //startTile = this.TilePicker(tileX, tileY).TileIndex;
                        // JE me repositionne sur la bonne tile
                        currentTile = row;

                        offset = offset + 8;
                        //if (offset < 48*2) offset = offset + 8; else offset = 0;

                        //currentTile = currentTile + offset / 8;

                    }
                    

                }

                if (debugOnScreen)
                { 
                    Vector2 thexy = new Vector2(300, 2);
                    sb.DrawString(font, startTile.ToString(), thexy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                    sb.DrawString(font, startTile.ToString(), thexy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);
                }

            /*
            // je parcours l'écran en fonction de la taille définit. Cela me donnera le nombre de tile à afficher
            while (tileX > -this.TileWidth && tileY < Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO)
            {
                //Du coup, comme je dois partir le plus a droite, la tile la plus à droite est la tile de gauche + le nombre de
                //tile possible a afficher dans une row


                // Donc la je sais que sur une ligne affiché, je peux mettre "row" tiles (en nombre, donc)
                // Comme je commence à droite, celle de droite sera start tile + row
                // Mais il me manque le Y
                // Donc la même

                Console.WriteLine("startTile=" + startTile + " currentTile=" + currentTile);
                Vector2 coords = new Vector2(tileX + viewport.X, tileY + viewport.Y);
                //if (startTile + currentTile > 0)
                //{
                    index = this.tiles[startTile + currentTile].SpriteIndex;
                    int elem = startTile + currentTile;
                    sb.Draw(ss.spriteElements[index].sprite, coords, Color.White);


                    Vector2 xy = new Vector2(tileX + 5 + viewport.X, tileY + 5 + viewport.Y);
                    /*
                                        sb.DrawString(font, row.ToString(), xy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                                        sb.DrawString(font, row.ToString(), xy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);
/*                        */
            /*                    xy = new Vector2(tileX + 10 + viewport.X, tileY + 10 + viewport.Y);
                                sb.DrawString(font, elem.ToString(), xy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                                sb.DrawString(font, elem.ToString(), xy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);
                            //}

                            //#TODO le décalage dans la tile
        /*
                            tileX = tileX - this.TileWidth;

                            // Ensuite, je ferais la tile de droite - 1 donc je dois décrémenter row
                            currentTile--;

                            // Si j'ai fais toute une ligne, j'incrémente sur Y
                            if (currentTile == 0)
                            {
                                tileX = Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO;
                                tileY = tileY + this.TileHeight;// * Settings.PIXEL_RATIO);
                                // Je remet au max mon compteur de décalage de row
                                startTile = startTile + this.Width;
                                // JE me repositionne sur la bonne tile
                                currentTile = row;

                            }



                        }
                    */




            /*


            int tilePart = 0;
            row = (Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO) / this.TileWidth+1;

            while (tileX>-this.TileWidth && tileY < Settings.SCREEN_HEIGHT/ Settings.PIXEL_RATIO)
            //for (int i = 0; i < 500; i++)
            {

                Vector2 coords = new Vector2(tileX+viewport.X, tileY+viewport.Y);
                //Console.WriteLine("X: " + tileX.ToString() + " Y: " + tileY.ToString());




                int index = this.tiles[tilePart].SpriteIndex;


                sb.Draw(ss.spriteElements[index].sprite, coords, Color.White);

                int elem = row-tilePart;
                Vector2 xy = new Vector2(tileX + 5 + viewport.X, tileY + 5 + viewport.Y);

                sb.DrawString(font, row.ToString(), xy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                sb.DrawString(font, row.ToString(), xy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);
                xy = new Vector2(tileX + 10 + viewport.X, tileY + 10 + viewport.Y);
                sb.DrawString(font, elem.ToString(), xy, Color.Black, 0f, Vector2.One, 0.4f, SpriteEffects.None, 0f);
                sb.DrawString(font, elem.ToString(), xy, Color.White, 0f, Vector2.One, 0.37f, SpriteEffects.None, 0f);

                tileX = tileX - this.TileWidth;// * Settings.PIXEL_RATIO);
               //row--;

                if (tileX < -this.TileHeight)
                {
                    tileX = Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO;
                    tileY = tileY + this.TileHeight;// * Settings.PIXEL_RATIO);
                    row = row + row;
                }
                tilePart++;


            }
            */
        }



        }

        Map map;

        Tiles()
        {

            

        }

        void Generate(int x, int y)
        { // #TOFIX
            map = new Map("basemap", x, y, 16, 31, 8);

            for (int i=0; i<x*y; i++)
            {
                map.AddTile(i, new Tile(1, 1, 0, true));
            }

        }



        void Update()
        {

        }


        public void Draw(SpriteBatch sb, SpritesSheet ss)
        {

            // Dans l'idée je récupère le viewport et j'affiche ce qui est dans le viewport

            int tileIndex = 1;
            int tileX = Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO;
            int tileY = -16;// Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO;

            for (int i = 0; i < 500; i++)
            {
                Vector2 coords = new Vector2(tileX, tileY);
                //Console.WriteLine("X: " + tileX.ToString() + " Y: " + tileY.ToString());
                sb.Draw(ss.spriteElements[tileIndex].sprite, coords, Color.White);
                tileX = tileX - 24;// * Settings.PIXEL_RATIO);
                if (tileX < -24)
                {
                    tileX = Settings.SCREEN_WIDTH / Settings.PIXEL_RATIO;
                    tileY = tileY + 16;// * Settings.PIXEL_RATIO);
                }


            }
        }

    }
}
