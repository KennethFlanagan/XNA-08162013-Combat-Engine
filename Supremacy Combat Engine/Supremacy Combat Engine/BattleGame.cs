using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Supremacy_Combat_Engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BattleGame : Microsoft.Xna.Framework.Game
    {
         GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ArcBallCamera camera;
       // Terrain terrain;
        Effect effect;
     //   BasicEffect effect; // mars runner skybox addon
      //  Skybox skybox;  // mars runner addon

        Point screenCenter;
        Point saveMousePoint;
        bool moveMode = false;
        float scrollRate = 1.0f;
        MouseState previousMouse;
        List<ShipsAndStations> shipsAndStations = new List<ShipsAndStations>();

        public BattleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
// below camer looks at 64,16,64 point with elivation -30 degrees and rotation 0, min view 32 and max 192 starting at 128
            camera = new ArcBallCamera(
                new Vector3(64f, 16f, 64f),
                MathHelper.ToRadians(-30),
                0f,
                32f,
                192f,
                128f,
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                512f);

            screenCenter.X = this.Window.ClientBounds.Width / 2;
            screenCenter.Y = this.Window.ClientBounds.Height / 2;

            this.IsMouseVisible = true;

            previousMouse = Mouse.GetState();
            Mouse.SetPosition(screenCenter.X, screenCenter.Y);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
           // if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content"); // mars runner


            /*terrain = new Terrain(
                GraphicsDevice,
                Content.Load<Texture2D>(@"Textures\HeightMap_02"),
                Content.Load<Texture2D>(@"Textures\Grass"),
                32f,
                128,
                128,
                30f);
// terrain is 128 by 128, textureScale is 32, scaling factor of 30 so top peak is 30.
            effect = Content.Load<Effect>(@"Effects/Terrain");

             TODO: use this.Content to load your game content here
                  */
            shipsAndStations.Add(new ShipsAndStations(GraphicsDevice, Content.Load<Model>(@"Models\kling_scout_iii"), new Vector3(61,40,61)));


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (this.IsActive)// if the game window is active go on
            {

                MouseState mouse = Mouse.GetState();

                if (moveMode)
                {// adds half the x movement to rotation and half the y to elevation of camera
                    camera.Rotation += MathHelper.ToRadians(
                        (mouse.X - screenCenter.X) / 2f);
                    camera.Elevation += MathHelper.ToRadians(
                        (mouse.Y - screenCenter.Y) / 2f);

                    Mouse.SetPosition(screenCenter.X, screenCenter.Y); // return mouse to center of screen
                }

                if (mouse.RightButton == ButtonState.Pressed)
                {
                    if (!moveMode &&
                        previousMouse.RightButton == ButtonState.Released)
                    {
                        if (graphics.GraphicsDevice.Viewport.Bounds.Contains(
                            new Point(mouse.X, mouse.Y)))
                        {
                            moveMode = true;
                            saveMousePoint.X = mouse.X;
                            saveMousePoint.Y = mouse.Y;
                            Mouse.SetPosition(screenCenter.X, screenCenter.Y);
                            this.IsMouseVisible = false;
                        }
                    }
                }
                else
                {
                    if (moveMode)
                    {
                        moveMode = false;
                        Mouse.SetPosition(saveMousePoint.X, saveMousePoint.Y);
                        this.IsMouseVisible = true;
                    }
                }

                if (mouse.ScrollWheelValue - previousMouse.ScrollWheelValue != 0)
                {
                    float wheelChange = mouse.ScrollWheelValue -
                        previousMouse.ScrollWheelValue;

                    camera.ViewDistance -= (wheelChange / 120) * scrollRate;
                }

                previousMouse = mouse;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // terrain.Draw(camera, effect);

            // TODO: Add your drawing code here
            foreach (ShipsAndStations shipAndStation in shipsAndStations)
            {
                shipAndStation.Draw(camera);

            }

            base.Draw(gameTime);
        }
    }
}
