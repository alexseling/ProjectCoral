using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectCoral
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ProjectCoralGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;
        public Camera Camera {get { return _camera; }}

        private SpriteFont scoreFont;
        private Butterfly _butterfly;
        private Frog _frog;
        private Batty _bat;
        private FootballField _field;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public ProjectCoralGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _frog = new Frog(this);
            _butterfly = new Butterfly(this);
            _bat = new Batty(this);
            _field = new FootballField(this);
            _camera = new Camera(graphics);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Camera.Initialize();
            // Temporary!
            Camera.Eye = new Vector3(0.0f, 5.0f, 25.0f);
            _previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _butterfly.LoadContent(Content);
            _frog.LoadContent(Content);
            _bat.LoadContent(Content);
            _field.LoadContent(Content);

            scoreFont = Content.Load<SpriteFont>("scorefont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _butterfly.Update(gameTime);
            _frog.Update(gameTime);
            _bat.Update(gameTime);
            _field.Update(gameTime);

            // Camera logic here
            if (_currentKeyboardState.IsKeyDown(Keys.Right))
            {
                Camera.Eye += new Vector3(1, 0, 0);
                Camera.Center += new Vector3(1, 0, 0);
                _butterfly.Horizontal -= 1;
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Left))
            {
                Camera.Eye += new Vector3(-1, 0, 0);
                Camera.Center += new Vector3(-1, 0, 0);
                _butterfly.Horizontal += 1;
            }

            _camera.Update(gameTime);

            _previousKeyboardState = _currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            _butterfly.Draw(graphics, gameTime);
            //_frog.Draw(graphics, gameTime);
            //_bat.Draw(graphics, gameTime);
            _field.Draw(graphics, gameTime);


            base.Draw(gameTime);
            string scoreString = String.Format("{0:f}", _butterfly.score);
            spriteBatch.Begin();
            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(10, 10), Color.White);
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
