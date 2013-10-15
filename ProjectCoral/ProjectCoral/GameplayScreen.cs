using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCoral
{
    public class GameplayScreen : GameScreen
    {
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

        public GameplayScreen(ProjectCoralGame game) : base(game)
        {
            this._game = game;
            
            _butterfly = new Butterfly(Game);
            _field = new FootballField(Game, _butterfly);
            Game.SetCamera(new Camera(Game.Graphics, _butterfly));

        }

        public override void Initialize()
        {
            Game.Camera.Initialize();

            _previousKeyboardState = Keyboard.GetState();

            int numFrogs = _random.Next(minNumCreatures, maxNumCreatures);
            int numBats = _random.Next(minNumCreatures, maxNumCreatures);

            for (int i = 0; i < numBats; i++)
            {
                Batty b = new Batty(Game);
                b.Position = new Vector3((float)(_minX + (_random.NextDouble() * (_maxX - _minX))), 0, _minZ + (float)((_random.NextDouble() * (_maxZ - _minZ))));
                bats.AddLast(b);
            }

            for (int i = 0; i < numFrogs; i++)
            {
                Frog f = new Frog(Game);
                f.Position = new Vector3((float)(_minX + (_random.NextDouble() * (_maxX - _minX))), 0, _minZ + (float)((_random.NextDouble() * (_maxZ - _minZ))));
                f.JumpTimer = (float)_random.NextDouble() * 2;
                frogs.AddLast(f);
            }
        }

        public override void LoadContent()
        {
            _butterfly.LoadContent(Game.Content);
            foreach (Batty b in bats)
            {
                b.LoadContent(Game.Content);
            }
            foreach (Frog f in frogs)
            {
                f.LoadContent(Game.Content);
            }
            _field.LoadContent(Game.Content);

            scoreFont = Game.Content.Load<SpriteFont>("scorefont");
        }

        public override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                //this.Exit();

            _butterfly.Update(gameTime);

            foreach (Batty b in bats)
            {
                b.Update(gameTime);

                if (b.TestForCollision(-1 * _butterfly.Position))
                {
                    Game.SoundBank.PlayCue("explosion3");
                    _butterfly.SlowDown(false);
                    System.Diagnostics.Trace.WriteLine("BAT");
                }
            }
            foreach (Frog f in frogs)
            {
                f.Update(gameTime);

                if (f.TestForCollision(-1 * _butterfly.Position))
                {
                    Game.SoundBank.PlayCue("explosion3");
                    _butterfly.SlowDown(true);
                    System.Diagnostics.Trace.WriteLine("FROG5");
                }
            }
            _field.Update(gameTime);

            // Camera logic here
            if (_currentKeyboardState.IsKeyDown(Keys.Right) && _butterfly.Position.X >= _minX)
            {
                Game.Camera.Eye += new Vector3(horizontalMoveSpeed, 0, 0);
                Game.Camera.Center += new Vector3(horizontalMoveSpeed, 0, 0);
                _butterfly.Position -= new Vector3(horizontalMoveSpeed, 0, 0);
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.Left) && _butterfly.Position.X <= _maxX)
            {
                Game.Camera.Eye -= new Vector3(horizontalMoveSpeed, 0, 0);
                Game.Camera.Center -= new Vector3(horizontalMoveSpeed, 0, 0);
                _butterfly.Position += new Vector3(horizontalMoveSpeed, 0, 0);
            }

            if (_butterfly.Position.Z > 500.0f)
            {
                Game.SetScreen(ProjectCoralGame.GameScreens.ScoreScreen);
            }

            Game.Camera.Update(gameTime);

            _previousKeyboardState = _currentKeyboardState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _butterfly.Draw(Game.Graphics, gameTime);
            foreach (Batty b in bats)
            {
                b.Draw(Game.Graphics, gameTime);
            }
            foreach (Frog f in frogs)
            {
                f.Draw(Game.Graphics, gameTime);
            }
            _field.Draw(Game.Graphics, gameTime);
        }
        
        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string scoreString = String.Format("{0:f}", _butterfly.score);
            spriteBatch.DrawString(scoreFont, scoreString, new Vector2(10, 10), Color.White);
        }

        public override void Activate()
        {
            _previousKeyboardState = Keyboard.GetState();

            base.Activate();
        }

        public override void Deactivate()
        {
            
        }

        public void Reset()
        {

        }
    }
}
