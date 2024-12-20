using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SnakeMonoGame.CSharp;

/* MyGame.cs est la classe principale du jeu Snake : elle gère l'initialisation, les mises à jour
   et le rendu du jeu, ainsi que les interactions avec le joueur. */
public class MyGame : Game {
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _scoreFont;
    private Score _score;
    private ScoreList _scoreList;

    private Snake snake;
    private Apple _apple;

    private Texture2D headLeft, headRight, headUp, headDown;
    private Texture2D apple;
    private Texture2D bodyVertical, bodyHorizontal, bodyTopLeft, bodyTopRight, bodyBottomLeft, bodyBottomRight;
    private Texture2D tailLeft, tailRight, tailUp, tailDown;
    private Texture2D _wallTexture;
    
    private bool isGameOver = false;
    private Texture2D gameOverImage;
    
    private Rectangle restartRect;
    private Rectangle quitRect;
    private Texture2D restartButton, quitButton;
    
    private Song gameMusic;
    
    /* Le constructeur MyGame configure les paramètres de la fenêtre du jeu,
       définit la fréquence de mise à jour et rend le curseur de la souris visible. */
    public MyGame() {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 900;
        _graphics.PreferredBackBufferHeight = 900;
        Content.RootDirectory = "Content";
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(120);
        IsMouseVisible = true;
    }

    /* La méthode Initialize() initialise les objets du jeu, y compris le serpent,
     la pomme et la gestion des scores. */
    protected override void Initialize() {
        _score = new Score();
        snake = new Snake();
        snake.Create();
        _apple = new Apple();
        _apple.Create(snake);
        _scoreList = ScoreManager.LoadScores();
        base.Initialize();
    }

    /* La méthode LoadContent() charge toutes les ressources nécessaires pour le jeu,
       y compris les textures et la musique de fond. */
    protected override void LoadContent() {
        base.LoadContent();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Chargement des textures pour le serpent, la pomme, les murs.
        headLeft = Content.Load<Texture2D>("../Content/images/head_left");
        headRight = Content.Load<Texture2D>("../Content/images/head_right");
        headUp = Content.Load<Texture2D>("../Content/images/head_up");
        headDown = Content.Load<Texture2D>("../Content/images/head_down");
        apple = Content.Load<Texture2D>("../Content/images/apple");
        bodyVertical = Content.Load<Texture2D>("../Content/images/body_vertical");
        bodyHorizontal = Content.Load<Texture2D>("../Content/images/body_horizontal");
        bodyTopLeft = Content.Load<Texture2D>("../Content/images/body_topleft");
        bodyTopRight = Content.Load<Texture2D>("../Content/images/body_topright");
        bodyBottomLeft = Content.Load<Texture2D>("../Content/images/body_bottomleft");
        bodyBottomRight = Content.Load<Texture2D>("../Content/images/body_bottomright");
        tailLeft = Content.Load<Texture2D>("../Content/images/tail_left");
        tailRight = Content.Load<Texture2D>("../Content/images/tail_right");
        tailUp = Content.Load<Texture2D>("../Content/images/tail_up");
        tailDown = Content.Load<Texture2D>("../Content/images/tail_down"); 
        _wallTexture = Content.Load<Texture2D>("../Content/images/wall");
        
        // Chargement des textures de l'écran de fin de jeu et des boutons
        gameOverImage = Content.Load<Texture2D>("../Content/images/game_over");
        restartButton = Content.Load<Texture2D>("../Content/images/restart");
        quitButton = Content.Load<Texture2D>("../Content/images/quit");
        
        // Chargement de la musique de fond
        gameMusic = Content.Load<Song>("../Content/sons/gameMusic");
        MediaPlayer.IsRepeating = true;  // La musique sera en boucle
        MediaPlayer.Play(gameMusic);
        _scoreFont = Content.Load<SpriteFont>("../Content/fonts/scoreFont");
        
    }
    
    /* La méthode UnloadContent() libère les ressources utilisées par la musique du jeu. */
    protected override void UnloadContent() {
        gameMusic.Dispose();
        base.UnloadContent();
    }
    
    /* La méthode Update() est appelée pour chaque image du jeu. Elle gère la logique du jeu,
       comme les mouvements du serpent, la détection de collision, 
       la gestion des entrées clavier et la mise à jour du score. */
    protected override void Update(GameTime gameTime) {
        if (isGameOver) {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
            
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed) {
                if (restartRect.Contains(mouseState.Position)) {
                    Initialize();
                    isGameOver = false;
                } else if (quitRect.Contains(mouseState.Position)) {
                    Exit();
                }
            }
            return;  // si le jeu est terminé, ne pas mettre à jour le serpent
        }
        
