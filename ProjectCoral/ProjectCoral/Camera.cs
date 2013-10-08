using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectCoral
{
    public class Camera
    {
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The eye position in space
        /// </summary>
        private Vector3 _eye = new Vector3(0, 0, 1000);
        public Vector3 Eye {get { return _eye; } set { _eye = value; ComputeView(); } }

        /// <summary>
        /// The location we are looking at in space.
        /// </summary>
        private Vector3 _center = new Vector3(0, 0, 0);
        public Vector3 Center { get { return _center; } set { _center = value; ComputeView(); } }

        /// <summary>
        /// The up direction
        /// </summary>
        private Vector3 _up = new Vector3(0, 1, 0);
        public Vector3 Up {get { return _up; } set { _up = value; ComputeView(); } }

        private float _fov = MathHelper.ToRadians(35);
        private float _znear = 10;
        private float _zfar = 20000;

        private Matrix _view;
        private Matrix _projection;
        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }

        private float _stiffness = 100;
        private float _damping = 60;

        public float Stiffness { get { return _stiffness; } set { _stiffness = value; } }
        public float Damping { get { return _damping; } set { _damping = value; } }

        public Camera(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void Initialize()
        {
            ComputeView();
            ComputeProjection();


        }

        public void Update(GameTime gameTime)
        {
            
        }

        private void ComputeView()
        {
            _view = Matrix.CreateLookAt(_eye, _center, _up);
        }

        private void ComputeProjection()
        {
            _projection = Matrix.CreatePerspectiveFieldOfView(_fov, graphics.GraphicsDevice.Viewport.AspectRatio, _znear, _zfar);
        }
    }
}
