using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BullfrogExplorer.Engines
{

    // A mega texture is just a big big texture made with a bunch of textures or sprites.
    // The aim is to use SpriteBatch to construct this texture with some other textures/sprite inside.
    // Not used, currently, not needed at all.

    class MegaTexture
    {

        public int Width;
        public int Height;

        public RenderTarget2D Buffer;

        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;

        public Texture2D dotTexture;

        public MegaTexture(GraphicsDevice gd)
        {
            this.Width = 1000;
            this.Height = 1000;
            this.spriteBatch = new SpriteBatch(gd);
            this.graphicsDevice = gd;
        }

        public MegaTexture(GraphicsDevice gd, int w, int h)
        {
            this.Width = w;
            this.Height = h;
            this.spriteBatch = new SpriteBatch(gd);
            this.graphicsDevice = gd;
        }
        /*
        public MegaTexture(GraphicsDevice gd, SpriteBatch sb,int w, int h)
        {
            this.graphicsDevice = gd;
            this.spriteBatch = sb;
            this.Width = w;
            this.Height = h;
        }

        public MegaTexture(GraphicsDevice gd, SpriteBatch sb)
        {
            this.graphicsDevice = gd;
            this.spriteBatch = sb;

        }
        */
        public void LoadContent()
        {
            this.Buffer = new RenderTarget2D(this.graphicsDevice, this.Width, this.Height,
                false, SurfaceFormat.Color, DepthFormat.None);

            this.dotTexture = TextureDotCreate(this.graphicsDevice);

        }

        public static Texture2D TextureDotCreate(GraphicsDevice device)
        {
            Color[] data = new Color[1];
            data[0] = new Color(255, 255, 255, 255);
            return TextureFromColorArray(device, data, 1, 1);
        }
        public static Texture2D TextureFromColorArray(GraphicsDevice device, Color[] data, int width, int height)
        {
            Texture2D tex = new Texture2D(device, width, height);
            tex.SetData<Color>(data);
            return tex;
        }

        public void Update()
        {

        }

        public void Draw()
        {
            this.spriteBatch.Begin();
            this.graphicsDevice.SetRenderTarget(this.Buffer);
            this.spriteBatch.Draw(this.dotTexture, new Rectangle(10, 10, 20, 20), Color.GreenYellow);
            this.spriteBatch.End();
            this.graphicsDevice.SetRenderTarget(null);

        }

    }
}
