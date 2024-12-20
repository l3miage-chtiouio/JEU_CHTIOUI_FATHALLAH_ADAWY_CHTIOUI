using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SnakeMonoGame.CSharp;

/*  La classe Snake représente le serpent dans le jeu. Elle gère la position, 
    la direction, la croissance et les collisions du serpent. 
    Elle contient les informations sur la longueur du corps du serpent, 
    sa direction actuelle et une liste de segments représentant 
    les différentes parties du corps du serpent. */
public class Snake {
    
    private int bodyLength;
    private List<SnakeBody> bodyParts;
    private Direction direction;

    /* Constructeur : Snake
        Description : Initialise un serpent avec une longueur de corps par défaut 
        et une direction initiale vers la droite. */
    public Snake() {
        bodyLength = 30;
        bodyParts = new List<SnakeBody>();
        direction = Direction.RIGHT;
    }

    /* Méthode : Create
    Description : Crée le serpent en ajoutant sa tête et ses trois premiers segments de corps.
    Comportement : 
        - Ajoute la tête du serpent à la position (300, 300).
        - Ajoute trois segments supplémentaires à gauche de la tête. */
    public void Create() {
        bodyParts.Add(new SnakeBody(300, 300));
        for (int i = 1; i < 4; i++) {
            bodyParts.Add(new SnakeBody(bodyParts[0].XPosition - (bodyLength * i), bodyParts[0].YPosition));
        }
    }

    /*  La méthode : GetBodyParts retourne la liste (liste des objets SnakeBody) 
    des parties du corps du serpent. */
    public List<SnakeBody> GetBodyParts() => bodyParts;

    /* Méthode : Move
    Description : Déplace le serpent dans la direction actuelle et met à jour les positions
    des segments de son corps.
    Comportement : 
        - Déplace la tête du serpent dans la direction spécifiée.
        - Déplace les autres segments du corps en suivant la tête, en gardant la position 
        de chaque segment du corps avant le mouvement.
    */
    public void Move() {
        bodyParts[0].LastXPosition = bodyParts[0].XPosition;
        bodyParts[0].LastYPosition = bodyParts[0].YPosition;

        switch (direction) {
            case Direction.UP:
                bodyParts[0].YPosition -= bodyLength;
                break;
            case Direction.DOWN:
                bodyParts[0].YPosition += bodyLength;
                break;
            case Direction.LEFT:
                bodyParts[0].XPosition -= bodyLength;
                break;
            case Direction.RIGHT:
                bodyParts[0].XPosition += bodyLength;
                break;
        }
        
        // Déplacer les autres parties du corps
        for (int i = 1; i < bodyParts.Count; i++) {
            bodyParts[i].LastXPosition = bodyParts[i].XPosition;
            bodyParts[i].LastYPosition = bodyParts[i].YPosition;
            bodyParts[i].XPosition = bodyParts[i - 1].LastXPosition;
            bodyParts[i].YPosition = bodyParts[i - 1].LastYPosition;
        }
    }

    /* Méthode : SetDirection
    Description : Modifie la direction du serpent. */
    public void SetDirection(Direction direction) => this.direction = direction;

    /* Méthode : GetDirection
    Description : Retourne la direction actuelle du serpent. */
    public Direction GetDirection() => direction;

    /* Méthode : GetHead
    Description : Retourne un rectangle représentant la tête du serpent. */
    public Rectangle GetHead() => new Rectangle(bodyParts[0].XPosition, bodyParts[0].YPosition, bodyLength, bodyLength);

    /* Méthode : Grow
    Description : Fait grandir le serpent en ajoutant un nouveau segment à la fin de son corps.
    Comportement : Ajoute un segment supplémentaire à la fin du corps du serpent en prenant 
    la position du dernier segment. */
    public void Grow() {
        var tail = bodyParts.Last();
        bodyParts.Add(new SnakeBody(tail.XPosition, tail.YPosition));
    }

    /* Méthode : Collided
    Description : Vérifie si le serpent a heurté un autre segment de son corps ou un mur.
    Comportement : 
    - Vérifie si la tête du serpent est en collision avec l'un des segments de son corps.
    - Vérifie si la tête du serpent est en collision avec les murs. */
    public bool Collided() {
        // Check for collisions with the body
        for (int i = 1; i < bodyParts.Count; i++) {
            if (bodyParts[0].XPosition == bodyParts[i].XPosition && bodyParts[0].YPosition == bodyParts[i].YPosition)
                return true;
        }

        // Check for collisions with the walls
        if (bodyParts[0].XPosition < 30 || bodyParts[0].XPosition >= 870 || bodyParts[0].YPosition < 30 || bodyParts[0].YPosition >= 870)
            return true;

        return false; // pas de collision
    }
    
}
