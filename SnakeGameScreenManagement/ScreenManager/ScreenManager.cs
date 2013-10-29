using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SnakeGameScreenManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> gScreens = new List<GameScreen>();
        List<GameScreen> gScreensToUpdate = new List<GameScreen>();

        InputState inState = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D blankTexture;
        Song backgroundMusic;

        bool isInitialized;
        bool traceEnabled;

        public SpriteBatch SpriteBatch
        { 
            get { return spriteBatch; }
        }

        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
        }

        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        public ScreenManager(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();
            isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts/font1");
            blankTexture = content.Load<Texture2D>("Backgrounds/blank");
            backgroundMusic = content.Load<Song>("Audio/snakebackgroundmusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;

            foreach (GameScreen gs in gScreens)
            {
                gs.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (GameScreen gs in gScreens)
            {
                gs.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            inState.Update();
            gScreensToUpdate.Clear();
            foreach (GameScreen gs in gScreens) gScreensToUpdate.Add(gs);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (gScreensToUpdate.Count > 0)
            {
                GameScreen topScreen = gScreensToUpdate[gScreensToUpdate.Count - 1];
                gScreensToUpdate.RemoveAt(gScreensToUpdate.Count - 1);

                topScreen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (topScreen.ScreenState == ScreenState.TransitionOn ||
                    topScreen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        topScreen.HandleInput(inState);
                        otherScreenHasFocus = true;
                    }

                    if (!topScreen.IsPopup) coveredByOtherScreen = true;
                }
            }

            if (traceEnabled) TraceScreens();
        }

        void TraceScreens()
        {
            List<string> gScreenNames = new List<string>();
            foreach (GameScreen gs in gScreens)
                gScreenNames.Add(gs.GetType().Name);
            Trace.WriteLine(string.Join(", ", gScreenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen gs in gScreens)
            {
                if (gs.ScreenState == ScreenState.Hidden) continue;
                gs.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen gs, PlayerIndex? controllingPlayer)
        {
            gs.ControllingPlayer = controllingPlayer;
            gs.ScreenManager = this;
            gs.IsExiting = false;

            if (isInitialized) gs.LoadContent();
            gScreens.Add(gs);
        }

        public void RemoveScreen(GameScreen gs)
        {
            if (isInitialized) gs.UnloadContent();

            gScreens.Remove(gs);
            gScreensToUpdate.Remove(gs);
        }

        public GameScreen[] GetScreens()
        {
            return gScreens.ToArray();
        }

        public void FadeBackBufferToBlack(int alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height),
                new Color(0, 0, 0, (byte)alpha));
            spriteBatch.End();
        }
    }
}
