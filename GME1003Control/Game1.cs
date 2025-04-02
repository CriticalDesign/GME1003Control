using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GME1003Control
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _mySelf, _myHero;
        private SpriteFont _gameFont;
        private float _mySelfX, _mySelfY, _myHeroX, _myHeroY, _myHeroSpeed, _mySelfSpeed;
        private Color _mySelfColor, _myHeroColor;
        private Random _myRNG;
        private int _mySelfHealth;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _mySelfX = 0;
            _mySelfY = 0;
            _myHeroX = 1000;
            _myHeroY = 200;
            _myHeroSpeed = 500;
            _mySelfSpeed = 500;
            _mySelfColor = Color.White;
            _myHeroColor = Color.White;
            _mySelfHealth = 5;

            _myRNG = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mySelf = Content.Load<Texture2D>("Me");
            _myHero = Content.Load<Texture2D>("hero");
            _gameFont = Content.Load<SpriteFont>("GameFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            KeyboardState keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.W))
                _mySelfY -= _mySelfSpeed * deltaTime; 
            if (keyboardState.IsKeyDown(Keys.S))
                _mySelfY += _mySelfSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.A))
                _mySelfX -= _mySelfSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D))
                _mySelfX += _mySelfSpeed * deltaTime;
            /*
            if (keyboardState.IsKeyDown(Keys.Up))
                _myHeroY -= _myHeroSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.Down))
                _myHeroY += _myHeroSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.Left))
                _myHeroX -= _myHeroSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.Right))
                _myHeroX += _myHeroSpeed * deltaTime;
            */
            Rectangle _mySelfHitBox = new Rectangle((int)_mySelfX, (int)_mySelfY, _mySelf.Width, _mySelf.Height);

            Rectangle _myHeroHitBox = new Rectangle((int)_myHeroX, (int)_myHeroY, _myHero.Width, _myHero.Height);

            _myHeroX -= (_myHeroSpeed * deltaTime) / 2;
            if (_myHeroX < -200 || _mySelfHitBox.Intersects(_myHeroHitBox))
            {
                _myHeroX = 1000;
                _mySelfHealth -= 1;
            }

            //_myX = Mouse.GetState().Position.X;
            //_myY = Mouse.GetState().Position.Y;

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _mySelfColor = new Color(128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128));
            }



            if (Mouse.GetState().LeftButton == ButtonState.Pressed
                && _mySelfHitBox.Contains(Mouse.GetState().Position)
                )
            {
                _mySelfColor = new Color(128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128));
            }

            if(_mySelfHitBox.Intersects(_myHeroHitBox))
            {
                _mySelfColor = new Color(128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128));
                _myHeroColor = new Color(128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128));
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            if (_mySelfHealth > 0)
            {
                _spriteBatch.Draw(_mySelf, new Vector2(_mySelfX, _mySelfY), _mySelfColor);
            }
            _spriteBatch.Draw(_myHero, new Vector2(_myHeroX,_myHeroY), _myHeroColor);

            _spriteBatch.DrawString(_gameFont, "" + _mySelfHealth, new Vector2(100, 100), Color.White);


            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
