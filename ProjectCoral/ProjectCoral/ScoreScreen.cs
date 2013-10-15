﻿using System;
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
            
            base.DrawSprites(gameTime, spriteBatch);
        }
    }
}
