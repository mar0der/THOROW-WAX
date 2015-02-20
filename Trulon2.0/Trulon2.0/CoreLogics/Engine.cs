﻿using System;
using System.Threading;
using Trulon.Models.Items;

namespace Trulon.CoreLogics
{
    using global::Trulon.Models;
    using global::Trulon.Models.Maps;

    #region Using Statements

    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Config;
    using Enums;
    using Models.Entities;
    using Models.Entities.NPCs;
    using Models.Entities.NPCs.Allies;
    using Models.Entities.NPCs.Enemies;
    using Models.Entities.Players;
    using Models.Items.Equipments;
    #endregion

    #region Engine Summary
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    #endregion
    public class Engine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D t;

        private Texture2D backgroundTexture;
        //Loading Entites
        private Player player;
        private Vendor vendor;
        private IList<Enemy> enemies;
        private Map[] maps = new Map[3];
        private int currentMap = 0;

        private IList<Potion> timeoutItems;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private int countDown;
        private int indexFrame;
        private bool isMoving;
        private bool isAttacking;
        private Texture2D[] AnimationsRight;
        private Texture2D[] AnimationsLeft;
        private Texture2D[] AnimationsRightAttack;
        private Texture2D[] AnimationsLeftAttack;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Resources/Images";
        }

        #region Initialize Summary
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        #endregion
        protected override void Initialize()
        {
            //Sets screen size
            this.graphics.PreferredBackBufferWidth = Config.ScreenWidth;
            this.graphics.PreferredBackBufferHeight = Config.ScreenHeight;
            graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            //setting entites on the scene
            this.player = new Barbarian(0, 0);
            this.player.PlayerEquipment.CurrentEquipment.Add(EquipmentSlots.RightHand, new Sword());
            this.vendor = new Vendor(500, 500);
            this.enemies = new List<Enemy>()
            {
                new Boss(100, 200),
                new Demon(0, 111),
                new Goblin(300, 200),
                new Orc(400, 200),
                new Troll(500, 200)
            };

            this.timeoutItems = new List<Potion>();
            maps[0] = new Level1();
            maps[1] = new Level2();
            maps[2] = new Level3();

            base.Initialize();
        }

        #region LoadContent Summary
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        #endregion
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here.
            //Load map image
            this.backgroundTexture = this.Content.Load<Texture2D>("MapImages/TrulonHomeMap");

            //Load the player resources
            this.player.Initialize(Content.Load<Texture2D>(Assets.BarbarianImages[0]), this.player.Position);

            AnimationsRight = new[]
            {
                Content.Load<Texture2D>(Assets.BarbarianImages[0]),
                Content.Load<Texture2D>(Assets.BarbarianImages[1]),
                Content.Load<Texture2D>(Assets.BarbarianImages[2]),
                Content.Load<Texture2D>(Assets.BarbarianImages[3])
            };

            AnimationsLeft = new[]
            {
                Content.Load<Texture2D>(Assets.BarbarianImages[4]),
                Content.Load<Texture2D>(Assets.BarbarianImages[5]),
                Content.Load<Texture2D>(Assets.BarbarianImages[6]),
                Content.Load<Texture2D>(Assets.BarbarianImages[7])
            };

            AnimationsRightAttack = new[]
            {
                Content.Load<Texture2D>(Assets.BarbarianImages[8]),
                Content.Load<Texture2D>(Assets.BarbarianImages[9]),
                Content.Load<Texture2D>(Assets.BarbarianImages[10]),
                Content.Load<Texture2D>(Assets.BarbarianImages[11])
            };

            AnimationsLeftAttack = new[]
            {
                Content.Load<Texture2D>(Assets.BarbarianImages[12]),
                Content.Load<Texture2D>(Assets.BarbarianImages[13]),
                Content.Load<Texture2D>(Assets.BarbarianImages[14]),
                Content.Load<Texture2D>(Assets.BarbarianImages[15])
            };
            //Load the vendor resources
            this.vendor.Initialize(Content.Load<Texture2D>(Assets.Vendor[0]), this.vendor.Position);

            foreach (var enemy in enemies)
            {
                enemy.Initialize(enemy is Goblin ? Content.Load<Texture2D>(Assets.GoblinImages[0]) : 
                Content.Load<Texture2D>(Assets.OrcImages[0]), enemy.Position);
            }
            //create line
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.White });

        }

        #region UnloadContent Summary
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        #endregion
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #region GameUpdate
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        #endregion
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Save previous state of the keyboard to determine single key presses
            previousKeyboardState = currentKeyboardState;
            //Read the current state of the keyboard and store it
            currentKeyboardState = Keyboard.GetState();

            //Update player
            this.player.Update(maps[currentMap]);

            //update enemies
            foreach (var enemy in enemies)
            {
                enemy.Update();
            }

            var enemiesInRange = this.player.GetEnemiesInRange(enemies);
            if (enemiesInRange.Count > 0)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                     this.player.Attack(enemiesInRange);
                }
            }

            for (var i = 0; i < this.enemies.Count; i++)
            {
                if (!this.enemies[i].IsAlive)
                {
                    this.player.AddCoins(this.enemies[i]);
                    this.player.AddExperience(this.enemies[i]);
                    var equipmentDrop = ItemGenerator.GetEquipmentItem();
                    this.player.Inventory.Add(equipmentDrop);
                    var potionDrop = ItemGenerator.GetPotionItem();
                    this.player.Inventory.Add(potionDrop);
                      this.enemies.RemoveAt(i);
                    break;
                }
            }

            if (this.enemies.Count == 0)
            {
                //TODO
            }

            //Testing inventory
            //Equipment
            if (currentKeyboardState.IsKeyDown(Keys.E))
            {
                foreach (var item in this.player.Inventory)
                {
                    var equipment = item as Equipment;
                    if (equipment != null)
                    {
                        this.player.UseEquipment(equipment);
                        break;
                    }
                }
            }
            //Potions
            if (currentKeyboardState.IsKeyDown(Keys.R))
            {
                foreach (var item in this.player.Inventory)
                {
                    var potion = item as Potion;
                    if (potion != null)
                    {
                        this.player.DrinkPotion(potion);
                        this.timeoutItems.Add(potion);
                        break;
                    }
                }
            }

            //Check for timeout items
            CheckForTimedoutItems();
            
            //Check for player is moving
            UpdateInput();
            if (isMoving || isAttacking)
            {
                this.AnimatePlayer();
            }
                
            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();

            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Up) ||
                newState.IsKeyDown(Keys.Down) ||
                newState.IsKeyDown(Keys.Right) ||
                newState.IsKeyDown(Keys.Left))
            {
                // If not down last update, key has just been pressed.
                if (!previousKeyboardState.IsKeyDown(Keys.Up) ||
                    !previousKeyboardState.IsKeyDown(Keys.Down) ||
                    !previousKeyboardState.IsKeyDown(Keys.Right) ||
                    !previousKeyboardState.IsKeyDown(Keys.Left))
                {
                    isMoving = true;
                }
            }
            else if (previousKeyboardState.IsKeyDown(Keys.Up) ||
                     previousKeyboardState.IsKeyDown(Keys.Down) ||
                     previousKeyboardState.IsKeyDown(Keys.Right) ||
                     previousKeyboardState.IsKeyDown(Keys.Left))
            {
                // Key was down last update, but not down now, so
                // it has just been released.

                isMoving = false;
            }

            if (newState.IsKeyDown(Keys.Space))
            {
                // If not down last update, key has just been pressed.
                if (!previousKeyboardState.IsKeyDown(Keys.Space))
                {
                    isAttacking = true;
                }
            }
            else if (previousKeyboardState.IsKeyDown(Keys.Space))
            {
                // Key was down last update, but not down now, so
                // it has just been released.

                isAttacking = false;
            }

            // Update saved state.
            previousKeyboardState = newState;
        }

        private void CheckForTimedoutItems()
        {
            for (int i = 0; i < timeoutItems.Count; i++)
            {
                if (timeoutItems[i].Countdown == 0)
                {
                    var item = timeoutItems[i];
                    item.HasTimedOut = true;

                    this.player.RemovePotionBuff(item);
                    this.player.Inventory.Remove(item);
                    this.timeoutItems.Remove(item);
                    break;
                }
                timeoutItems[i].Countdown--;
            }
        }

        #region GameDraw Summary
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        #endregion
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, backgroundTexture.Width, backgroundTexture.Height), Color.White);
            
            this.player.Draw(spriteBatch);

            this.vendor.Draw(spriteBatch);

            foreach (var enemy in enemies)
            {
                enemy.Draw(this.spriteBatch);
            }

            this.DrawLine(spriteBatch,
                new Vector2(0, 495),
                new Vector2(175, 495));

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void AnimatePlayer()
        {
            if (countDown == 0)
            {
                if (indexFrame >= AnimationsRight.Length)
                {
                    indexFrame = 0;
                }
                //change direction
                if (isAttacking)
                {
                    if (this.player.PreviousDirection == "right")
                    {
                        this.player.Image = this.AnimationsRightAttack[indexFrame++];
                    }
                    else if (this.player.PreviousDirection == "left")
                    {
                        this.player.Image = this.AnimationsLeftAttack[indexFrame++];
                    }
                }
                else if (this.player.PreviousDirection == "right")
                {
                    this.player.Image = this.AnimationsRight[indexFrame++];
                }
                else
                {
                    this.player.Image = this.AnimationsLeft[indexFrame++];
                }

                countDown = 10;
            }

            countDown--;
        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle = 0;
                //(float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}
