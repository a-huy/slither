using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SnakeGameScreenManagement
{
    class MainMenuScreen : MenuScreen
    {
        Texture2D mainTitleBackground;

        public MainMenuScreen()
        {
            MenuEntry playGameMenuEntry = new MenuEntry("Start New Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Change Options");
            MenuEntry helpMenuEntry = new MenuEntry("How to Play");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            helpMenuEntry.Selected += HelpMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(helpMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new PlayScreen());
        }

        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void HelpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HelpScreen(), e.PlayerIndex);
        }

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit?";
            MessageBoxScreen confirmMessageBox = new MessageBoxScreen(message);
            confirmMessageBox.Accepted += ConfirmMessageBoxAccepted;
            ScreenManager.AddScreen(confirmMessageBox, playerIndex);
        }

        void ConfirmMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        public override void LoadContent()
        {
            mainTitleBackground = ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/maintitlebackground");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 titleBackgroundSize = new Vector2(550, 128);
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Vector2 titleBackgroundPosition = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - titleBackgroundSize.X) / 2, 64 - transitionOffset * 100);
            Rectangle titleBackgroundRect = new Rectangle(0, 0, (int)titleBackgroundSize.X, (int)titleBackgroundSize.Y);
            Color color = new Color(Color.White.R, Color.White.G, Color.White.B, TransitionAlpha);
            spriteBatch.Begin();
            spriteBatch.Draw(mainTitleBackground, titleBackgroundPosition, titleBackgroundRect, color, 0f,
                Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
