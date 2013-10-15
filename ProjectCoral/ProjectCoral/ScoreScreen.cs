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
            _splashScreenTexture = Game.Content.Load<Texture2D>("ScoreScreen_Awesome");

            scoreFont = Game.Content.Load<SpriteFont>("endfont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();

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
            spriteBatch.Draw(_splashScreenTexture, new Rectangle(0, 0, Game.Graphics.PreferredBackBufferWidth, Game.Graphics.PreferredBackBufferHeight), Color.White);

            string textString = "Your score";
            string scoreString =  String.Format("{0:f}", _game.Score);
            Vector2 scoreSize = scoreFont.MeasureString(scoreString);
            Vector2 textSize = scoreFont.MeasureString(textString);

            int height = Game.Graphics.PreferredBackBufferHeight;
            int width = Game.Graphics.PreferredBackBufferWidth;


            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(width / 2 - scoreSize.X / 2, height / 2), Color.White);
            spriteBatch.DrawString(scoreFont, textString, new Vector2(width / 2 - textSize.X / 2, height / 2 - textSize.Y), Color.White);

            base.DrawSprites(gameTime, spriteBatch);
        }
    }
}
