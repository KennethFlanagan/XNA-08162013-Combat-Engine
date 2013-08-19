using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Supremacy_Combat_Engine
{
    class ArcBallCamera
     {
        #region Fields
        private Vector3 cameraPosition = Vector3.Zero;
        private Vector3 targetPosition = Vector3.Zero;

        private float elevation;
        private float rotation;

        private float minDistance; // will move camera in and out 
        private float maxDistance;
        private float viewDistance = 12f; // how far back the camera can be, radius of sphere

        private Vector3 baseCameraReference = new Vector3(0, 0, 1); // camera is at 0,0,1 looking at 0,0,0 
        private bool needViewResync = true;

        private Matrix cachedViewMatrix;
        #endregion

        #region Properties
        public Matrix Projection { get; private set; }
        public Matrix WideProjection { get; private set; } // added from Mars Runner
        public Vector3 Target
        {
            get { return targetPosition; }
            set
            {
                targetPosition = value;
                needViewResync = true;
            }
        }

        public Vector3 Position
        {
            get // no set, is read only set by rotation and elevation
            {
                return cameraPosition;
            }
        }

        public float Elevation
        {
            get { return elevation; }
            set
            {
                elevation = MathHelper.Clamp(
                    value,
                    MathHelper.ToRadians(-70), 
                    // looks to be degrees camera looking down angle from arcball
                    MathHelper.ToRadians(-10)); // change here to let 
                // camer get below horazontal and look up at ships
                needViewResync = true;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = MathHelper.WrapAngle(value);// keeps the value of angle within one full circle of rotation
                needViewResync = true;
            }
        }

        public float ViewDistance
        {
            get { return viewDistance; }
            set
            {
                viewDistance = MathHelper.Clamp(
                    value,
                    minDistance,
                    maxDistance);
            }
        }

        public Matrix View // calculate and return View Matrix and camera position from elivation roatation
        {
            get
            {
                if (needViewResync) // call ArcBallCamera method below sets needViewResync to true
                {
                    Matrix transformMatrix = Matrix.CreateFromYawPitchRoll(
                        rotation,
                        elevation,
                        0f);// roation = yaw, elevation = pitch, roll = 0 in matrix for view
                    cameraPosition = Vector3.Transform(
                        baseCameraReference,
                        transformMatrix);// baseCameraReference is at 0,0,1 looking back at 0,0,0
                    // transformMatrix relocates camer from baseCamerReference to cameraPosition
                    cameraPosition *= viewDistance; // viewDistance * camerPosition a 1 z gets new cameraPosition out on sphere
                    cameraPosition += targetPosition; // moves camer from looking at origin to look at targetPosition

                    cachedViewMatrix = Matrix.CreateLookAt(
                        cameraPosition,
                        targetPosition,
                        Vector3.Up);// matrix stores view
                }

                return cachedViewMatrix;
            }
        }
        #endregion

        #region Constructor ArcBallCamera passes parameters given to it along to the various class fields
        // also sets up Projection matrix and flags needViewResync to true.
        public ArcBallCamera(
            Vector3 targetPosition,
            float initialElevation,
            float initialRotation,
            float minDistance,
            float maxDistance,
            float initialDistance,
            float aspectRatio,
            float nearClip,
            float farClip)
        {
            Target = targetPosition;
            Elevation = initialElevation;
            Rotation = initialRotation;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            ViewDistance = initialDistance;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                aspectRatio,
                nearClip,
                farClip);

            needViewResync = true;
        }
        #endregion

    }
}

   
