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
    public class Paddle
    {
        # region Fields

        // sprite and draw rectangle
        Texture2D sprite;
        Rectangle drawRectangle;

        // window width and height
        int windowWidth;
        int windowHeight;

        // speed
        int verticalSpeed;

        // colors
        List<Color> colors;
        Color currentColor = Color.White;

        // keys
        List<Keys> colorKeys;
        Keys upKey;
        Keys downKey;

        // paddle side (left if true, right if false)
        bool leftPaddle;

        // ball passed paddle
        int totalBallPassedMilliseconds;
        int elapsedBallPassedMilliseconds = 0;
        bool ballPassed = false;
        
        // wait between color change
        int totalColorChangeMilliseconds;
        int elapsedColorChangeMilliseconds = 0;

        # endregion

        # region Constructors

        public Paddle(Texture2D sprite, Vector2 center, int windowWidth, int windowHeight, int verticalSpeed, List<Color> colors, List<Keys> colorKeys, Keys upKey, Keys downKey, bool leftPaddle, int totalBallPassedMilliseconds, int totalColorChangeMilliseconds)
        {
            this.sprite = sprite;
            this.drawRectangle = new Rectangle((int)(center.X - sprite.Width / 2), (int)(center.Y - sprite.Height / 2), sprite.Width, sprite.Height);

            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            this.verticalSpeed = verticalSpeed;

            this.colors = colors;

            this.colorKeys = colorKeys;
            this.upKey = upKey;
            this.downKey = downKey;

            this.leftPaddle = leftPaddle;

            this.totalBallPassedMilliseconds = totalBallPassedMilliseconds;

            this.totalColorChangeMilliseconds = totalColorChangeMilliseconds;
        }

        # endregion

        # region Properties

        # endregion

        # region Methods

        public void Update(KeyboardState keyboard, Ball ball, GameTime gameTime, Score otherScore)
        {
            // move paddle
            if (keyboard.IsKeyDown(upKey))
            {
                drawRectangle.Y -= verticalSpeed;
            }
            else if (keyboard.IsKeyDown(downKey))
            {
                drawRectangle.Y += verticalSpeed;
            }

            // keep paddle in window
            if (drawRectangle.Y < 0)
            {
                drawRectangle.Y = 0;
            }
            else if (drawRectangle.Y > windowHeight - drawRectangle.Height)
            {
                drawRectangle.Y = windowHeight - drawRectangle.Height;
            }

            // change color when key pressed (if time passed)
            elapsedColorChangeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedColorChangeMilliseconds > totalColorChangeMilliseconds)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    if (keyboard.IsKeyDown(colorKeys[i]))
                    {
                        currentColor = colors[i];
                        elapsedColorChangeMilliseconds = 0;
                    }
                }
            }
            
            if (!ballPassed)
            {
                // bounce ball off paddle if correct color
                if ((leftPaddle && ball.CollisionRectangle.X < drawRectangle.X + drawRectangle.Width) ||
                    !leftPaddle && ball.CollisionRectangle.X > drawRectangle.X - ball.CollisionRectangle.Width)
                {
                    if (currentColor == ball.Color && ball.CollisionRectangle.Y > drawRectangle.Y - ball.CollisionRectangle.Height && ball.CollisionRectangle.Y < drawRectangle.Y + drawRectangle.Height)
                    {
                        ball.InvertVelocity(true);
                    }
                    else
                    {
                        ballPassed = true;

                        // increment other player's score
                        otherScore.IncrementScore();
                    }
                }
            }
            else
            {
                // reset ball after time
                elapsedBallPassedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedBallPassedMilliseconds > totalBallPassedMilliseconds)
                {
                    elapsedBallPassedMilliseconds = 0;
                    ballPassed = false;
                    ball.ResetBall();

                    // force ball to opposite side
                    ball.InvertVelocity(true);
                    ball.SetRandomYSpeed();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, currentColor);
        }

        # endregion
    }
}
