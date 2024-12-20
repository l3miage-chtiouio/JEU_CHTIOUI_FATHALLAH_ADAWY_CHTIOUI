using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeMonoGame.CSharp;

/* La classe Sprite représente un objet graphique dans le jeu, avec une texture, une position,
et une couleur. Elle permet de dessiner l'objet à l'écran, de le déplacer, 
et de gérer les collisions avec d'autres objets. */
public class Sprite {
    
    protected Texture2D _texture;
    protected Vector2 _position;
    protected Color _color = Color.White;
    protected int _gridSize = 30; 

    /*  Le constructeur Sprite initialise un objet Sprite avec une texture 
    et une position données. */
    public Sprite(Texture2D texture, Vector2 position) {
        _texture = texture;
        _position = position;
    }

    public Rectangle _Rect => new Rectangle((int)_position.X, (int)_position.Y, _gridSize, _gridSize);

    /* La méthode Draw dessine l'objet à l'écran en utilisant un SpriteBatch. */
    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(_texture, _Rect, _color); 
    }

    /* La méthode Intersects vérifie si l'objet graphique entre en collision avec un autre rectangle.
    Retourne true si l'objet entre en collision avec le rectangle spécifié, sinon false. */
    public bool Intersects(Rectangle other) => _Rect.Intersects(other);

    /* La méthode Move déplace l'objet dans une direction donnée.
    Paramètres :
    - direction : La direction dans laquelle déplacer l'objet.
    - stepSize : Le nombre de pixels à déplacer l'objet à chaque appel. */
    public void Move(Direction direction, int stepSize) {
        switch (direction) {
            case Direction.UP:
                _position.Y -= stepSize;
                break;
            case Direction.DOWN:
                _position.Y += stepSize;
                break;
            case Direction.LEFT:
                _position.X -= stepSize;
                break;
            case Direction.RIGHT:
                _position.X += stepSize;
                break;
        }
    }

    /* La méthode PlaceRandomly place l'objet à une position aléatoire sur l'écran, 
    en s'assurant qu'il ne chevauche pas d'autres objets.
    Paramètres: - maxWidth : La largeur maximale de l'écran (en pixels).
    - maxHeight : La hauteur maximale de l'écran (en pixels).
    - existingObjects : La liste des objets déjà présents sur l'écran pour vérifier les collisions. */
    public void PlaceRandomly(int maxWidth, int maxHeight, List<Sprite> existingObjects) {
        var random = new Random();
        do { 
            _position = new Vector2(
                random.Next(0, maxWidth / _gridSize) * _gridSize,
                random.Next(0, maxHeight / _gridSize) * _gridSize
            );
        } while (existingObjects.Exists(obj => obj._Rect.Intersects(_Rect)));
    }
    
}