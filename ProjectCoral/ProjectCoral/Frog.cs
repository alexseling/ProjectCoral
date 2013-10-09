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
        private const float maxLegAngle = .6f;
        private const float minLegAngle = 0;

        private float headAngle = 0;
        private float headAngleRate = .5f;
        private float maxHeadAngle = .1f;
        private float minHeadAngle = 0;

        private bool increasingAngle = true;

        private const float jumpWaitTime = 1f;
        private float jumpTimer = jumpWaitTime;
        bool waiting = false;

        private float elevation = 0;
        private float elevationRate = 5f;
        private float maxElevation = 2f;
        private float minElevation = 0f;

        private float movement = 0;
        private float movementRate = 3f;




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

        /// <summary>
        /// This function is called to update this component of our game
        /// to the current game time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
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
                if (increasingAngle)
                {
                    if (legAngle <= maxLegAngle)
                    {
                        legAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * legAngleRate;
                    }
                    if (elevation <= maxElevation)
                    {
                        elevation += (float)gameTime.ElapsedGameTime.TotalSeconds * elevationRate;
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
                    if (elevation >= minElevation)
                    {
                        elevation -= (float)gameTime.ElapsedGameTime.TotalSeconds * elevationRate;
                    }
                    if (headAngle >= minHeadAngle)
                    {
                        headAngle -= (float)gameTime.ElapsedGameTime.TotalSeconds * headAngleRate;
                    }
                }
                movement += movementRate * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }

            if (legAngle > maxLegAngle && elevation > maxElevation)
            {
                increasingAngle = false;
            }
            else if (legAngle < minLegAngle && elevation < minElevation)
            {
                increasingAngle = true;
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
            Matrix transform = Matrix.CreateTranslation(new Vector3(0, elevation, 0));
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
