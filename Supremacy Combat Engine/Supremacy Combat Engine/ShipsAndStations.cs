using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Supremacy_Combat_Engine
{
    class ShipsAndStations
    {
        #region Fields
        private Model model;
        private GraphicsDevice device;

        private Vector3 position;
        private float shipRotation;
        /*  private float turretRotation;
          private float gunElevation;

          private Matrix baseTurretTransform;
          private Matrix baseGunTransform;
          private Matrix[] boneTransforms;
        */
        #endregion
        #region Properties
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public float ShipRotation
        {
            get
            {
                return shipRotation;
            }
            set
            {
                shipRotation = MathHelper.WrapAngle(value);
            }
        }
        /*
        public float TurretRotation
        {
            get
            {
                return turretRotation;
            }
            set
            {
                turretRotation = MathHelper.WrapAngle(value);
            }
        }

        public float GunElevation
        {
            get
            {
                return gunElevation;
            }
            set
            {
                gunElevation = MathHelper.Clamp(
                    value,
                    MathHelper.ToRadians(-90),
                    MathHelper.ToRadians(0));
            }
        } */
        #endregion
        #region Constructor
        public ShipsAndStations(GraphicsDevice device, Model model, Vector3 position)
        {
            this.device = device;
            this.model = model;
            Position = position;
           // boneTransforms = new Matrix[model.Bones.Count];

           // baseTurretTransform = model.Bones["turret_geo"].Transform;
           // baseGunTransform = model.Bones["canon_geo"].Transform;
        }
        #endregion
        #region Draw
        public void Draw(ArcBallCamera camera)
        {
            model.Root.Transform = Matrix.Identity *
                Matrix.CreateScale(0.005f) *
                Matrix.CreateRotationY(ShipRotation) *
                Matrix.CreateTranslation(Position);

           // model.Bones["turret_geo"].Transform =
           // Matrix.CreateRotationY(TurretRotation) * baseTurretTransform;

           // model.Bones["canon_geo"].Transform =
             //   Matrix.CreateRotationX(gunElevation) * baseGunTransform;

           // model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                   // basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;

                    basicEffect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
        #endregion
    }
}
