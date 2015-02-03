using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace HelloWorld {
    class HelloGame : Game {
        GraphicsDeviceManager graphics;

        public HelloGame() {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;

            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 320;
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);

            graphics.GraphicsDevice.Clear(Color.BlanchedAlmond);
        }
    }
}
