using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCoral
{
    public class Butterfly
    {
        private ProjectCoralGame _game;

        private Model _model;
        public Model Model {get { return _model; }}

        // These ints hold the index for each butterfly wing
        private int _leftWingIndex;
        private int _rightWingIndex;
        private float _wingAngle = 0.0f;

        public float score = 0;

        private const float _maxSpeed = 50f;
        private float _speed = _maxSpeed;
        public float Speed {get { return _speed; }}
        private const float _maxDistance = 500f;
        private bool _moving = true;
        public bool Moving { get { return _moving; } set { _moving = value; } }

        private const float _maxSlowDownTime = 2f;
        private float _slowDownTime = 0;
        private bool _isSlow = false;

        private Quaternion _orientation = Quaternion.Identity;
        private Vector3 _position = new Vector3(0, 0, 0);
        public Vector3 Position { get { return _position; } set { _position = value; } }

        public Butterfly(ProjectCoralGame game)
        {
            _game = game;
        }

        /// <summary>
        /// This function is called to load content into this component
        /// of our game.
        /// </summary>
        /// <param name="content">The content manager to load from.</param>
        public void LoadContent(ContentManager content)
        {
            _model = content.Load<Model>("Butterfly");
             _leftWingIndex = _model.Bones.IndexOf(_model.Bones["WingLeft"]);
             _rightWingIndex = _model.Bones.IndexOf(_model.Bones["WingRight"]);
        }

        public void SlowDown(bool isFrog)
        {
            if (!_isSlow)
            {
                _speed = isFrog ? _speed / 4 : _speed / 3;
                _isSlow = true;
                _slowDownTime = _maxSlowDownTime;
            }
        }

        public void Update(GameTime gameTime)
        {
           
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_isSlow)
            {
                _slowDownTime -= delta;
                if (_slowDownTime <= 0)
                {
                    _slowDownTime = 0;
                    _isSlow = false;
                    _speed = _maxSpeed;
                }
            }

            _wingAngle = (float)Math.Sin(8 * gameTime.TotalGameTime.TotalSeconds) / 2.0f;

            if (_moving)
            {
                score += delta;
                _position.Z +=  _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_position.Z >= _maxDistance)
                    _moving = false;
            }

            System.Diagnostics.Trace.WriteLine("Butterfly: " + _position.Z);
        }

        /// <summary>
        /// This function is called to draw this game component.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            Matrix transform = Matrix.CreateTranslation(_position) * Matrix.CreateRotationY((float)Math.PI);

            DrawModel(graphics, _model, transform);
        }

        private void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            transforms[_leftWingIndex] = Matrix.CreateRotationY(_wingAngle) * transforms[_leftWingIndex];
            transforms[_rightWingIndex] = Matrix.CreateRotationY(-_wingAngle) * transforms[_rightWingIndex];

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = _game.Camera.View;
                    effect.Projection = _game.Camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
