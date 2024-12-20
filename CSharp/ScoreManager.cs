using System;
using System.IO;
using System.Xml.Serialization;

/*  La classe ScoreManager est responsable de la gestion des scores, 
    notamment de la sauvegarde et du chargement des scores dans un fichier XML. 
    Elle fournit des méthodes statiques permettant d'enregistrer les scores et 
    de les récupérer à partir d'un fichier scores.xml situé dans ../xml/. . */

namespace SnakeMonoGame.CSharp {

    public static class ScoreManager {

        // Chemin du fichier XML où les scores sont sauvegardés
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "xml", "scores.xml"); 
        
        /*  Méthode : SaveScores
            Description : Sauvegarde la liste des scores dans un fichier XML.
            Paramètre : scoreList : La liste des scores à sauvegarder dans le fichier XML.
            Comportement :
                - Vérifie si le dossier où le fichier XML sera enregistré existe. 
                Si ce n'est pas le cas, il est créé.
                - Sérialise la liste des scores et la sauvegarde dans un fichier XML 
                  à l'emplacement spécifié par filePath. */
        public static void SaveScores(ScoreList scoreList) {
            string fullPath = Path.GetFullPath(filePath);

            // verifier si le dossier existe dans le répertoire racine du projet
            string directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath); 
            }
            
            // serialisation et sauvegarde des scores dans le fichier XML
            XmlSerializer serializer = new XmlSerializer(typeof(ScoreList));
            using (StreamWriter writer = new StreamWriter(fullPath)) {
                serializer.Serialize(writer, scoreList);
            }
        }
        
        /*  Méthode : LoadScores
            Description : Charge la liste des scores à partir d'un fichier XML.
            Retourne : 
                - Une instance de ScoreList contenant les scores chargés à partir du fichier XML.
                - Si le fichier n'existe pas, retourne une nouvelle liste vide.
            Comportement :
                - Vérifie si le fichier XML existe. Si c'est le cas, il désérialise le contenu du fichier 
                et le retourne sous forme de ScoreList.
                - Si le fichier n'existe pas, une nouvelle instance vide de ScoreList est retournée. */
        public static ScoreList LoadScores() {
            string fullPath = Path.GetFullPath(filePath);

            if (File.Exists(fullPath)) {
                XmlSerializer serializer = new XmlSerializer(typeof(ScoreList));
                using (StreamReader reader = new StreamReader(fullPath)) {
                    return (ScoreList)serializer.Deserialize(reader);
                }
            }
            
            return new ScoreList(); // si le fichier n'existe pas, on retourne une nouvelle liste vide
        }
    }
    
}
