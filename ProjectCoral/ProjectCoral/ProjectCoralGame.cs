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
        private FootballField _field;
        private LinkedList<Batty> bats = new LinkedList<Batty>();
        private LinkedList<Frog> frogs = new LinkedList<Frog>();

        private Random _random = new Random();
        private const float _maxZ = -100f;
        private const float _minZ = -450f;
        private const float _maxX = 120f;
        private const float _minX = -120f;
        private const int minNumCreatures = 25;
        private const int maxNumCreatures = 35;

        private const float horizontalMoveSpeed = 1f;


        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public enum GameScreens
        {
            Splash,
            Menu,
            Game,
            ScoreScreen
        }

        private GameScreens _currentGameScreen;
        public GameScreens CurrentGameScreen {get { return _currentGameScreen; }}

        private SplashScreen _splashScreen;
        private MenuGameScreen _menuGameScreen;
        private GameplayScreen _gameplayScreen;
        private ScoreScreen _scoreScreen;

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

            _butterfly = new Butterfly(this);
            _field = new FootballField(this, _butterfly);
            _camera = new Camera(graphics, _butterfly);

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
            Camera.Initialize();

            _previousKeyboardState = Keyboard.GetState();

            int numFrogs = _random.Next(minNumCreatures, maxNumCreatures);
            int numBats = _random.Next(minNumCreatures, maxNumCreatures);

            for (int i = 0; i < numBats; i++)
            {
                Batty b = new Batty(this);
                b.Position = new Vector3((float)(_minX + (_random.NextDouble() * (_maxX - _minX))), 0, _minZ + (float)((_random.NextDouble() * (_maxZ - _minZ))));
                bats.AddLast(b);
            }

            for (int i = 0; i < numFrogs; i++)
            {
                Frog f = new Frog(this);
                f.Position = new Vector3((float)(_minX + (_random.NextDouble() * (_maxX - _minX))), 0, _minZ + (float)((_random.NextDouble() * (_maxZ - _minZ))));
                f.JumpTimer = (float)_random.NextDouble() * 2;
                frogs.AddLast(f);
            }

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

            _butterfly.LoadContent(Content);
            foreach (Batty b in bats)
            {
                b.LoadContent(Content);
            }
            foreach (Frog f in frogs)
            {
                f.LoadContent(Content);
            }
            _field.LoadContent(Content);

            audioEngine = new AudioEngine("Content\\ProjectCoralAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");

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

            foreach (Batty b in bats)
            {
                b.Update(gameTime);

                if (b.TestForCollision(-1 * _butterfly.Position))
                {
                    SoundBank.PlayCue("explosion3");
                    _butterfly.SlowDown(false);
                    System.Diagnostics.Trace.WriteLine("BAT");
                }
            }
            foreach (Frog f in frogs)
            {
                f.Update(gameTime);

                if (f.TestForCollision(-1*_butterfly.Position))
                {
                    SoundBank.PlayCue("explosion3");
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

            _butterfly.Draw(graphics, gameTime);
            foreach (Batty b in bats)
            {
                b.Draw(graphics, gameTime);
            }
            foreach (Frog f in frogs)
            {
                f.Draw(graphics, gameTime);
            }
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
