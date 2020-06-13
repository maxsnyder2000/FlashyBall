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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // declare window width and height
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        // current game state
        static GameState currentGameState = GameState.Start;

		// theme
        SoundEffectInstance theme;
		
        // random number generator
        Random rand = new Random();
		
        // button sprites
        Texture2D playButtonSprite;
        Texture2D creditsButtonSprite;
        Texture2D quitButtonSprite;
        Texture2D backButtonSprite;
        Texture2D easyButtonSprite;
        Texture2D mediumButtonSprite;
        Texture2D hardButtonSprite;
        Texture2D impossibleButtonSprite;

        // button colors
        Color buttonHoverColor = Color.LightBlue;
        Color buttonPressColor = Color.Blue;

        // start screen
        Texture2D startScreen;
        Rectangle startDrawRectangle;
        const int TOTAL_START_MILLISECONDS = 1000;
        int elapsedStartMilliseconds = 0;

        // menu screen game logos
        Texture2D currentLogo;
        Rectangle logoDrawRectangle;
        List<Texture2D> logos = new List<Texture2D>();
        int currentLogoIndex = -1;
        const int TOTAL_LOGO_MILLISECONDS = 250;
        int elapsedLogoMilliseconds = 0;

        // menu screen buttons
        Button menuToOptionsButton;
        Button menuToCreditsButton;
        Button menuToQuitButton;
        const int HORIZONTAL_MENU_BUTTON_CENTER = WINDOW_WIDTH * 3 / 4;
        const int VERTICAL_MENU_BUTTON_SPACING = WINDOW_HEIGHT / 4;
        
        // options screen rules
        Texture2D rules;
        Rectangle rulesDrawRectangle;
        
        // options screen buttons
        Button optionsToMenuButton;
        const int HORIZONTAL_OPTIONS_BUTTON_CENTER = WINDOW_WIDTH / 2;
        const int VERTICAL_OPTIONS_BUTTON_CENTER = WINDOW_HEIGHT * 9 / 10;

        // options screen difficulty buttons
        Button easyButton;
        Button mediumButton;
        Button hardButton;
        Button impossibleButton;
        const int HORIZONTAL_DIFFICULTY_BUTTON_SPACING = WINDOW_WIDTH / 5;
        const int VERTICAL_DIFFICULTY_BUTTON_CENTER = WINDOW_HEIGHT * 3 / 4;

        // difficulty variables
        int ballColorChangeMilliseconds;
        const int RANDOM_MILLISECONDS_MARGIN = 500;
        const int EASY_MILLISECONDS = 3000;
        const int MEDIUM_MILLISECONDS = 2000;
        const int HARD_MILLISECONDS = 2000;
        const int IMPOSSIBLE_MILLISECONDS = 1000;
        List<Color> gameColors = new List<Color>();
        Color color0 = Color.Red;
        Color color1 = Color.Blue;
        Color color2 = Color.Yellow;
        Color color3 = Color.Green;

        // wait between options and play
        const int TOTAL_PLAY_WAIT_MILLISECONDS = 1000;
        int elapsedPlayWaitMilliseconds;

        // paddles
        Paddle player1Paddle;
        Paddle player2Paddle;
        Texture2D paddleSprite;
        const int HORIZONTAL_PADDLE_BORDER = WINDOW_WIDTH / 20;
        const int PADDLE_VERTICAL_SPEED = 20;
        const int TOTAL_BALL_PASSED_MILLISECONDS = 1000;
        const int TOTAL_PADDLE_COLOR_CHANGE_MILLISECONDS = 250;

        // player 1 keys
        List<Keys> player1ColorKeys = new List<Keys>();
        Keys player1Color0 = Keys.D1;
        Keys player1Color1 = Keys.D2;
        Keys player1Color2 = Keys.D3;
        Keys player1Color3 = Keys.D4;
        Keys player1UpKey = Keys.W;
        Keys player1DownKey = Keys.S;
        
        // player 2 keys
        List<Keys> player2ColorKeys = new List<Keys>();
        Keys player2Color0 = Keys.D7;
        Keys player2Color1 = Keys.D8;
        Keys player2Color2 = Keys.D9;
        Keys player2Color3 = Keys.D0;
        Keys player2UpKey = Keys.Up;
        Keys player2DownKey = Keys.Down;

        // ball
        Ball ball;
        Texture2D ballSprite;
        const int BALL_SPEED = 5;

        // dividing line
        Texture2D dividingLineSprite;
        Rectangle dividingLineDrawRectangle;

        // play screen scores
        SpriteFont scoreFont;
        Color scoreColor = Color.Black;
        Score player1Score;
        Score player2Score;
        const int HORIZONTAL_SCORE_1_LOCATION = WINDOW_WIDTH / 25;
        const int HORIZONTAL_SCORE_2_LOCATION = WINDOW_WIDTH * 4 / 5;
        const int VERTICAL_SCORE_LOCATION = WINDOW_HEIGHT / 25;
        const int WIN_SCORE = 10;

        // win screen message
        Texture2D player1WinMessage;
        Texture2D player2WinMessage;
        Rectangle winMessageDrawRectangle;

        // win screen buttons
        Button winToMenuButton;
        Button winToQuitButton;
        const int HORIZONTAL_WIN_BUTTON_SPACING = WINDOW_WIDTH / 3;
        const int VERTICAL_WIN_BUTTON_CENTER = WINDOW_HEIGHT * 7 / 8;

        // credits screen message
        Texture2D credits;
        Rectangle creditsDrawRectangle;

        // credits screen buttons
        Button creditsToMenuButton;
        const int HORIZONTAL_CREDITS_BUTTON_CENTER = WINDOW_WIDTH / 2;
        const int VERTICAL_CREDITS_BUTTON_CENTER = WINDOW_HEIGHT * 7 / 8;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // set window width and height
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            // make mouse visible
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // play theme
            SoundEffect soundEffect = Content.Load<SoundEffect>(@"Audio\theme");
            theme = soundEffect.CreateInstance();
            theme.IsLooped = true;
            theme.Play();
            
            // load button sprites
            playButtonSprite = Content.Load<Texture2D>(@"Buttons\playbutton");
            creditsButtonSprite = Content.Load<Texture2D>(@"Buttons\creditsbutton");
            quitButtonSprite = Content.Load<Texture2D>(@"Buttons\quitbutton");
            backButtonSprite = Content.Load<Texture2D>(@"Buttons\backbutton");
            easyButtonSprite = Content.Load<Texture2D>(@"Buttons\easybutton");
            mediumButtonSprite = Content.Load<Texture2D>(@"Buttons\mediumbutton");
            hardButtonSprite = Content.Load<Texture2D>(@"Buttons\hardbutton");
            impossibleButtonSprite = Content.Load<Texture2D>(@"Buttons\impossiblebutton");

            // load start screen
            startScreen = Content.Load<Texture2D>(@"Screens\xnascreen");
            startDrawRectangle = new Rectangle(0, 0, startScreen.Width, startScreen.Height);
            
            // load menu screen logos
            logos.Add(Content.Load<Texture2D>(@"Logos\logored"));
            logos.Add(Content.Load<Texture2D>(@"Logos\logoyellow"));
            logos.Add(Content.Load<Texture2D>(@"Logos\logogreen"));
            logos.Add(Content.Load<Texture2D>(@"Logos\logoblue"));
            RandomLogo();
            logoDrawRectangle = new Rectangle(0, 0, currentLogo.Width, currentLogo.Height);

            // load menu screen buttons
            menuToOptionsButton = new Button(playButtonSprite, new Vector2(HORIZONTAL_MENU_BUTTON_CENTER, VERTICAL_MENU_BUTTON_SPACING), buttonHoverColor, buttonPressColor, GameState.Options);
            menuToCreditsButton = new Button(creditsButtonSprite, new Vector2(HORIZONTAL_MENU_BUTTON_CENTER, VERTICAL_MENU_BUTTON_SPACING * 2), buttonHoverColor, buttonPressColor, GameState.Credits);
            menuToQuitButton = new Button(quitButtonSprite, new Vector2(HORIZONTAL_MENU_BUTTON_CENTER, VERTICAL_MENU_BUTTON_SPACING * 3), buttonHoverColor, buttonPressColor, GameState.Quit);
            
            // load options screen rules
            rules = Content.Load<Texture2D>(@"Screens\rulesscreen");
            rulesDrawRectangle = new Rectangle(0, 0, rules.Width, rules.Height);

            // load options screen buttons
            optionsToMenuButton = new Button(backButtonSprite, new Vector2(HORIZONTAL_OPTIONS_BUTTON_CENTER, VERTICAL_OPTIONS_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Menu);
            
            // load options screen difficulty buttons
            easyButton = new Button(easyButtonSprite, new Vector2(HORIZONTAL_DIFFICULTY_BUTTON_SPACING, VERTICAL_DIFFICULTY_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Easy);
            mediumButton = new Button(mediumButtonSprite, new Vector2(HORIZONTAL_DIFFICULTY_BUTTON_SPACING * 2, VERTICAL_DIFFICULTY_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Medium);
            hardButton = new Button(hardButtonSprite, new Vector2(HORIZONTAL_DIFFICULTY_BUTTON_SPACING * 3, VERTICAL_DIFFICULTY_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Hard);
            impossibleButton = new Button(impossibleButtonSprite, new Vector2(HORIZONTAL_DIFFICULTY_BUTTON_SPACING * 4, VERTICAL_DIFFICULTY_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Impossible);
            
            // load play screen object sprites
            paddleSprite = Content.Load<Texture2D>(@"Objects\paddlesprite");
            ballSprite = Content.Load<Texture2D>(@"Objects\ballsprite");
            dividingLineSprite = Content.Load<Texture2D>(@"Objects\dividinglinesprite");
            dividingLineDrawRectangle = new Rectangle(WINDOW_WIDTH / 2 - dividingLineSprite.Width / 2, WINDOW_HEIGHT / 2 - dividingLineSprite.Height / 2, dividingLineSprite.Width, dividingLineSprite.Height);
            
            // load play screen scores
            scoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");
            player1Score = new Score(scoreFont, new Vector2(HORIZONTAL_SCORE_1_LOCATION, VERTICAL_SCORE_LOCATION), scoreColor);
            player2Score = new Score(scoreFont, new Vector2(HORIZONTAL_SCORE_2_LOCATION, VERTICAL_SCORE_LOCATION), scoreColor);

            // load win screen messages
            player1WinMessage = Content.Load<Texture2D>(@"Winners\player1win");
            player2WinMessage = Content.Load<Texture2D>(@"Winners\player2win");
            winMessageDrawRectangle = new Rectangle(0, 0, player1WinMessage.Width, player1WinMessage.Height);

            // load win screen buttons
            winToMenuButton = new Button(backButtonSprite, new Vector2(HORIZONTAL_WIN_BUTTON_SPACING, VERTICAL_WIN_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Menu);
            winToQuitButton = new Button(quitButtonSprite, new Vector2(HORIZONTAL_WIN_BUTTON_SPACING * 2, VERTICAL_WIN_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Quit);

            // load credits screen credits
            credits = Content.Load<Texture2D>(@"Screens\creditsscreen");
            creditsDrawRectangle = new Rectangle(0, 0, credits.Width, credits.Height);

            // load credits screen buttons
            creditsToMenuButton = new Button(backButtonSprite, new Vector2(HORIZONTAL_CREDITS_BUTTON_CENTER, VERTICAL_CREDITS_BUTTON_CENTER), buttonHoverColor, buttonPressColor, GameState.Menu);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // get mouse and keyboard state
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
                        
            switch (currentGameState)
            {
                case GameState.Start:

                    // go to menu screen after a short time
                    elapsedStartMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedStartMilliseconds > TOTAL_START_MILLISECONDS)
                    {
                        elapsedStartMilliseconds = 0;
                        currentGameState = GameState.Menu;
                    }

                    break;

                case GameState.Menu:

                    // update logo
                    elapsedLogoMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedLogoMilliseconds > TOTAL_LOGO_MILLISECONDS)
                    {
                        elapsedLogoMilliseconds = 0;
                        RandomLogo();
                    }

                    // update buttons
                    menuToOptionsButton.Update(mouse);
                    menuToCreditsButton.Update(mouse);
                    menuToQuitButton.Update(mouse);

                    // reset information in case of new game
                    elapsedPlayWaitMilliseconds = 0;
                    player1Score.ResetScore();
                    player2Score.ResetScore();
                    gameColors.Clear();

                    break;
                
                case GameState.Options:

                    // update buttons
                    optionsToMenuButton.Update(mouse);

                    // update difficulty buttons
                    easyButton.Update(mouse);
                    mediumButton.Update(mouse);
                    hardButton.Update(mouse);
                    impossibleButton.Update(mouse);

                    break;
                
                case GameState.Easy:

                    // set variables
                    ballColorChangeMilliseconds = rand.Next(EASY_MILLISECONDS - RANDOM_MILLISECONDS_MARGIN, EASY_MILLISECONDS + RANDOM_MILLISECONDS_MARGIN);
                    gameColors.Add(color0);
                    gameColors.Add(color1);

                    // create play objects
                    CreatePlayObjects();

                    // enter play mode
                    currentGameState = GameState.Play;

                    break;

                case GameState.Medium:

                    // set variables
                    ballColorChangeMilliseconds = rand.Next(MEDIUM_MILLISECONDS - RANDOM_MILLISECONDS_MARGIN, MEDIUM_MILLISECONDS + RANDOM_MILLISECONDS_MARGIN);
                    gameColors.Add(color0);
                    gameColors.Add(color1);
                    gameColors.Add(color2);

                    // create play objects
                    CreatePlayObjects();

                    // enter play mode
                    currentGameState = GameState.Play;

                    break;

                case GameState.Hard:

                    // set variables
                    ballColorChangeMilliseconds = rand.Next(HARD_MILLISECONDS - RANDOM_MILLISECONDS_MARGIN, HARD_MILLISECONDS + RANDOM_MILLISECONDS_MARGIN);
                    gameColors.Add(color0);
                    gameColors.Add(color1);
                    gameColors.Add(color2);
                    gameColors.Add(color3);

                    // create play objects
                    CreatePlayObjects();

                    // enter play mode
                    currentGameState = GameState.Play;

                    break;

                case GameState.Impossible:

                    // set variables
                    ballColorChangeMilliseconds = rand.Next(IMPOSSIBLE_MILLISECONDS - RANDOM_MILLISECONDS_MARGIN, IMPOSSIBLE_MILLISECONDS + RANDOM_MILLISECONDS_MARGIN);
                    gameColors.Add(color0);
                    gameColors.Add(color1);
                    gameColors.Add(color2);
                    gameColors.Add(color3);

                    // create play objects
                    CreatePlayObjects();

                    // enter play mode
                    currentGameState = GameState.Play;

                    break;

                case GameState.Play:

                    // wait before play starts
                    if (elapsedPlayWaitMilliseconds < TOTAL_PLAY_WAIT_MILLISECONDS)
                    {
                        elapsedPlayWaitMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else
                    {
                        // update play objects
                        player1Paddle.Update(keyboard, ball, gameTime, player2Score);
                        player2Paddle.Update(keyboard, ball, gameTime, player1Score);
                        ball.Update(gameTime);

                        // go to win screen if player wins
                        if (player1Score.CurrentScore >= WIN_SCORE || player2Score.CurrentScore >= WIN_SCORE)
                        {
                            currentGameState = GameState.Win;
                        }
                    }
                                       
                    break;
                
                case GameState.Win:
                    
                    // update buttons
                    winToMenuButton.Update(mouse);
                    winToQuitButton.Update(mouse);

                    break;

                case GameState.Credits:

                    // update buttons
                    creditsToMenuButton.Update(mouse);

                    break;

                case GameState.Quit:

                    // exit the game
                    this.Exit();

                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.Start:

                    // draw start screen
                    spriteBatch.Draw(startScreen, startDrawRectangle, Color.White);

                    break;

                case GameState.Menu:

                    // draw logo
                    spriteBatch.Draw(currentLogo, logoDrawRectangle, Color.White);

                    // draw buttons
                    menuToOptionsButton.Draw(spriteBatch);
                    menuToCreditsButton.Draw(spriteBatch);
                    menuToQuitButton.Draw(spriteBatch);

                    break;

                case GameState.Options:

                    // draw rules
                    spriteBatch.Draw(rules, rulesDrawRectangle, Color.White);

                    // draw buttons
                    optionsToMenuButton.Draw(spriteBatch);

                    // draw difficulty buttons
                    easyButton.Draw(spriteBatch);
                    mediumButton.Draw(spriteBatch);
                    hardButton.Draw(spriteBatch);
                    impossibleButton.Draw(spriteBatch);

                    break;

                case GameState.Play:

                    // draw dividing line (first, so it's under everything)
                    spriteBatch.Draw(dividingLineSprite, dividingLineDrawRectangle, Color.White);

                    // draw play objects
                    player1Paddle.Draw(spriteBatch);
                    player2Paddle.Draw(spriteBatch);
                    
                    // only draw ball if play wait finished
                    if (elapsedPlayWaitMilliseconds > TOTAL_PLAY_WAIT_MILLISECONDS)
                    {
                        ball.Draw(spriteBatch);
                    }

                    // draw scores
                    player1Score.Draw(spriteBatch);
                    player2Score.Draw(spriteBatch);

                    break;
                
                case GameState.Win:

                    // draw proper win message (first, so the scores are displayed over it)
                    if (player1Score.CurrentScore >= WIN_SCORE)
                    {
                        // player 1 win
                        spriteBatch.Draw(player1WinMessage, winMessageDrawRectangle, Color.White);
                    }
                    else
                    {
                        // player 2 win
                        spriteBatch.Draw(player2WinMessage, winMessageDrawRectangle, Color.White);
                    }

                    // draw buttons
                    winToMenuButton.Draw(spriteBatch);
                    winToQuitButton.Draw(spriteBatch);

                    // draw scores
                    player1Score.Draw(spriteBatch);
                    player2Score.Draw(spriteBatch);

                    break;

                case GameState.Credits:

                    // draw credits
                    spriteBatch.Draw(credits, creditsDrawRectangle, Color.White);

                    // draw buttons
                    creditsToMenuButton.Draw(spriteBatch);

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void ChangeState(GameState newGameState)
        {
            currentGameState = newGameState;
        }

        private void RandomLogo()
        {
            int index = rand.Next(logos.Count);
            while (index == currentLogoIndex)
            {
                index = rand.Next(logos.Count);
            }
            currentLogoIndex = index;

            currentLogo = logos[index];
        }

        private void CreatePlayObjects()
        {
            player1ColorKeys.Add(player1Color0);
            player1ColorKeys.Add(player1Color1);
            player1ColorKeys.Add(player1Color2);
            player1ColorKeys.Add(player1Color3);
            player1Paddle = new Paddle(paddleSprite, new Vector2(HORIZONTAL_PADDLE_BORDER, WINDOW_HEIGHT / 2), WINDOW_WIDTH, WINDOW_HEIGHT, PADDLE_VERTICAL_SPEED, gameColors, player1ColorKeys, player1UpKey, player1DownKey, true, TOTAL_BALL_PASSED_MILLISECONDS, TOTAL_PADDLE_COLOR_CHANGE_MILLISECONDS);

            player2ColorKeys.Add(player2Color0);
            player2ColorKeys.Add(player2Color1);
            player2ColorKeys.Add(player2Color2);
            player2ColorKeys.Add(player2Color3);
            player2Paddle = new Paddle(paddleSprite, new Vector2(WINDOW_WIDTH - HORIZONTAL_PADDLE_BORDER, WINDOW_HEIGHT / 2), WINDOW_WIDTH, WINDOW_HEIGHT, PADDLE_VERTICAL_SPEED, gameColors, player2ColorKeys, player2UpKey, player2DownKey, false, TOTAL_BALL_PASSED_MILLISECONDS, TOTAL_PADDLE_COLOR_CHANGE_MILLISECONDS);

            ball = new Ball(ballSprite, WINDOW_WIDTH, WINDOW_HEIGHT, BALL_SPEED, gameColors, ballColorChangeMilliseconds);
        }
    }
}