        var keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.Escape)) {
            Exit();
        }
        
        // Gestion des directions du serpent
        if (keyState.IsKeyDown(Keys.Up) && snake.GetDirection() != Direction.DOWN) snake.SetDirection(Direction.UP);
        if (keyState.IsKeyDown(Keys.Down) && snake.GetDirection() != Direction.UP) snake.SetDirection(Direction.DOWN);
        if (keyState.IsKeyDown(Keys.Left) && snake.GetDirection() != Direction.RIGHT) snake.SetDirection(Direction.LEFT);
        if (keyState.IsKeyDown(Keys.Right) && snake.GetDirection() != Direction.LEFT) snake.SetDirection(Direction.RIGHT);

        // Détection de la collision avec la pomme
        if (_apple.WasEated(snake.GetHead())) {
            _apple.Create(snake);
            snake.Grow();
            _score.Increase();
        }
        
        // Détection de la collision avec le serpent lui-même ou les murs
        if (snake.Collided()) {
            isGameOver = true;
            _scoreList.AddScore(_score); // ajouter le score actuel à la liste des scores
            ScoreManager.SaveScores(_scoreList);
        }
        
        snake.Move();
        base.Update(gameTime);
    }

    /* La méthode Draw() est appelée pour chaque image du jeu.
       Elle gère l'affichage des éléments du jeu à l'écran,
       y compris le serpent, la pomme, le score et l'écran de fin de jeu. */
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Green); 
        _spriteBatch.Begin(); 
        var snakeBody = snake.GetBodyParts(); 
        
        int gridWidth = 30 * 30;  // largeur de la grille
        int gridHeight = 30 * 30; // hauteur de la grille
        
        // dessiner les murs de la grille
        for (int x = 0; x < gridWidth; x += 30) {
            _spriteBatch.Draw(_wallTexture, new Rectangle(x, 0, 30, 30), Color.White); 
            _spriteBatch.Draw(_wallTexture, new Rectangle(x, gridHeight - 30, 30, 30), Color.White); 
        }
        for (int y = 0; y < gridHeight; y += 30) {
            _spriteBatch.Draw(_wallTexture, new Rectangle(0, y, 30, 30), Color.White); 
            _spriteBatch.Draw(_wallTexture, new Rectangle(gridWidth - 30, y, 30, 30), Color.White); 
        }
        
        // Dessiner chaque partie du serpent
        for (int i = 0; i < snakeBody.Count; i++) { 
            var part = snakeBody[i]; 
            Texture2D texture;
            if (i == 0) { // tete
                texture = snake.GetDirection() switch { 
                    Direction.UP => headUp, 
                    Direction.DOWN => headDown, 
                    Direction.LEFT => headLeft, 
                    Direction.RIGHT => headRight, 
                    _ => headRight
                };
                
            } else if (i == snakeBody.Count - 1) { // queue
                var previous = snakeBody[i - 1]; 
                if (previous.XPosition > part.XPosition) texture = tailLeft;
                else if (previous.XPosition < part.XPosition) texture = tailRight;
                else if (previous.YPosition > part.YPosition) texture = tailUp;
                else texture = tailDown; 
                
            } else { // change direction
                var previous = snakeBody[i - 1]; 
                var next = snakeBody[i + 1];
                if (previous.XPosition == next.XPosition) // Verticale
                    texture = bodyVertical;
                else if (previous.YPosition == next.YPosition) // Horizontale
                    texture = bodyHorizontal;
                else { // tourne
                    if (previous.XPosition < part.XPosition && next.YPosition < part.YPosition || 
                    next.XPosition < part.XPosition && previous.YPosition < part.YPosition) 
                        texture = bodyTopLeft;
                    else if (previous.XPosition > part.XPosition && next.YPosition < part.YPosition || 
                             next.XPosition > part.XPosition && previous.YPosition < part.YPosition) 
                        texture = bodyTopRight;
                    else if (previous.XPosition < part.XPosition && next.YPosition > part.YPosition || 
                             next.XPosition < part.XPosition && previous.YPosition > part.YPosition) 
                        texture = bodyBottomLeft;
                    else if (previous.XPosition > part.XPosition && next.YPosition > part.YPosition || 
                             next.XPosition > part.XPosition && previous.YPosition > part.YPosition) 
                        texture = bodyBottomRight;
                    else 
                        texture = bodyHorizontal; 
                }
            }
            
            _spriteBatch.Draw(texture, new Rectangle(part.XPosition, part.YPosition, 30, 30), Color.White);
        }
        
        // Affichage de la pomme (objet apple)
        _spriteBatch.Draw (
            apple,
            new Rectangle((int)_apple.GetPosition().X, (int)_apple.GetPosition().Y, 40, 40),
            Color.White );
        
        // Afficher le score
        Vector2 scorePosition = new Vector2(10, 10);
        string scoreText = "Score :" + _score.GetScore();
        Vector2 textSize = _scoreFont.MeasureString(scoreText);
        // Définir le rectangle autour du score
        Rectangle scoreRectangle = new Rectangle(
            (int)scorePosition.X - 5,  
            (int)scorePosition.Y - 5,  
            (int)textSize.X + 10,      
            (int)textSize.Y + 10       
        );
        Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.Yellow * 0.5f }); 
        _spriteBatch.Draw(rectangleTexture, scoreRectangle, Color.White);
        _spriteBatch.DrawString(_scoreFont, scoreText, scorePosition, Color.White);
        
        if (isGameOver) {
            // Calcul de la position pour centrer l'image "Game Over" à l'écran
            float gameOverX = (900 - gameOverImage.Width) / 2;
            float gameOverY = (900 - gameOverImage.Height) / 2;  
            _spriteBatch.Draw(gameOverImage, new Rectangle((int)gameOverX, (int)gameOverY, gameOverImage.Width, gameOverImage.Height), Color.White);

            // Dimensions des boutons
            int buttonWidth = 100; // Largeur commune pour les deux boutons
            int buttonHeight = 100; // Hauteur commune pour les deux boutons
            int buttonSpacing = 20;
            
            // Calcul de la largeur totale 
            int totalWidth = (buttonWidth * 2) + buttonSpacing; 
            int startX = (900 - totalWidth) / 2; 
            // Position Y pour les placer sous "Game Over"
            int buttonsY = (int)gameOverY + gameOverImage.Height + 20;
            
            // Définition des rectangles pour les boutons "Restart" et "Quit"
            restartRect = new Rectangle(startX, buttonsY, buttonWidth, buttonHeight);
            quitRect = new Rectangle(startX + buttonWidth + buttonSpacing, buttonsY, buttonWidth, buttonHeight);
            
            // Dessin des boutons
            _spriteBatch.Draw(restartButton, restartRect, Color.White);
            _spriteBatch.Draw(quitButton, quitRect, Color.White);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

}