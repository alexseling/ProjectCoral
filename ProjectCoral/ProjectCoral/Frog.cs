using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectCoral
{
    public class Frog
    {
        private ProjectCoralGame game;
        private Model model;
        private int frontLeftLeg;
        private int frontRightLeg;
        private int backLeftLeg;
        private int backRightLeg;
        private int head;

        private float legAngle = 0;
        private float legAngleRate = 2f;
        private const float maxLegAngle = .7f;
        private const float minLegAngle = 0;

        private float headAngle = 0;
        private float headAngleRate = .5f;
        private float maxHeadAngle = .1f;
        private float minHeadAngle = 0;

        private const float jumpWaitTime = .6f;
        private float jumpTimer = jumpWaitTime;
        public float JumpTimer { get { return jumpTimer; } set { jumpTimer = value; } }
        bool waiting = true;

        private const float acceleration = 25f;
        private const float upVelocity = 15;
        private float time = 0;
        private float oldPosition = 0;

        private Vector3 position;
        public Vector3 Position { get { return position; } set { position = value; } }

        public Frog(ProjectCoralGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// This function is called to load content into this component
        /// of our game.
        /// </summary>
        /// <param name="content">The content manager to load from.</param>
        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("Frog-rigid");
            frontLeftLeg = model.Bones.IndexOf(model.Bones["LeftFrontLeg"]);
            frontRightLeg = model.Bones.IndexOf(model.Bones["RightFrontLeg"]);
            backLeftLeg = model.Bones.IndexOf(model.Bones["LeftBackLeg"]);
            backRightLeg = model.Bones.IndexOf(model.Bones["RightBackLeg"]);
            head = model.Bones.IndexOf(model.Bones["Head"]);
        }

        public bool TestForCollision(Vector3 testPosition)
        {
            BoundingSphere bs = model.Meshes[0].BoundingSphere;
            bs = bs.Transform(model.Bones[0].Transform);

            bs.Radius *= 1.5f;
            bs.Center += position;

            if ((testPosition - bs.Center).LengthSquared() < bs.Radius * bs.Radius)
            {   
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function is called to update this component of our game
        /// to the current game time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Wait for jumpTimer after jumping
            if (waiting)
            {
                jumpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (jumpTimer <= 0)
                {
                    waiting = false;
                    jumpTimer = jumpWaitTime;
                }
            }

            if (!waiting)
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Set the legs and head
                if (position.Y >= oldPosition)
                {
                    if (legAngle <= maxLegAngle)
                    {
                        legAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * legAngleRate;
                    }
                    if (headAngle <= maxHeadAngle)
                    {
                        headAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * headAngleRate;
                    }
                }
                else
                {
                    if (legAngle >= minLegAngle)
                    {
                        legAngle -= (float)gameTime.ElapsedGameTime.TotalSeconds * legAngleRate;
                    }
                    if (headAngle >= minHeadAngle)
                    {
                        headAngle -= (float)gameTime.ElapsedGameTime.TotalSeconds * headAngleRate;
                    }
                }

                // Set the vertical position
                oldPosition = position.Y;
                position.Y = upVelocity * time + .5f * -acceleration * time * time;
                position.Y = position.Y < 0 ? 0 : position.Y;
            }

            if (position.Y == 0)
            {
                time = 0;
                waiting = true;
            }

        }

        /// <summary>
        /// This function is called to draw this game component.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            Matrix transform = Matrix.CreateTranslation(position);
            DrawModel(graphics, model, transform);
        }

        private void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            transforms[frontLeftLeg] = Matrix.CreateRotationX(legAngle) * transforms[frontLeftLeg];
            transforms[frontRightLeg] = Matrix.CreateRotationX(legAngle) * transforms[frontRightLeg];
            transforms[backLeftLeg] = Matrix.CreateRotationX(legAngle) * transforms[backLeftLeg];
            transforms[backRightLeg] = Matrix.CreateRotationX(legAngle) * transforms[backRightLeg];
            transforms[head] = Matrix.CreateRotationX(-headAngle) * transforms[head];

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
