using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ProjectCoral
{
    public class Batty
    {

        private ProjectCoralGame game;
        private Model model;
        private ModelBone leftWingBase;
        private ModelBone rightWingBase;
        private Matrix lwingBaseBind;
        private Matrix rwingBaseBind;
        private float angle = 0;
        private float rate = 0.25f;
        private float offset;

        float radius = 5; //Whatever you want your radius to be
        /// <summary>
        /// Current position
        /// </summary>
        private Vector3 position = new Vector3(0, 0, 0);
        public Vector3 Position { get { return position; } set { position = value; } }
       
        //private Vector3 moveSpeed = new Vector3(1, 0, -1);

        //public Model Model { get { return model; } }
        private int head;
        private int LeftWing1;          // Index to the wing 1 bone
        private int LeftWing2;          // Index to the wing 2 bone
        private int RightWing1;          // Index to the wing 1 bone
        private int RightWing2;          // Index to the wing 2 bone
        private float wingAngle = -1; // Current wing deployment angle
        Boolean flap = false;
        
        public Batty(ProjectCoralGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// This function is called to update this component of our game
        /// to the current game time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (wingAngle >= -1f && flap == false)
            {
                wingAngle += (float)(Math.PI/2 * delta);
            }
            if (wingAngle <= 1f && flap == true)
            {
                wingAngle -= (float)(Math.PI/2 * delta);
            }
            if (wingAngle >= 1)
            {
                wingAngle -= (float)(Math.PI/2 * delta);
                wingAngle = 1;
                flap = true;
            }
            if (wingAngle <= -1)
            {
                wingAngle += (float)(Math.PI/2 * delta);
                wingAngle = -1;
                flap = false;
            }
            
          
            offset = (float)Math.Sin(angle) * radius;
            position.Y = offset + 10;
            angle += 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            leftWingBase.Transform = Matrix.CreateRotationY(wingAngle) * lwingBaseBind;
            rightWingBase.Transform = Matrix.CreateRotationY(-wingAngle) * rwingBaseBind;
        }

        /// <summary>
        /// This function is called to load content into this component
        /// of our game.
        /// </summary>
        /// <param name="content">The content manager to load from.</param>
        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("Bat-rigid");

            head = model.Bones.IndexOf(model.Bones["Head"]);

            leftWingBase = model.Bones["LeftWing1"];
            lwingBaseBind = leftWingBase.Transform;

            rightWingBase = model.Bones["RightWing1"];
            rwingBaseBind = rightWingBase.Transform;
        }

        /// <summary>
        /// This function is called to draw this game component.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            Matrix move = Matrix.CreateTranslation(position);

            DrawModel(model, move);
        }

        private void DrawModel(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            transforms[head] = Matrix.CreateRotationZ(wingAngle) * transforms[head];

            //transforms[LeftWing1] = Matrix.CreateRotationY(wingAngle1) * transforms[LeftWing1];
            //transforms[RightWing1] = Matrix.CreateRotationY(-wingAngle1) * transforms[RightWing1];

            transforms[LeftWing2] = Matrix.CreateRotationY(wingAngle) * transforms[LeftWing2];
            transforms[RightWing2] = Matrix.CreateRotationY(-wingAngle) * transforms[RightWing2];

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = game.Camera.View;
                    effect.Projection = game.Camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
