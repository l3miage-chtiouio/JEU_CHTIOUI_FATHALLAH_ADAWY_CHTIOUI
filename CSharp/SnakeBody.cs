namespace SnakeMonoGame.CSharp;

/* La classe SnakeBody représente une partie du corps du serpent dans le jeu.
Chaque objet SnakeBody contient les informations de position (actuelle et précédente) 
d'un segment du serpent. */
public class SnakeBody {
    
    public SnakeBody(int x, int y) {
        XPosition = x;
        YPosition = y;
    }

    public int XPosition { get; set; }
    
    public int YPosition { get; set; }
    
    public int LastXPosition { get; set; }
    
    public int LastYPosition { get; set; }
    
}