using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlashyBall
{
    public class Ball
    {
        # region Fields

        // sprite and draw rectangle
        Texture2D sprite;
        Rectangle drawRectangle = new Rectangle();

        // window width and height
        int windowWidth;
        int windowHeight;

        // random number generator
        Random rand = new Random();

        // speed
        int speed;
        Vector2 velocity;

        // colors
        List<Color> colors;
        Color currentColor = Color.White;
        int currentColorIndex = -1;
        int totalColorChangeMilliseconds;
        int elapsedColorChangeMilliseconds = 0;

        # endregion

        # region Constructors

        public Ball(Texture2D sprite, int windowWidth, int windowHeight, int speed, List<Color> colors, int totalColorChangeMilliseconds)
        {
            this.sprite = sprite;

            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            ResetBall();

            this.speed = speed;
            this.velocity = new Vector2(RandomSpeed(), RandomSpeed());

            this.colors = colors;
            this.totalColorChangeMilliseconds = totalColorChangeMilliseconds;
            ChangeColor();
        }

        # endregion

        # region Properties

        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        public Color Color
        {
            get { return currentColor; }
        }

        # endregion

        # region Methods

        public void Update(GameTime gameTime)
        {
            // move ball
            drawRectangle.X += (int)velocity.X;
            drawRectangle.Y += (int)velocity.Y;

            // keep ball in window
            if (drawRectangle.Y < 0 || drawRectangle.Y > windowHeight - drawRectangle.Height)
            {
                InvertVelocity(false);
            }

            // change color after time
            elapsedColorChangeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedColorChangeMilliseconds > totalColorChangeMilliseconds)
            {
                elapsedColorChangeMilliseconds = 0;
                ChangeColor();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, currentColor);
        }

        public void InvertVelocity(bool x)
        {
            if (x)
            {
                velocity.X *= -1;
            }
            else
            {
                velocity.Y *= -1;
            }
        }

        public void ResetBall()
        {
            drawRectangle.X = windowWidth / 2 - sprite.Width / 2;
            drawRectangle.Y = RandomY();
            drawRectangle.Width = sprite.Width;
            drawRectangle.Height = sprite.Height;
        }
        
        public void SetRandomYSpeed()
        {
            velocity.Y = RandomSpeed();
        }

        private int RandomSpeed()
        {
            int positiveNegative = rand.Next(2);
            if (positiveNegative == 0)
            {
                return speed;
            }
            else
            {
                return -speed;
            }
        }

        private int RandomY()
        {
            return rand.Next(50, windowHeight - 50);
        }

        private void ChangeColor()
        {
            int index = rand.Next(colors.Count);
            while (index == currentColorIndex)
            {
                index = rand.Next(colors.Count);
            }
            currentColorIndex = index;

            currentColor = colors[index];
        }

        # endregion
    }
}
