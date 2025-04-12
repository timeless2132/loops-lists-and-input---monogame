using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace loops_lists_and_input___monogame
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;

        Rectangle screen, mowerRect;
        Texture2D grassTexture, mowerTexture;

        SoundEffect mowerSound;
        SoundEffectInstance mowerSoundInstance;

        Vector2 mowerSpeed;

        List<Rectangle> grassTiles;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            screen = new Rectangle(0, 0, 600, 500);

            _graphics.PreferredBackBufferWidth = screen.Width;
            _graphics.PreferredBackBufferHeight = screen.Height;
            _graphics.ApplyChanges();

            mowerRect = new Rectangle(100, 100, 30, 30);
            
            grassTiles = new List<Rectangle>();
            for (int x = 0; x < screen.Width; x += 5)
                for (int y = 0; y < screen.Height; y += 5)
                    grassTiles.Add(new Rectangle(x, y, 5, 5));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mowerTexture = Content.Load<Texture2D>("mower");
            grassTexture = Content.Load<Texture2D>("long_grass");
            mowerSound = Content.Load<SoundEffect>("mower_sound");

            mowerSoundInstance = mowerSound.CreateInstance();
            mowerSoundInstance.IsLooped = true;

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();

            for (int i = 0; i < grassTiles.Count; i++)
                if (mowerRect.Contains(grassTiles[i]))
                {
                    grassTiles.Remove(grassTiles[i]);
                    i--;
                }
            
            mowerSpeed = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.W))
                mowerSpeed.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                mowerSpeed.Y += 1;
            if (keyboardState.IsKeyDown(Keys.A))
                mowerSpeed.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                mowerSpeed.X += 1;

            mowerRect.Offset(mowerSpeed);

            if (mowerSpeed == Vector2.Zero)
                mowerSoundInstance.Stop();
            else mowerSoundInstance.Play();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.LightGreen);

            _spriteBatch.Begin();

            foreach (Rectangle grass in grassTiles)
                _spriteBatch.Draw(grassTexture, grass, Color.White);

            _spriteBatch.Draw(mowerTexture, mowerRect, Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
