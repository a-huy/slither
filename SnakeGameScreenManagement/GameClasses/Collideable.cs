using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGameScreenManagement
{
    class Collideable
    {
        public enum CollideType { Food, Rock, Mover };
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        Point frameSize;
        int collisionOffset = 0;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 200;
        Point currentFrame = new Point(0, 0);
        Point sheetSize;
        CollideType type;

        public Collideable(Texture2D texture, Vector2 position, Vector2 velocity, Point frameSize, Point sheetSize, CollideType type)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.frameSize = frameSize;
            this.sheetSize = sheetSize;
            this.type = type;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public CollideType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Point FrameSize
        {
            get { return frameSize; }
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)position.X + collisionOffset, (int)position.Y + collisionOffset, frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public bool collides(Rectangle target)
        {
            return collisionRect.Intersects(target);
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y) currentFrame.Y = 0;
                }
            }

            position += velocity;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
        }
    }
}
