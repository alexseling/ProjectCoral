using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCoral
{
    public class SplashScreen : GameScreen
    {
        public SplashScreen(ProjectCoralGame game) : base(game)
        {
            this._game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void DrawModel(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawModel(gameTime, spriteBatch);
        }
    }
}
