using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullfrogExplorer.Data
{

    /* Needs to be rework */

    public class Guests
    {
        public static int currentGuestIndex = 1;

        public struct Guest {
            public int Index; // useless almost but anyway
            public string Name; // The name I give them, quick reminder
            public int Gender; //0=guy, 1=girl
            public int Age; //10=children, 20=adult
            public List<AnimationIndex> animationIndexes;

            public Guest(int i, string n, int g, int a)
            {
                Index = i;
                Name = n;
                Gender = g;
                Age = a;
                animationIndexes = new List<AnimationIndex>();
            }

            public Guest(int i, string n, int g, int a, List<AnimationIndex> ai)
            {
                Index = i;
                Name = n;
                Gender = g;
                Age = a;
                
                animationIndexes = ai;
            }

            public void AddAnimationIndex(AnimationIndex ai)
            {
                animationIndexes.Add(ai);
            }

            public int GetAnimationIndex(int index)
            {
                return animationIndexes.Find(x => x.Index.Equals(index)).Sprite;
            }

        }

        public struct AnimationIndex {
            public int Index;
            public string Name;
            public int Sprite;

                public AnimationIndex(int i, string n, int s)
                {
                    Index = i;
                    Name = n;
                    Sprite = s;
                }
        }

        public static List<Guest> guests = new List<Guest>();

        public Guests()
        {
            int i = 0;
            guests.Add(new Guest(i++, "guy red jacket", 0, 20));
            guests.Add(new Guest(i++, "girl green dress", 0, 20));
            guests.Add(new Guest(i++, "child guy white shirt", 0, 10));
            guests.Add(new Guest(i++, "child girl red pants", 1, 10));
            guests.Add(new Guest(i++, "guy yellow pants", 0, 20));
            guests.Add(new Guest(i++, "girl purple dress", 0, 20));
            guests.Add(new Guest(i++, "child guy blue suits", 0, 10));
            guests.Add(new Guest(i++, "child girl white shirt", 1, 10));

            i = 1;
            /***** girl green dress ******/

            // MOUVEMENT
            for (int j = 1; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 146));
            }

            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 211));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j-65).ToString(), j + 146));
            }


            // SNAKE
            for (int j = 97; j < 105; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "Snake" + (j-96).ToString(), j+137));
            }

            i++;
            /***** CHILD GUY WHITE SHIRT ******/

            // MOUVEMENT
            for (int j = 1; j < 5; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 269));
            }
            for (int j = 5; j < 9; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 275));
            }
            for (int j = 10; j < 13; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 281));
            }
            for (int j = 13; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 288));
            }

            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 366));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j - 65).ToString(), j + 301));
            }


            // SNAKE
            for (int j = 97; j < 104; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "Snake" + (j - 96).ToString(), j + 301));
            }
            // A vérifier
            guests[i].AddAnimationIndex(new AnimationIndex(104, "Snake8", 380));

            i++;
            /***** CHILD GIRL RED PANT *****/

            // MOUVEMENT
            for (int j = 1; j < 5; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 438));
            }
            for (int j = 5; j < 9; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 444));
            }
            for (int j = 10; j < 13; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 450));
            }
            for (int j = 13; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 456));
            }


            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 491));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j - 65).ToString(), j + 460));
            }

            // SNAKE
            for (int j = 97; j < 105; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "Snake" + (j - 96).ToString(), j + 454));
            }

            i++;
            /***** ADULT GUY YELLOW PANTS *****/

            // MOUVEMENT
            
            for (int j = 1; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2015));
            }

            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 2087));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j - 65).ToString(), j + 2022));
            }


            // SNAKE
            // WTF seriously
            guests[i].AddAnimationIndex(new AnimationIndex(97, "Snake1", 2118));
            guests[i].AddAnimationIndex(new AnimationIndex(98, "Snake2", 2118));
            guests[i].AddAnimationIndex(new AnimationIndex(99, "Snake3", 2120));
            guests[i].AddAnimationIndex(new AnimationIndex(100, "Snake4", 2126));
            guests[i].AddAnimationIndex(new AnimationIndex(101, "Snake5", 2127));
            guests[i].AddAnimationIndex(new AnimationIndex(102, "Snake6", 2121));
            guests[i].AddAnimationIndex(new AnimationIndex(103, "Snake7", 2122));
            guests[i].AddAnimationIndex(new AnimationIndex(104, "Snake8", 2123));

            i++;
            /***** ADULT GIRL PURPLE DRESS *****/
            
            // MOUVEMENT
            for (int j = 1; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2159));
            }

            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 2220));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j - 65).ToString(), j + 2155));
            }

            // SNAKE
            for (int j = 97; j < 105; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "Snake" + (j - 96).ToString(), j + 2146));
            }

            i++;
            /***** CHILD GUY BLUE SUIT *****/

            // MOUVEMENT
            for (int j = 1; j < 5; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2285));
            }
            for (int j = 5; j < 9; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2291));
            }
            for (int j = 10; j < 13; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2297));
            }
            for (int j = 13; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2303));
            }

            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 2382));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j - 65).ToString(), j + 2317));
            }


            // SNAKE
            for (int j = 97; j < 105; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "Snake" + (j - 96).ToString(), j + 2317));
            }

            i++;
            /***** CHILD GIRL WHITE SHIRT *****/

            // MOUVEMENT
            for (int j = 1; j < 5; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2458));
            }
            for (int j = 5; j < 9; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2464));
            }
            for (int j = 10; j < 13; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2470));
            }
            for (int j = 13; j < 17; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "walk" + (j).ToString(), j + 2476));
            }

            // STANDING
            guests[i].AddAnimationIndex(new AnimationIndex(65, "Stand", 2547));

            // SIT + WHEEL
            for (int j = 66; j < 76; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "wheel" + (j - 65).ToString(), j + 2482));
            }

            // SNAKE
            for (int j = 97; j < 105; j++)
            {
                guests[i].AddAnimationIndex(new AnimationIndex(j, "Snake" + (j - 96).ToString(), j + 2474));
            }
       }

        public int Indexof(string s, int i)
        {
            return guests.Find(x => x.Name == s).animationIndexes.Find(x => x.Index == i).Sprite;
        }
        



    }
}
