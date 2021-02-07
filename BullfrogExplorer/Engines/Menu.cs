using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Input;

using BullfrogExplorer.Loaders;
using BullfrogExplorer.Data;

namespace BullfrogExplorer.Engines
{
    class Menu
    {


        bool debug = false;

        Text text;
        Vector2 position;

        private SpritesSheet spritesSheet;

        public enum MenuState { normal, hilight, grey};
      
        public class MenuElement
        {
            public int Index;
            public MenuState State;
            public string Text;
            public string Label;
            public Keys Key;
            public Rectangle Area;

            public MenuElement(int i, MenuState st, string t, string s, Keys k, Rectangle r)
            {
                Index = i;
                State = st;
                Text = t;
                Label = s;
                Key = k;
                Area = r;
            }

            public MenuElement(int i, MenuState st, string t, string s, Keys k)
            {
                Index = i;
                State = st;
                Text = t;
                Label = s;
                Key = k;
            }

            public MenuElement(int i, MenuState st, string t, string s)
            {
                Index = i;
                State = st;
                Text = t;
                Label = s;
            }

            public MenuElement(int i, MenuState st, string t)
            {
                Index = i;
                State = st;
                Text = t;
                Label = "";
                Area = new Rectangle(new Point(0,0), new Point(0,0));
            }
        }

        public List<MenuElement> menuList = new List<MenuElement>();

        public Menu(SpritesSheet ss, List<MenuElement> ml, Vector2 p)
        {

            text = new Text(ss);
            this.spritesSheet = ss;
            this.menuList = ml;
            this.position = p;
        }

        public string Update(GameTime gt, MouseState ms, KeyboardState ks)
        {
            string r = "";

            for (int i = 0; i < menuList.Count; i++)
            {
                

                if (menuList[i].State != MenuState.grey)
                {
                    if (ks.IsKeyDown(menuList[i].Key)) r = menuList[i].Label;

                    if (menuList[i].Area.Intersects(new Rectangle(new Point(ms.Position.X / Settings.PIXEL_RATIO, ms.Position.Y / Settings.PIXEL_RATIO), new Point(1, 1))))
                    {
                        menuList[i].State = MenuState.hilight;
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            r = menuList[i].Label;
                        }
                    }
                    else
                    {
                        menuList[i].State = MenuState.normal;
                    }
                }
            }

            return r;

        }


        public void Draw(SpriteBatch sb)
        {
            text.SetPosition(this.position);
            text.SetInterline(3);
            text.SetColor(Color.Red);

            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].State == MenuState.grey) text.SetColor(Color.ForestGreen);
                if (menuList[i].State == MenuState.hilight) text.SetColor(Color.White);
                if (menuList[i].State == MenuState.normal) text.SetColor(Color.OliveDrab);

                menuList[i].Area = text.WriteLine(sb, menuList[i].Key.ToString() + " " + menuList[i].Text);
            }
        }
    }
}
