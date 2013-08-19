using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supremacy_Combat_Engine
{
    class Skybox
    {
      #region Fields
        private GraphicsDevice device;
        private Texture2D texture;
        private VertexBuffer cubeVertexBuffer;
        private List<VertexPositionTexture> vertices = new
            List<VertexPositionTexture>();
        private float rotation = 0f;
        #endregion
        #region Constructor
        public Skybox(
            GraphicsDevice graphicsDevice,
            Texture2D texture)
        {
            device = graphicsDevice;
            this.texture = texture;

            // Create the cube’s vertical faces
            BuildFace(
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 1),
                new Vector2(0, 0.25f)); // west face
            BuildFace(
                new Vector3(0, 0, 1),
                new Vector3(1, 1, 1),
                new Vector2(0.75f, 0.25f)); // south face
            BuildFace(
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 0),
                new Vector2(0.5f, 0.25f)); // east face
            BuildFace(
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector2(0.25f, 0.25f)); // North face

            // Create the cube’s horizontal faces
            BuildFaceHorizontal(
                new Vector3(1, 1, 0),
                new Vector3(0, 1, 1),
                new Vector2(0.25f, 0)); // Top face
            BuildFaceHorizontal(
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 0),
                new Vector2(0.25f, 0.5f)); // Bottom face

            cubeVertexBuffer = new VertexBuffer(
                device,
                VertexPositionTexture.VertexDeclaration,
                vertices.Count,
                BufferUsage.WriteOnly);

            cubeVertexBuffer.SetData<VertexPositionTexture>(
                vertices.ToArray());
        }
        #endregion
        #region Helper Methods
        private void BuildFace(Vector3 p1, Vector3 p2, Vector2 txCoord)
        {
            vertices.Add(BuildVertex(
                p1.X, p1.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y + 0.25f));
            vertices.Add(BuildVertex(
                p2.X, p2.Y, p2.Z, txCoord.X, txCoord.Y));
            vertices.Add(BuildVertex(
                p1.X, p2.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y));

            vertices.Add(BuildVertex(
                p1.X, p1.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y + 0.25f));
            vertices.Add(BuildVertex(
                p2.X, p1.Y, p2.Z, txCoord.X, txCoord.Y + 0.25f));
            vertices.Add(BuildVertex(
                p2.X, p2.Y, p2.Z, txCoord.X, txCoord.Y));
        }

        private void BuildFaceHorizontal(
            Vector3 p1, Vector3 p2, Vector2 txCoord)
        {
            vertices.Add(BuildVertex(
                p1.X, p1.Y, p1.Z, txCoord.X, txCoord.Y + 0.25f));
            vertices.Add(BuildVertex(
                p2.X, p2.Y, p2.Z, txCoord.X + 0.25f, txCoord.Y));
            vertices.Add(BuildVertex(
                p2.X, p1.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y + 0.25f));

            vertices.Add(BuildVertex(
                p1.X, p1.Y, p1.Z, txCoord.X, txCoord.Y + 0.25f));
            vertices.Add(BuildVertex(
                p1.X, p1.Y, p2.Z, txCoord.X, txCoord.Y));
            vertices.Add(BuildVertex(
                p2.X, p2.Y, p2.Z, txCoord.X + 0.25f, txCoord.Y));
        }

        private VertexPositionTexture BuildVertex(
            float x,
            float y,
            float z,
            float u,
            float v)
        {
            return new VertexPositionTexture(
            new Vector3(x, y, z),
            new Vector2(u, v));
        }
        #endregion
        #region Draw
        public void Draw(ArcBallCamera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;
            effect.LightingEnabled = false;

            Matrix center = Matrix.CreateTranslation(
                new Vector3(-0.5f, -0.5f, -0.5f));
            Matrix scale = Matrix.CreateScale(200f);

            Matrix translate = Matrix.CreateTranslation(camera.Position);

            Matrix rot = Matrix.CreateRotationY(rotation);

            effect.World = center * rot * scale * translate;
            effect.View = camera.View;
            effect.Projection = camera.WideProjection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(cubeVertexBuffer);
                device.DrawPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    cubeVertexBuffer.VertexCount / 3);
            }
        }
        #endregion
        
    }
}
