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
        private GraphicsDeviceManager graphics;
        public GraphicsDeviceManager Graphics {get { return graphics; }}
        
        SpriteBatch spriteBatch;

        public enum GameScreens
        {
            Splash,
            Menu,
            Game,
            ScoreScreen
        }

        private GameScreens _currentScreenState;
        public GameScreens CurrentScreenState {get { return _currentScreenState; }}

        private SplashScreen _splashScreen;
        private MenuGameScreen _menuGameScreen;
        private GameplayScreen _gameplayScreen;
        private ScoreScreen _scoreScreen;

        private GameScreen _currentGameScreen;

        /// <summary>
        /// A reference to the audio engine we use
        /// </summary>
        AudioEngine audioEngine;

        /// <summary>
        /// The loaded audio wave bank
        /// </summary>
        WaveBank waveBank;

        /// <summary>
        /// The loaded audio sound bank
        /// </summary>
        SoundBank soundBank;
        public SoundBank SoundBank { get { return soundBank; } }

        public ProjectCoralGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _splashScreen = new SplashScreen(this);
            _menuGameScreen = new MenuGameScreen(this);
            _gameplayScreen = new GameplayScreen(this);
            _scoreScreen = new ScoreScreen(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _splashScreen.Initialize();
            _menuGameScreen.Initialize();
            _gameplayScreen.Initialize();
            _scoreScreen.Initialize();

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
            
            audioEngine = new AudioEngine("Content\\ProjectCoralAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");
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
<<<<<<< HEAD
            _currentGameScreen.Update(gameTime);
=======
            _currentKeyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _butterfly.Update(gameTime);

            foreach (Batty b in bats)
            {
                b.Update(gameTime);

                if (b.TestForCollision(-1 * _butterfly.Position))
                {
                    SoundBank.PlayCue("batchirp");
                    _butterfly.SlowDown(false);
                    System.Diagnostics.Trace.WriteLine("BAT");
                }
            }
            foreach (Frog f in frogs)
            {
                f.Update(gameTime);

                if (f.TestForCollision(-1*_butterfly.Position))
                {
                    SoundBank.PlayCue("frog");
                    _butterfly.SlowDown(true);
                    System.Diagnostics.Trace.WriteLine("FROG5");
                }
            }
            _field.Update(gameTime);

            // Camera logic here
            if (_currentKeyboardState.IsKeyDown(Keys.Right) && _butterfly.Position.X >= _minX)
            {
                Camera.Eye += new Vector3(horizontalMoveSpeed, 0, 0);
                Camera.Center += new Vector3(horizontalMoveSpeed, 0, 0);
                _butterfly.Position -= new Vector3(horizontalMoveSpeed, 0, 0);
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Left) && _butterfly.Position.X <= _maxX)
            {
                Camera.Eye -= new Vector3(horizontalMoveSpeed, 0, 0);
                Camera.Center -= new Vector3(horizontalMoveSpeed, 0, 0);
                _butterfly.Position += new Vector3(horizontalMoveSpeed, 0, 0);
            }

            _camera.Update(gameTime);

            _previousKeyboardState = _currentKeyboardState;
>>>>>>> c6d932d489f57f60af055cf51fdbd6acd9b9a585

            // Update audioEngine.
            audioEngine.Update();

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            _currentGameScreen.Draw(gameTime);
            
            spriteBatch.Begin();
            _currentGameScreen.DrawSprites(gameTime, spriteBatch);
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }

        public void SetScreen(GameScreens screen)
        {
            _currentGameScreen.Deactivate();

            switch (screen)
            {
                case GameScreens.Splash:
                    _currentGameScreen = _splashScreen;
                    break;
                case GameScreens.Menu:
                    _currentGameScreen = _menuGameScreen;
                    break;
                case GameScreens.Game:
                    _currentGameScreen = _gameplayScreen;
                    break;
                case GameScreens.ScoreScreen:
                    _currentGameScreen = _scoreScreen;
                    break;
            }
            
            _currentGameScreen.Activate();
        }

    }
}
