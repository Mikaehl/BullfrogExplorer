using System;
using System.Collections.Generic;
using System.Linq;
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
        float width;
        float height;

        Color menuStateGrey;
        Color menuStateHilight;
        Color menuStateNormal;




        private SpritesSheet spritesSheet;

        public enum MenuState { normal, hilight, grey };

        #region - class MenuElement - 
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
                Area = new Rectangle(new Point(0, 0), new Point(0, 0));
            }
        }
        #endregion

        public List<MenuElement> menuList = new List<MenuElement>();

        public Menu(SpritesSheet ss, List<MenuElement> ml, Vector2 p)
        {

            text = new Text(ss);
            this.spritesSheet = ss;
            this.menuList = ml;
            this.position = p;
            this.width = 0;
            this.height = 0;
            this.menuStateGrey = Color.ForestGreen;
            this.menuStateHilight = Color.White;
            this.menuStateNormal = Color.OliveDrab;
        }

        public Menu(SpritesSheet ss, List<MenuElement> ml, Vector2 p, float w, float h)
        {

            text = new Text(ss);
            this.spritesSheet = ss;
            this.menuList = ml;
            this.position = p;
            this.width = w;
            this.height = h;
            this.menuStateGrey = Color.ForestGreen;
            this.menuStateHilight = Color.White;
            this.menuStateNormal = Color.OliveDrab;

        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetColors(Color menuStateGrey, Color menuStateHilight, Color menuStateNormal)
        {
            this.menuStateGrey = menuStateGrey;
            this.menuStateHilight = menuStateHilight;
            this.menuStateNormal = menuStateNormal;
        }


        public string GetLabel(int index)
        {
            if (index > 0) return menuList.Find(x => x.Index == index).Label; else return "";
        }

        public string GetText(int index)
        {
            if (index > 0) return menuList.Find(x => x.Index == index).Text; else return "";
        }

        public int Update(GameTime gt, MouseState ms, KeyboardState ks)
        {
            int r = 0;

            for (int i = 0; i < menuList.Count; i++)
            {


                if (menuList[i].State != MenuState.grey)
                {
                    if (ks.IsKeyDown(menuList[i].Key))
                    {
                        if (ks.IsKeyDown(Keys.LeftControl))
                        {
                            r = menuList[i].Index + 26;
                            break;
                        }
                        else
                        {
                            r = menuList[i].Index;
                            break;
                        }
                    }
                    if (menuList[i].Area.Intersects(new Rectangle(new Point(ms.Position.X / Settings.PIXEL_RATIO, ms.Position.Y / Settings.PIXEL_RATIO), new Point(1, 1))))
                    {
                        menuList[i].State = MenuState.hilight;
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            r = menuList[i].Index;
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
            float maxX = 0;

            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].State == MenuState.grey) text.SetColor(menuStateGrey);
                if (menuList[i].State == MenuState.hilight) text.SetColor(menuStateHilight);
                if (menuList[i].State == MenuState.normal) text.SetColor(menuStateNormal);

                menuList[i].Area = text.WriteLine(sb, menuList[i].Key.ToString() + " " + menuList[i].Text);
                if (menuList[i].Area.Width > maxX) maxX = menuList[i].Area.Width;

                if (this.height > 0 && text.position.Y + text.height - this.position.Y > this.height)
                {
                    if (this.width > 0 && text.position.X + maxX - this.position.X > this.width)
                        break;

                    text.SetPosition(new Vector2(text.position.X + maxX, this.position.Y));

                }



            }
        }
    }
}
