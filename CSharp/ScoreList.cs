using System;
using System.Collections.Generic;
using System.Xml.Serialization;

/* La classe ScoreList gère une liste de scores, avec la possibilité d'ajouter un score
   tout en maintenant une limite de 5 scores. 
   Elle utilise la sérialisation XML pour stocker et récupérer les scores. */

namespace SnakeMonoGame.CSharp {

    [Serializable]
    public class ScoreList {

        [XmlArray("Scores")]
        [XmlArrayItem("Score")]
        public List<Score> Scores { get; set; }

        /* Constructeur de la classe ScoreList
           Initialise la liste des scores comme une nouvelle liste vide */
        public ScoreList() { 
            Scores = new List<Score>(); 
        }

        // ajouter un score à la liste, en maintenant la limite de 5 scores
        public void AddScore(Score score) {
            Scores.Add(score);
            if (Scores.Count > 5) {
                Scores.RemoveAt(0); 
            }
        }
        
    }
    
}