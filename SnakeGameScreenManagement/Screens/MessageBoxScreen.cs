using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SnakeGameScreenManagement
{
    class MessageBoxScreen : GameScreen
    {
        string message;
        Texture2D gradientTexture;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public MessageBoxScreen(string message)
            : this(message, true)
        { }

        public MessageBoxScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nA button, Space, Enter = OK" + "\nB button, Esc = Cancel";
            if (includeUsageText) this.message = message + usageText;
            else this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            gradientTexture = ScreenManager.Game.Content.Load<Texture2D>("Backgrounds/messageboxbackground");
        }

        public override void HandleInput(InputState inState)
        {
            PlayerIndex playerIndex;

            if (inState.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                if (Accepted != null) Accepted(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
            else if (inState.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                if (Cancelled != null) Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.SpriteFont;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            Vector2 backgroundSize = new Vector2(400, 150);
            Vector2 backgroundPosition = (viewportSize - backgroundSize) / 2;

            Rectangle backgroundRect = new Rectangle((int)backgroundPosition.X, (int)backgroundPosition.Y, (int)backgroundSize.X, (int)backgroundSize.Y);

            Color backgroundColor = new Color(Color.White.R, Color.White.G, Color.White.B, TransitionAlpha);
            Color textColor = new Color(0, 0, 0, TransitionAlpha);

            spriteBatch.Begin();
            spriteBatch.Draw(gradientTexture, backgroundRect, backgroundColor);
            spriteBatch.DrawString(font, message, textPosition, textColor);
            spriteBatch.End();
        }
    }
}
