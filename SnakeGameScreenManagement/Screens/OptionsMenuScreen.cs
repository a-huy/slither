using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SnakeGameScreenManagement
{
    class OptionsMenuScreen : MenuScreen
    {
        Texture2D optionsTitleBackground;
        MenuEntry fullScreenMenuEntry;
        MenuEntry musicVolumeMenuEntry;
        MenuEntry muteMusicMenuEntry;

        static bool activateFullScreen = false;
        static bool musicMuted = false;
        static int musicVolume = 1;

        public OptionsMenuScreen()
        {
            fullScreenMenuEntry = new MenuEntry(string.Empty);
            muteMusicMenuEntry = new MenuEntry(string.Empty);
            musicVolumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Back");

            fullScreenMenuEntry.Selected += FullScreenMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;
            muteMusicMenuEntry.Selected += MuteMusicMenuEntrySelected;
            musicVolumeMenuEntry.Selected += MusicVolumeMenuEntrySelected;

            MenuEntries.Add(musicVolumeMenuEntry);
            MenuEntries.Add(muteMusicMenuEntry);
            MenuEntries.Add(fullScreenMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void SetMenuEntryText()
        {
            muteMusicMenuEntry.Text = "Mute Background Music : " + (musicMuted ? "Yes" : "No");
            fullScreenMenuEntry.Text = "Full Screen : " + (activateFullScreen ? "On" : "Off");
            musicVolumeMenuEntry.Text = "Background Music Volume : " + musicVolume;
        }

        void FullScreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            activateFullScreen = !activateFullScreen;
            GraphicsDeviceManager graphics = ScreenManager.Game.Services.GetService(typeof(IGraphicsDeviceService)) as GraphicsDeviceManager;
            graphics.ToggleFullScreen();
            SetMenuEntryText();
        }

        void MusicVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ++musicVolume;
            if (musicVolume > 5) musicVolume = 0;
            MediaPlayer.Volume = musicVolume * .2f;
            SetMenuEntryText();
        }

        void MuteMusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicMuted = !musicMuted;
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            SetMenuEntryText();
        }

        public override void LoadContent()
        {
            optionsTitleBackground = ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/optionstitlebackground");
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
            spriteBatch.Draw(optionsTitleBackground, titleBackgroundPosition, titleBackgroundRect, color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
