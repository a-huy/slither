using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace SnakeGameScreenManagement
{
    class PlayScreen : GameScreen
    {
        ContentManager content;
        SpriteFont playFont;

        int maxWidth, maxHeight;
        List<SnakePart> snake = new List<SnakePart>();
        List<Collideable> collideList = new List<Collideable>();
        Texture2D bodySheet, tailSheet, headSheet, tongueSheet, foodSheet, rockSheet, moverSheet, background, gameBar;
        int timeUntilMove = 0;
        int rockSpawnChanceMax = 5;
        int currentScore = 0;
        Vector2 facingChange;
        bool ateFood;
        bool changeRockSpawnChanceMax = false;
        Random random = new Random();
        TimeSpan elapsedTime;
        int elapsedSeconds;
        int elapsedMinutes;
        SoundEffect death;
        SoundEffectInstance deathInstance;

        SnakePart Tail { get { return snake[snake.Count - 1]; } }
        SnakePart Head { get { return snake[0]; } }
        SnakePart Tongue;
        Vector2 Up = new Vector2(0, -16);
        Vector2 Down = new Vector2(0, 16);
        Vector2 Left = new Vector2(-16, 0);
        Vector2 Right = new Vector2(16, 0);

        public PlayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.5);
        }

        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
            elapsedSeconds = 0;
            elapsedMinutes = 0;
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
            playFont = content.Load<SpriteFont>("Fonts/font1");
            maxWidth = ScreenManager.Game.GraphicsDevice.Viewport.Width;
            maxHeight = ScreenManager.Game.GraphicsDevice.Viewport.Height;
            facingChange = new Vector2(16, 0);
            tailSheet = content.Load<Texture2D>("SpriteSheets/snaketailsheet");
            bodySheet = content.Load<Texture2D>("SpriteSheets/snakebodysheet");
            headSheet = content.Load<Texture2D>("SpriteSheets/snakeheadsheet");
            tongueSheet = content.Load<Texture2D>("SpriteSheets/snaketonguesheet");
            foodSheet = content.Load<Texture2D>("SpriteSheets/foodsheet");
            rockSheet = content.Load<Texture2D>("SpriteSheets/rocksheet");
            moverSheet = content.Load<Texture2D>("SpriteSheets/moversheet");
            background = content.Load<Texture2D>("Backgrounds/background1");
            gameBar = content.Load<Texture2D>("SpriteSheets/gamebarbackground");
            death = content.Load<SoundEffect>("Audio/death");
            for (int i = 0; i < 3; ++i)
            {
                snake.Insert(0, new SnakePart(bodySheet, new Vector2((16 * i), 80), new Point(16, 16), 0, new Point(6, 1), Vector2.Zero));
            }

            collideList.Add(new Collideable(foodSheet, new Vector2(384, 384), Vector2.Zero, new Point(16, 16), new Point(2, 1), Collideable.CollideType.Food));

            Tongue = new SnakePart(tongueSheet, Head.Position + facingChange, new Point(16, 16), 0, new Point(4, 1), Vector2.Zero);
            Head.Texture = headSheet;
            Tail.Texture = tailSheet;

            deathInstance = death.CreateInstance();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                ++timeUntilMove;
                if (timeUntilMove >= 2)
                {
                    timeUntilMove = 0;
                    SnakePart index = snake[0];
                    if (ateFood) { ateFood = false; }
                    else snake.RemoveAt(snake.Count - 1);
                    if (facingChange.X != 0)
                        snake.Insert(0, new SnakePart(headSheet, index.Position + facingChange, new Point(16, 16), 0, new Point(1, 0), Vector2.Zero));
                    else
                        snake.Insert(0, new SnakePart(headSheet, index.Position + facingChange, new Point(16, 16), 0, new Point(0, 0), Vector2.Zero));
                }
                if (Head.Position == snake[2].Position) Head.Position = snake[1].Position + (snake[1].Position - snake[2].Position);

                for (int i = 1; i < snake.Count - 1; ++i)
                {
                    snake[i].Texture = bodySheet;
                    if (snake[i].isAheadOf(snake[i - 1]) || snake[i].isBehind(snake[i - 1])) snake[i].CurrentFrame = new Point(0, 0);
                    if (snake[i].isAbove(snake[i - 1]) || snake[i].isBelow(snake[i - 1])) snake[i].CurrentFrame = new Point(1, 0);
                    if ((snake[i - 1].isBelow(snake[i]) && snake[i + 1].isBehind(snake[i])) || (snake[i - 1].isBehind(snake[i]) && snake[i + 1].isBelow(snake[i])))
                        snake[i].CurrentFrame = new Point(2, 0);
                    if ((snake[i - 1].isAbove(snake[i]) && snake[i + 1].isBehind(snake[i])) || (snake[i - 1].isBehind(snake[i]) && snake[i + 1].isAbove(snake[i])))
                        snake[i].CurrentFrame = new Point(3, 0);
                    if ((snake[i - 1].isAbove(snake[i]) && snake[i + 1].isAheadOf(snake[i])) || (snake[i - 1].isAheadOf(snake[i]) && snake[i + 1].isAbove(snake[i])))
                        snake[i].CurrentFrame = new Point(4, 0);
                    if ((snake[i - 1].isBelow(snake[i]) && snake[i + 1].isAheadOf(snake[i])) || (snake[i - 1].isAheadOf(snake[i]) && snake[i + 1].isBelow(snake[i])))
                        snake[i].CurrentFrame = new Point(5, 0);
                }
                if (Head.isAheadOf(snake[1])) Head.CurrentFrame = new Point(0, 0);
                if (Head.isBelow(snake[1])) Head.CurrentFrame = new Point(1, 0);
                if (Head.isBehind(snake[1])) Head.CurrentFrame = new Point(2, 0);
                if (Head.isAbove(snake[1])) Head.CurrentFrame = new Point(3, 0);

                Tail.Texture = tailSheet;
                if (Tail.isBehind(snake[snake.Count - 2])) Tail.CurrentFrame = new Point(0, 0);
                if (Tail.isAbove(snake[snake.Count - 2])) Tail.CurrentFrame = new Point(1, 0);
                if (Tail.isAheadOf(snake[snake.Count - 2])) Tail.CurrentFrame = new Point(2, 0);
                if (Tail.isBelow(snake[snake.Count - 2])) Tail.CurrentFrame = new Point(3, 0);

                // Collision detection between collideables and snake head
                for (int i = 0; i < collideList.Count; ++i)
                {
                    if (collideList[i].collides(Head.collisionRect))
                    {
                        if (collideList[i].Type == Collideable.CollideType.Food)
                        {
                            ateFood = true;
                            collideList.Remove(collideList[i]);
                            currentScore += 100;
                            collideList.Add(new Collideable(foodSheet, new Vector2(random.Next(0, 64) * 16, random.Next(5, 48) * 16), Vector2.Zero, new Point(16, 16), new Point(2, 1), 
                                Collideable.CollideType.Food));
                            // Adds a new rock if able, but makes sure it doesn't appear close to snake head
                            int rockSpawnChance = random.Next(0, rockSpawnChanceMax);
                            int moverSpawnChance = random.Next(0, 20);
                            if (moverSpawnChance == 0)
                            {
                                Collideable newRock = new Collideable(moverSheet, new Vector2(random.Next(0, 32) * 32, random.Next(3, 24) * 32), 
                                    new Vector2(random.Next(1, 7), random.Next(1, 7)), new Point(32, 32), new Point(8, 1), Collideable.CollideType.Mover);
                                while (isNearSnakeHead(newRock.collisionRect))
                                    newRock.Position = new Vector2(random.Next(0, 32) * 32, random.Next(0, 24) * 32);
                                collideList.Add(newRock);
                            }
                            else
                            {
                                if (rockSpawnChance == 0)
                                {
                                    Collideable newRock = new Collideable(rockSheet, new Vector2(random.Next(0, 32) * 32, random.Next(3, 24) * 32), Vector2.Zero, new Point(32, 32),
                                        new Point(1, 1), Collideable.CollideType.Rock);
                                    while (isNearSnakeHead(newRock.collisionRect))
                                        newRock.Position = new Vector2(random.Next(0, 32) * 32, random.Next(0, 24) * 32);
                                    collideList.Add(newRock);
                                }
                            }
                        }
                        else if (collideList[i].Type == Collideable.CollideType.Rock || collideList[i].Type == Collideable.CollideType.Mover)
                        {
                            deathInstance.Play();
                            ScreenManager.AddScreen(new GameOverScreen(), ControllingPlayer);
                        }
                    }
                }
                // Collision detection between snake head and its tail or the edge of the screen
                for (int i = 3; i < snake.Count; ++i)
                    if (snake[i].collides(Head.collisionRect))
                    {
                        deathInstance.Play();
                        ScreenManager.AddScreen(new GameOverScreen(), ControllingPlayer);
                    }
                if (Head.Position.X > maxWidth - 16 || Head.Position.X < 0 || Head.Position.Y > maxHeight - 16 ||
                    Head.Position.Y < 80)
                {
                    deathInstance.Play();
                    ScreenManager.AddScreen(new GameOverScreen(), ControllingPlayer);
                }
                Tongue.Position = Head.Position + (Head.Position - snake[1].Position);
                Tongue.CurrentFrame = Head.CurrentFrame;

                // Collision detection between each collideable with all other collidables
                for (int i = 0; i < collideList.Count; ++i)
                {
                    for (int c = 1; c < collideList.Count; ++c)
                    {
                        if (i == c) continue;
                        if (collideList[i].collides(collideList[c].collisionRect))
                        {
                            if (collideList[i].Type == Collideable.CollideType.Mover)
                                collideList[i].Velocity = new Vector2(-collideList[i].Velocity.X, -collideList[i].Velocity.Y);
                            else collideList[i].Position = new Vector2(random.Next(0, 64) * 16, random.Next(5, 48) * 16);
                        }
                    }
                }

                // Increases the chance of spawning a rock after every 2 minutes
                if ((gameTime.TotalGameTime.Minutes % 2 == 0) && changeRockSpawnChanceMax)
                {
                    changeRockSpawnChanceMax = false;
                    --rockSpawnChanceMax;
                    if (rockSpawnChanceMax < 0) rockSpawnChanceMax = 0;
                }
                else if (gameTime.TotalGameTime.Minutes % 2 != 0) changeRockSpawnChanceMax = true;

                // Collision detection between Movers and the edge of the playable screen
                foreach (Collideable c in collideList)
                {
                    if (c.Type == Collideable.CollideType.Mover)
                    {
                        if (c.Position.X > 992 || c.Position.X < 0) c.Velocity = new Vector2(-c.Velocity.X, c.Velocity.Y);
                        if (c.Position.Y > 736 || c.Position.Y < 80) c.Velocity = new Vector2(c.Velocity.X, -c.Velocity.Y);
                    }
                    c.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                }

                elapsedTime += gameTime.ElapsedGameTime;
                if (elapsedTime > TimeSpan.FromSeconds(1))
                {
                    elapsedTime = TimeSpan.Zero;
                    ++elapsedSeconds;
                }
                if (elapsedSeconds > 59)
                {
                    elapsedSeconds = 0;
                    ++elapsedMinutes;
                }
            }
        }

        public override void HandleInput(InputState inState)
        {
            if (inState == null) throw new ArgumentNullException("inState");
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState kState = inState.CurrentKeyBoardStates[playerIndex];
            GamePadState gState = inState.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gState.IsConnected && inState.GamePadWasConnected[playerIndex];

            if (inState.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if (kState.IsKeyDown(Keys.Up) || gState.ThumbSticks.Left.Y > .5) facingChange = Up;
                if (kState.IsKeyDown(Keys.Down) || gState.ThumbSticks.Left.Y < -.5) facingChange = Down;
                if (kState.IsKeyDown(Keys.Left) || gState.ThumbSticks.Left.X < -.5) facingChange = Left;
                if (kState.IsKeyDown(Keys.Right) || gState.ThumbSticks.Left.X > .5) facingChange = Right;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 textSize = playFont.MeasureString("Time: " + elapsedMinutes + " Minutes, " + elapsedSeconds + " Seconds");

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            foreach (SnakePart sp in snake) sp.Draw(gameTime, spriteBatch);
            Tongue.Draw(gameTime, spriteBatch);
            foreach (Collideable c in collideList) c.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, 1024, 768), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(gameBar, Vector2.Zero, new Rectangle(0, 0, 1024, 80), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(playFont, "Time: " + elapsedMinutes + " Minutes, " + elapsedSeconds + " Seconds",
                new Vector2(1024 - textSize.X - 30, (80 - textSize.Y) / 2), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(playFont, "Score: " + currentScore, new Vector2(30, (80 - textSize.Y) / 2), Color.Black, 0,
                Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.End();

            if (TransitionPosition > 0) ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }

        bool isNearSnakeHead(Rectangle target)
        {
            return target.Intersects(new Rectangle((int)Head.Position.X - 128, (int)Head.Position.Y - 128, 272, 272));
        }
    }
}
