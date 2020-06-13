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
    public class Button
    {
        # region Fields

        // sprite and draw rectangle
        Texture2D sprite;
        Rectangle drawRectangle;

        // click processing
        bool leftButtonPressed = false;

        // colors
        Color currentColor = Color.White;
        Color press;
        Color hover;

        // new game state
        GameState newGameState;
        
        # endregion

        # region Constructors

        public Button(Texture2D sprite, Vector2 center, Color hover, Color press, GameState newGameState)
        {
            this.sprite = sprite;
            this.drawRectangle = new Rectangle((int)(center.X - sprite.Width / 2), (int)(center.Y - sprite.Height / 2), sprite.Width, sprite.Height);

            this.hover = hover;
            this.press = press;

            this.newGameState = newGameState;
        }

        # endregion

        # region Properties
        
        # endregion

        # region Methods

        public bool Update(MouseState mouse)
        {
            // update color
            if (drawRectangle.Contains(mouse.X, mouse.Y))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    // press
                    leftButtonPressed = true;
                    currentColor = press;
                }
                else if (leftButtonPressed && mouse.LeftButton == ButtonState.Released)
                {
                    // release
                    leftButtonPressed = false;
                    Game1.ChangeState(newGameState);
                }
                else
                {
                    // hover
                    leftButtonPressed = false;
                    currentColor = hover;
                }
            }
            else
            {
                // normal color
                leftButtonPressed = false;
                currentColor = Color.White;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, currentColor);
        }

        # endregion
    }
}
