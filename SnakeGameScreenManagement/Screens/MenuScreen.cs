using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SnakeGameScreenManagement
{
    abstract class MenuScreen : GameScreen
    {
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        SoundEffect selectionChange;
        SoundEffectInstance selectionChangeInstance;

        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        public MenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void HandleInput(InputState inState)
        {
            selectionChange = ScreenManager.Game.Content.Load<SoundEffect>("Audio/menuselectionchange");
            selectionChangeInstance = selectionChange.CreateInstance();
            if (inState.IsMenuUp(ControllingPlayer))
            {
                selectedEntry--;
                if (selectedEntry < 0) selectedEntry = menuEntries.Count - 1;
                selectionChangeInstance.Play();
            }

            if (inState.IsMenuDown(ControllingPlayer))
            {
                selectedEntry++;
                if (selectedEntry > menuEntries.Count - 1) selectedEntry = 0;
                selectionChangeInstance.Play();
            }

            PlayerIndex playerIndex;
            if (inState.IsMenuSelect(ControllingPlayer, out playerIndex)) OnSelectEntry(selectedEntry, playerIndex);
            if (inState.IsMenuCancel(ControllingPlayer, out playerIndex)) OnCancel(playerIndex);
        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        protected virtual void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            for (int argi = 0; argi < menuEntries.Count; ++argi)
            {
                bool isSelected = IsActive && (argi == selectedEntry);
                menuEntries[argi].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont spriteFont = ScreenManager.SpriteFont;

            Vector2 position = new Vector2(312, 250);
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            if (ScreenState == ScreenState.TransitionOn) position.X -= transitionOffset * 256;
            else position.X -= transitionOffset * 512;

            spriteBatch.Begin();

            for (int argi = 0; argi < menuEntries.Count; ++argi)
            {
                MenuEntry menuEntry = menuEntries[argi];
                bool isSelected = IsActive && (argi == selectedEntry);
                menuEntry.Draw(this, position, isSelected, gameTime);
                position.Y += menuEntry.GetHeight(this) + 50;
            }

            //Vector2 titlePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 160);
            //Vector2 titleOrigin = spriteFont.MeasureString(menuTitle) / 2;
            //Color titleColor = new Color(Color.Teal, TransitionAlpha);
            //float titleScale = 1.25f;

            //titlePosition.Y -= transitionOffset * 100;
            //spriteBatch.DrawString(spriteFont, menuTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);
                
            spriteBatch.End();
        }
    }
}
