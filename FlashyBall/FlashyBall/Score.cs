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
    public class Score
    {
        # region Fields

        // font, text, and location
        SpriteFont font;
        string text;
        Vector2 location;

        // score
        string scoreText = "Score: ";
        int score = 0;

        // color
        Color color;

        # endregion

        # region Constructors

        public Score(SpriteFont font, Vector2 location, Color color)
        {
            this.font = font;
            UpdateText();
            this.location = location;

            this.color = color;
        }

        # endregion

        # region Properties

        public int CurrentScore
        {
            get { return score; }
        }

        # endregion

        # region Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, location, color);
        }

        public void IncrementScore()
        {
            score += 1;
            UpdateText();
        }

        public void ResetScore()
        {
            score = 0;
            UpdateText();
        }

        private void UpdateText()
        {
            text = scoreText + score;
        }

        # endregion
    }
}
