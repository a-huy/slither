using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGameScreenManagement
{
    class GameOverScreen : MenuScreen
    {
        Texture2D gameOverTitleBackground;

        public GameOverScreen()
        {
            IsPopup = true;

            MenuEntry startGameMenuEntry = new MenuEntry("Start New Game");
            MenuEntry returnMenuEntry = new MenuEntry("Return to Main Menu");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            returnMenuEntry.Selected += ReturnMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(returnMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        void StartGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new PlayScreen());
        }

        void ReturnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Return to title screen?";
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitToTitleAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox, ControllingPlayer);
        }

        void ConfirmExitToTitleAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitGameAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitGameAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        public override void LoadContent()
        {
            gameOverTitleBackground = ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/gameovertitlebackground");
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 titleBackgroundSize = new Vector2(550, 128);
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Vector2 titleBackgroundPosition = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - titleBackgroundSize.X) / 2, 64 - transitionOffset * 100);
            Rectangle titleBackgroundRect = new Rectangle(0, 0, (int)titleBackgroundSize.X, (int)titleBackgroundSize.Y);
            Color color = new Color(Color.White.R, Color.White.G, Color.White.B, TransitionAlpha);
            spriteBatch.Begin();
            spriteBatch.Draw(gameOverTitleBackground, titleBackgroundPosition, titleBackgroundRect, color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
