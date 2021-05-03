using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D myship;
        Vector2 myship_pos;
        Vector2 myship_speed;
        Texture2D coin;
        Vector2 coin_pos;
        List<Vector2> coin_pos_list = new List<Vector2>();
        Texture2D enemyship;
        Vector2 enemyship_pos;
        Vector2 enemyship_speed;
        List<Vector2> enemy_pos_list = new List<Vector2>();
        Rectangle rec_myship;
        Rectangle rec_coin;
        bool hit = false;
        SoundEffect myshout;
        bool shout = false;
        SpriteFont gameFont;
        int pointSumma=0;
        Texture2D bullet;
        Vector2 bullet_pos;
        Vector2 bullet_speed;
        List<Vector2> bullet_pos_list = new List<Vector2>();
        Rectangle rec_bullet;
        Rectangle rec_enemy;



        double timeSinceLastBullet = 0;






        public Game1()
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

        // Funktion som kontrollerar kollision mellan 2 objekt
        public bool CheckCollision(Rectangle player, Rectangle mynt)
        {
            return player.Intersects(mynt);
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Random slump = new Random();
            for (int i = 0; i < 3; i++)
            {
                coin_pos.X = slump.Next(0, Window.ClientBounds.Width - 50);
                coin_pos.Y = slump.Next(0, Window.ClientBounds.Height - 50);
                coin_pos_list.Add(coin_pos);
            }

            Random slumpenemy = new Random();
            for (int i = 0; i < 5; i++)
            {
                enemyship_pos.X = slumpenemy.Next(0, Window.ClientBounds.Width - 50);
                enemyship_pos.Y = slumpenemy.Next(-200, 0);
                enemy_pos_list.Add(enemyship_pos);
            }


            myship_pos.X = 490;
            myship_pos.Y = Window.ClientBounds.Height;
            myship_speed.X = 3.5f;
            myship_speed.Y = 5.5f;
            enemyship_speed.Y = 0.5f;
            bullet_speed.Y = 20f;
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

            // TODO: use this.Content to load your game content here
            myship = Content.Load<Texture2D> ("Sprites/ship");
            coin = Content.Load<Texture2D>("Sprites/coin");
            enemyship = Content.Load<Texture2D>("Sprites/tripod");
            myshout = Content.Load<SoundEffect>("Sounds/yehaw");
            gameFont = Content.Load<SpriteFont>("Utskrift/GameFont");
            bullet = Content.Load<Texture2D>("Sprites/bullet");



        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();
            //höger och vänster
            if (keyboardState.IsKeyDown(Keys.Right))
                myship_pos.X = myship_pos.X + myship_speed.X;
            else if (keyboardState.IsKeyDown(Keys.Left))
                myship_pos.X = myship_pos.X - myship_speed.X;
            //upp och ner
            if (keyboardState.IsKeyDown(Keys.Up))
                myship_pos.Y = myship_pos.Y - myship_speed.Y;
            else if (keyboardState.IsKeyDown(Keys.Down))
                myship_pos.Y = myship_pos.Y + myship_speed.Y;
            //stopp höger / vänster
            if (myship_pos.X >= Window.ClientBounds.Width - myship.Width)
                myship_pos.X = Window.ClientBounds.Width - myship.Width;
            else if 
                (myship_pos.X <= 0)
                myship_pos.X = 0;

            //stopp upp / ner
            if (myship_pos.Y >= Window.ClientBounds.Height - myship.Height)
                myship_pos.Y = Window.ClientBounds.Height - myship.Height;
            else if
               (myship_pos.Y <= 0)
                myship_pos.Y = 0;

            for (int i = 0; i < enemy_pos_list.Count; i++)
            {
                Vector2 temp_pos;
                temp_pos.X = enemy_pos_list.ElementAt(i).X;
                temp_pos.Y = enemy_pos_list.ElementAt(i).Y;
                temp_pos.Y = temp_pos.Y + enemyship_speed.Y;
                enemy_pos_list.RemoveAt(i);
                enemy_pos_list.Insert(i, temp_pos);

            }
            //Skall EJ vara här, rec_myship = new Rectangle(Convert.ToInt32(myship_pos.X), Convert.ToInt32(myship_pos.Y), myship.Width, myship.Height);

            rec_myship = new Rectangle(Convert.ToInt32(myship_pos.X), Convert.ToInt32(myship_pos.Y), myship.Width, myship.Height);
            foreach (Vector2 cn in coin_pos_list.ToList()) //förklaring = ????  
            {                
                
                rec_coin = new Rectangle(Convert.ToInt32(cn.X), Convert.ToInt32(cn.Y), coin.Width, coin.Height);
                hit = CheckCollision(rec_myship, rec_coin);
                if (hit == true)
                {
                    coin_pos_list.Remove(cn);
                    hit = false;
                    pointSumma += 10;
                }
                if (coin_pos_list.Count == 0 && shout == false)
                {
                    myshout.Play();
                    shout = true;
                }


            }


            //myship_pos /* = myship_pos + */ += myship_speed;
            //myship_pos = myship_pos + myship_speed;

            /*if (myship_pos.X >= Window.ClientBounds.Width - myship.Width)
            {
                myship_speed.X = myship_speed.X * (-1);
            }
            else if (myship_pos.X <= 0)
            {
                myship_speed.X = myship_speed.X * (-1);
            }

            if (myship_pos.Y >= Window.ClientBounds.Height - myship.Height)
            {
                myship_speed.Y = myship_speed.Y * (-1);
            }
            else if (myship_pos.Y <= 0)
            {
                myship_speed.Y = myship_speed.Y * (-1);
            }
           */
            

            if (keyboardState.IsKeyDown(Keys.Space) && gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
            {

                bullet_pos.X = myship_pos.X + myship.Width / 2;
                bullet_pos.Y = myship_pos.Y;
                bullet_pos_list.Add(bullet_pos);

                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }



            for (int i = 0; i < bullet_pos_list.Count; i++)
            {
                Vector2 temp_bullet;
                temp_bullet.X = bullet_pos_list.ElementAt(i).X;
                temp_bullet.Y = bullet_pos_list.ElementAt(i).Y;
                temp_bullet.Y = temp_bullet.Y - bullet_speed.Y;
                if (temp_bullet.Y < 0)
                {
                    bullet_pos_list.RemoveAt(i);
                }
                else
                {
                    bullet_pos_list.RemoveAt(i);
                    bullet_pos_list.Insert(i, temp_bullet);
                }
            }

            foreach (Vector2 bullets in bullet_pos_list.ToList())
            {
                rec_bullet = new Rectangle(Convert.ToInt32(bullets.X), Convert.ToInt32(bullets.Y), bullet.Width, bullet.Height);
                foreach (Vector2 enemy in enemy_pos_list.ToList())
                {
                    rec_enemy = new Rectangle(Convert.ToInt32(enemy.X), Convert.ToInt32(enemy.Y), enemyship.Width, enemyship.Height);
                    hit = CheckCollision(rec_bullet, rec_enemy);
                    if (hit == true)
                    {
                        enemy_pos_list.Remove(enemy);
                        bullet_pos_list.Remove(bullets);
                        hit = false;
                        pointSumma += 20;
                    }
                }
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(myship, myship_pos , Color.White);

            foreach (Vector2 cn in coin_pos_list)
            {
                spriteBatch.Draw(coin, cn, Color.White);
            }

            foreach (Vector2 enemy in enemy_pos_list)
            {
                spriteBatch.Draw(enemyship, enemy, Color.White);
            }

            foreach (Vector2 bullets in bullet_pos_list)
            {
                spriteBatch.Draw(bullet, bullets, Color.White);
            }


            spriteBatch.DrawString(gameFont, "Poäng:" + pointSumma, new Vector2(10, 10), Color.White);

            


            spriteBatch.End();

            


            base.Draw(gameTime);
        }
    }
}
