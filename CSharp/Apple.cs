/* Représente une pomme dans le jeu, utilisée pour nourrir le serpent. */
using System;
using Microsoft.Xna.Framework;

namespace SnakeMonoGame.CSharp;

public class Apple {
    
    private Vector2 position;

    /* La méthode Create crée une nouvelle pomme à une position aléatoire qui ne se superpose pas 
    avec le serpent ni les murs. */
    public void Create(Snake snake) {
        var random = new Random();
        do {
            position = new Vector2(
                random.Next(2, (900 / 30) - 2) * 30,  // limiter la position X à la grille
                random.Next(2, (900 / 30) - 2) * 30   // limiter la position Y à la grille
            );
        } while (snake.GetBodyParts().Exists(part => part.XPosition == position.X && part.YPosition == position.Y) 
                 || position.X == 0 || position.X == 900 - 30 || position.Y == 0 || position.Y == 900 - 30);  
                    // verifie que la pomme ne touche pas murs
    }

    /* Cette méthode Obtient la position de la pomme.
    Elle renvoie un vecteur représentant la position de la pomme. */
    public Vector2 GetPosition() => position;

    /* Cette méthode obtient la position sous forme de rectangle de la pomme pour la gestion
     de l'affichage. */
    public Rectangle GetSpritePosition() => new Rectangle((int)position.X, (int)position.Y, 30, 30);

    /* La méthode WasEated vérifie si la tête du serpent a mangé la pomme. */
    public bool WasEated(Rectangle snakeHead) => snakeHead.Intersects(GetSpritePosition());
}