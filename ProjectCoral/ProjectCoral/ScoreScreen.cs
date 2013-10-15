using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCoral
{
    public class ScoreScreen : GameScreen
    {
        private double _time = 0.0f;

        private Texture2D _splashScreenTexture;

        private KeyboardState _currentKeyboardState;

        private SpriteFont scoreFont;

        public ScoreScreen(ProjectCoralGame game) : base(game)
        {
            this._game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            _splashScreenTexture = Game.Content.Load<Texture2D>("SplashScreen_Awesome");

            scoreFont = Game.Content.Load<SpriteFont>("scorefont");
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

        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_splashScreenTexture, new Vector2(0, 0), Color.Transparent);

            string scoreString = String.Format("{0:f}", _game.Score);
            Vector2 stringLength = scoreFont.MeasureString(scoreString);

            int screenWidth = device.PresentationParameters.BackBufferWidth;
            int screenHeight = device.PresentationParameters.BackBufferHeight;

            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(10, 10), Color.White);
            
            base.DrawSprites(gameTime, spriteBatch);
        }
    }
}
