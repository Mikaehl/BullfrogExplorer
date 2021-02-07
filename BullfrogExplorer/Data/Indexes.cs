using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace BullfrogExplorer.Data
{
    class Indexes
    {
        public struct Credits
        {
            public int index;
            public Vector2 position;
            public int currentIndex;
            public int offset;
            public TimeSpan delay;
            public string background;

            public Credits(int i, Vector2 p)
            {
                this.index = i;
                this.position = p;
                this.currentIndex = i;
                this.offset = 0;
                this.delay = new TimeSpan(0, 0, 5);
                this.background = "MMENU-1";
            }
        }

        public static Credits credits = new Credits(822, new Vector2(65, 85));

    }
}
