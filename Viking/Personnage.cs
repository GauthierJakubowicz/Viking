using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VikingGame;

namespace Viking
{
    public class Personnage
    {
        public string Nom { get; }
        public BitmapImage Portrait { get; }
        public BitmapImage SpriteSheet { get; }
        public AnimationManager AnimManager { get; }

        public Personnage(
            string nom,
            string portraitFile,       // ex: "Viking Portrait.png"
            string spriteFile,         // ex: "Viking-Sheet.png"
            Image targetImage,         // ex: imgViking dans UCJeu.xaml
            int frameWidth,            // largeur d’une frame = 33
            int frameHeight,           // hauteur d’une frame = 46
            Dictionary<string, (int row, int frameCount)> animationMap
        )
        {
            Nom = nom;

            // Charger le portrait (optionnel mais utile pour UCMenu plus tard)
            if (!string.IsNullOrWhiteSpace(portraitFile))
            {
                Portrait = new BitmapImage(new Uri($"/img/{portraitFile}", UriKind.Relative));
            }

            // Charger la spritesheet
            SpriteSheet = new BitmapImage(new Uri($"/img/{spriteFile}", UriKind.Relative));

            // Créer le gestionnaire d’animations
            AnimManager = new AnimationManager(targetImage, frameWidth, frameHeight, SpriteSheet, animationMap);
        }

        public void JouerAnimation(string animationName)
        {
            AnimManager.Play(animationName);
        }
    }
}
