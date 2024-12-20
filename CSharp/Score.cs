using System;

namespace SnakeMonoGame.CSharp;

/*  La classe Score gère le score du joueur et la date à laquelle il a été enregistré. 
    Elle permet d'augmenter le score et d'obtenir la valeur actuelle du score,
    ainsi que la date de son enregistrement. */

public class Score {
    
    public int CurrentScore { get; set; } // Le score actuel du joueur
    public DateTime Date { get; set; } // La date à laquelle le score a été enregistré

    public Score() { // Constructeur de la classe Score
        CurrentScore = 0;
        Date = DateTime.Now;
    }

    public void Increase() => CurrentScore += 1; // Méthode pour augmenter le score de 1
    
    public int GetScore() => CurrentScore; // Méthode pour obtenir le score actuel
    
}