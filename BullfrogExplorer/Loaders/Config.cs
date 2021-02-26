using System;
using BullfrogExplorer.Data;
using Microsoft.Xna.Framework.Input;

namespace BullfrogExplorer.Loaders
{
    class Config
    {
        public Config()
        {

        }

        public bool LoadFile(string s)
        {
            string[] c_lines = System.IO.File.ReadAllLines(s);
            foreach (string c_line in c_lines)
            {
                string[] c_s = c_line.Split('=');
                switch (c_s[0])
                {
                    case "DATAPATH":
                        Settings.datapath = c_s[1];
                        break;
                    case "FULLSCREEN":
                        if (c_s[1] == "N") Settings.Fullscreen = false; else Settings.Fullscreen = true;
                        break;
                    case "PLAYINTRO":
                        if (c_s[1] == "N") Settings.playIntro = false; else Settings.playIntro = true;
                        break;
                    case "POSITION":
                        string[] pos = c_s[1].Split(',');
                        Settings.Position = new Microsoft.Xna.Framework.Point(int.Parse(pos[0]), int.Parse(pos[1]));
                        break;
                    case "LANG":
                        Settings.Lang = c_s[1];
                        break;
                    case "NAV":
                        Settings.movement.Up = (Keys)Enum.Parse(typeof(Keys), c_s[1].Substring(0, 1), true);
                        Settings.movement.Down = (Keys)Enum.Parse(typeof(Keys), c_s[1].Substring(1, 1), true);
                        Settings.movement.Left = (Keys)Enum.Parse(typeof(Keys), c_s[1].Substring(2, 1), true);
                        Settings.movement.Right = (Keys)Enum.Parse(typeof(Keys), c_s[1].Substring(3, 1), true);
                        break;
                    case "RESOLUTION":
                        string[] resolution = c_s[1].Split('x');
                        Settings.SCREEN_WIDTH = int.Parse(resolution[0]); // I know, I know
                        Settings.SCREEN_HEIGHT = int.Parse(resolution[1]); // same, same
                        break;
                    case "WINDOW":
                        string[] window = c_s[1].Split('x');
                        Settings.windowWidth = int.Parse(window[0]);
                        Settings.windowHeight = int.Parse(window[1]);
                        break;
                }
            }
            return true;
        }
    }
}
