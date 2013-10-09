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

        public float score = 1;

        private float distance = 0;
        private float horizontal = 0;
        public float Horizontal { get { return horizontal; } set { horizontal = value; } }

        /*private Vector3 _position = Vector3.Zero;
        public Vector3 Position {get { return _position; } set { _position = value; }}

        private float _speed = 0.0f;
        public float Speed {get { return _speed; }}*/

        private float _maxSpeed = 40f;
        public float MaxSpeed {get { return _maxSpeed; }}

        private Quaternion _orientation = Quaternion.Identity;

        /*public Matrix Transform
        {
            get
            {
                return Matrix.CreateFromQuaternion(_orientation) 
                    * Matrix.CreateTranslation(_position);
            }
        }*/

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

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            score += delta;

            _wingAngle = (float)Math.Sin(8 * gameTime.TotalGameTime.TotalSeconds) / 2.0f;

            distance = MaxSpeed * (float)gameTime.TotalGameTime.TotalSeconds;
        }

        /// <summary>
        /// This function is called to draw this game component.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            Matrix transform = Matrix.CreateTranslation(new Vector3(horizontal, 0, distance)) * Matrix.CreateRotationY((float)Math.PI);
            //Matrix transform = Matrix.CreateFromQuaternion(_orientation)
                                // * Matrix.CreateTranslation(_position);

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
