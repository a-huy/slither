using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGameScreenManagement
{
    class MenuEntry
    {
        string text;
        float selectionFade;
        Texture2D buttonBackground;
        Texture2D buttonSpinner;
        int spinnerFrame = 0;
        int timeToUpdate = 0;
        Vector2 spinnerSize = new Vector2(44, 44);
        Vector2 buttonSize = new Vector2(650, 64);

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;

        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null) Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(string text)
        {
            this.text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected) selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else selectionFade = Math.Max(selectionFade - fadeSpeed, 0);

            if (isSelected)
            {
                timeToUpdate++;
                if (timeToUpdate > 4)
                {
                    timeToUpdate = 0;
                    spinnerFrame++;
                    if (spinnerFrame > 3) spinnerFrame = 0;
                }
            }
            else spinnerFrame = 0;
        }

        public virtual void Draw(MenuScreen screen, Vector2 position, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.Teal : Color.Black;

            float scale = 1 + 0.2f * selectionFade;

            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.SpriteFont;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            buttonBackground = screen.ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/buttonbackgroundoutline");
            buttonSpinner = screen.ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/buttonbackgroundspinner");
            Rectangle buttonRect = new Rectangle(0, 0, (int)buttonSize.X, (int)buttonSize.Y);
            Rectangle spinnerRect = new Rectangle((int)spinnerSize.X * spinnerFrame, 0, (int)spinnerSize.X, (int)spinnerSize.Y);

            Color buttonColor = new Color(Color.White.R, Color.White.G, Color.White.B, screen.TransitionAlpha);
            spriteBatch.Draw(buttonBackground, new Vector2(position.X - 125, position.Y - 30), buttonRect, buttonColor, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(buttonSpinner, new Vector2(position.X - 73, position.Y - 20), spinnerRect, buttonColor, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 1);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.SpriteFont.LineSpacing;
        }
    }
}
