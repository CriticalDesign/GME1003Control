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
        private Texture2D _mySelf, _myCroc;
        private SpriteFont _gameFont;
        private float _mySelfX, _mySelfY, _crocX, _crocY, _crocSpeed, _mySelfSpeed;
        private Color _mySelfColor, _crocHero;
        private Random _myRNG;
        private int _mySelfHealth;
        private bool _heroIsDead;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _mySelfX = 0;
            _mySelfY = 0;
            _crocX = 1000;
            _crocY = 200;
            _crocSpeed = 500;
            _mySelfSpeed = 500;
            _mySelfColor = Color.White;
            _crocHero = Color.White;
            _mySelfHealth = 1;

            _heroIsDead = false;

            _myRNG = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mySelf = Content.Load<Texture2D>("Me");
            _myCroc = Content.Load<Texture2D>("hero");
            _gameFont = Content.Load<SpriteFont>("GameFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (_mySelfHealth <= 0)
            {
                _heroIsDead = true;
            }

            if (!_heroIsDead)
            {
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
                if(_mySelfX < 0)
                    _mySelfX = 0;
                if(_mySelfX > _graphics.PreferredBackBufferWidth - _mySelf.Width)
                    _mySelfX = _graphics.PreferredBackBufferWidth - _mySelf.Width; 

                */
                _mySelfX = MathHelper.Clamp(_mySelfX, 0, _graphics.PreferredBackBufferWidth - _mySelf.Width);
                _mySelfY = MathHelper.Clamp(_mySelfY, 0, _graphics.PreferredBackBufferHeight - _mySelf.Height);



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

                Rectangle _crocHitBox = new Rectangle((int)_crocX, (int)_crocY, _myCroc.Width, _myCroc.Height);

                _crocX -= (_crocSpeed * deltaTime) / 2;


                if (_crocX < -200 )
                {
                    _crocX = 1000;   
                }

                if (_mySelfHitBox.Intersects(_crocHitBox))
                {
                    _crocX = 1000;
                    _mySelfHealth--;
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

                if (_mySelfHitBox.Intersects(_crocHitBox))
                {
                    _mySelfColor = new Color(128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128));
                    _crocHero = new Color(128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128), 128 + _myRNG.Next(1, 128));
                }
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (!_heroIsDead)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Draw(_mySelf, new Vector2(_mySelfX, _mySelfY), _mySelfColor);

                _spriteBatch.DrawString(_gameFont, "" + _mySelfHealth, new Vector2(_mySelfX + _mySelf.Width / 2, _mySelfY - 50), Color.White);
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                string gameOverText = "Game over, thanks for playing. <3";
                int screenCenterX = _graphics.PreferredBackBufferWidth / 2;
                int screenCenterY = _graphics.PreferredBackBufferHeight / 2;
                int textHalfWidth = (int)_gameFont.MeasureString(gameOverText).X / 2;
                int textHalfHeight = (int)_gameFont.MeasureString(gameOverText).Y / 2;
                _spriteBatch.DrawString(_gameFont, gameOverText, new Vector2(screenCenterX - textHalfWidth, screenCenterY - textHalfHeight), Color.White);
            }
            _spriteBatch.Draw(_myCroc, new Vector2(_crocX,_crocY), _crocHero);



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
