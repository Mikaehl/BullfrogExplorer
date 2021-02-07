using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace BullfrogExplorer.Data
{
    class Settings
    {
        // STATIC FIELDS

        public static int PIXEL_RATIO = 4;
        public static int SCREEN_WIDTH = 320 * PIXEL_RATIO;
        public static int SCREEN_HEIGHT = 200 * PIXEL_RATIO;

        public static Color BACKGROUND_COLOR = Color.Black;

        public static int FRAMERATE = 60;

        public static bool Fullscreen = true;
        public static Point Position = new Point(2240, 190);
        public static string Lang = "ENGLISH";
        public static string AnimationSerie = "MSTA-0.ANI";
        public static string mousePointerSpritesheet = "MPOINTER";

        public static bool showFooter = true;

        #region * INTRO *
        public static bool playIntro = true;
        public static double introFadeDelay = .035;
        public static float introMyAlphaValue = 0;
        public static float introMyFadeIncrement = 0.1f;
        #endregion

        public static string datapath = "";

        public static bool showsprites = false;
        public static bool AnimNextFrame = false;
        public static bool playAnim = false;
        public static bool showIndex = false;
        public static bool showTiles = false;
        public static bool showMouse = false;
        public static bool showToolbar = false;

        public static int mousePointerIndex = 1;
        public static int backpict = 0;
        public static int spritesheet = 0;
        public static int firstsprite = 0;
        public static int currentanim = 1;//93 wheel; // 604 planches
        // 388 gradins
        // 376 flip test
        //100 snake

        public static Point aboutText = new Point(60, 70);
        public static bool spriteIndex = false;

        public static Point spritesheetViewport = new Point(0, 0);

        public struct Movement
        {
            public Keys Up;
            public Keys Down;
            public Keys Left;
            public Keys Right;

            public Movement(Keys u, Keys d, Keys l, Keys r)
            {
                Up = u;
                Down = d;
                Left = l;
                Right = r;
            }

        }

        public static Movement movement = new Movement(Keys.Z, Keys.S, Keys.Q, Keys.D);

    }




}
