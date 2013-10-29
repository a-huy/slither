using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGameScreenManagement
{
    class CreditsScreen : GameScreen
    {
        ContentManager content;
        Texture2D creditsBackground;

        public CreditsScreen()
        {
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
            creditsBackground = content.Load<Texture2D>("Backgrounds/credits");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void HandleInput(InputState inState)
        {
            PlayerIndex playerIndex;
            if (inState.IsMenuSelect(ControllingPlayer, out playerIndex) || inState.IsMenuCancel(ControllingPlayer, out playerIndex))
                this.ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 backgroundSize = new Vector2(600, 450);
            Vector2 backgroundPosition = (viewportSize - backgroundSize) / 2;
            Rectangle backgroundRect = new Rectangle((int)backgroundPosition.X, (int)backgroundPosition.Y, (int)backgroundSize.X, (int)backgroundSize.Y);
            Color color = new Color(Color.White.R, Color.White.G, Color.White.B, TransitionAlpha);

            spriteBatch.Begin();
            spriteBatch.Draw(creditsBackground, backgroundRect, color);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
