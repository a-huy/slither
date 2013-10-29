using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGameScreenManagement
{
    public class SnakePart
    {
        Texture2D texture;
        Vector2 position;
        Point frameSize;
        int collisionOffset;
        Point currentFrame;
        Point sheetSize;
        Vector2 speed;

        public SnakePart(Texture2D texture, Vector2 position, Point frameSize, int collisionOffset, Point sheetSize, Vector2 speed)
        {
            this.texture = texture;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.sheetSize = sheetSize;
            this.speed = speed;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Point CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
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

        public bool isBehind(SnakePart sp)
        {
            return position.X < sp.Position.X;
        }

        public bool isAheadOf(SnakePart sp)
        {
            return position.X > sp.Position.X;
        }

        public bool isAbove(SnakePart sp)
        {
            return position.Y < sp.Position.Y;
        }

        public bool isBelow(SnakePart sp)
        {
            return position.Y > sp.Position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        }
    }
}
