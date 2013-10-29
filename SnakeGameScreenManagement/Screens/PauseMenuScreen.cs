using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGameScreenManagement
{
    class PauseMenuScreen : MenuScreen
    {
        Texture2D pausedTitleBackground;

        public PauseMenuScreen()
        {
            IsPopup = true;

            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }

        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        public override void LoadContent()
        {
            pausedTitleBackground = ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/pausedtitlebackground");
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
            spriteBatch.Draw(pausedTitleBackground, titleBackgroundPosition, titleBackgroundRect, color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

 	        base.Draw(gameTime);
        }
    }
}
