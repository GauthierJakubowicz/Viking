using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VikingGame;

namespace Viking
{
    public partial class UCJeu : UserControl
    {
        private Personnage personnageActuel;
        private string typePersonnage;
        private double vitesseMarche = 3;
        private double vitesseCourse = 6;

        // Constructeur par défaut (Viking)
        public UCJeu() : this("Viking")
        {
        }

        // Constructeur avec choix du personnage
        public UCJeu(string typePersonnage)
        {
            InitializeComponent();
            this.typePersonnage = typePersonnage;
            Loaded += UCJeu_Loaded;
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            // Créer le personnage en fonction du type choisi
            CreerPersonnage(typePersonnage);

            // Initialiser la position du personnage
            Canvas.SetLeft(imgViking, 100);
            Canvas.SetTop(imgViking, 300);

            // Empêcher le zoom de l'image
            imgViking.Stretch = Stretch.None;
            imgViking.SnapsToDevicePixels = true;
            imgViking.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(imgViking, BitmapScalingMode.NearestNeighbor);

            // Brancher les touches clavier
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.KeyDown += ParentWindow_KeyDown;
            }
        }

        private void CreerPersonnage(string type)
        {
            switch (type)
            {
                case "Viking":
                    personnageActuel = Personnage.CreerViking(imgViking);
                    break;
                case "Spearwoman":
                    personnageActuel = Personnage.CreerSpearwoman(imgViking);
                    break;
                case "FireWarrior":
                    personnageActuel = Personnage.CreerFireWarrior(imgViking);
                    break;
                default:
                    personnageActuel = Personnage.CreerViking(imgViking);
                    break;
            }
        }

        // Méthode pour obtenir une position sûre (évite les NaN)
        private double GetSafeLeft()
        {
            double left = Canvas.GetLeft(imgViking);
            return double.IsNaN(left) ? 100 : left;
        }

        private void ParentWindow_KeyDown(object sender, KeyEventArgs e)
        {
            string animation = "";
            double deplacement = 0;

            // DÉPLACEMENT - Animations communes à tous
            if (e.Key == Key.Right)
            {
                personnageActuel.FacingRight = true;
                animation = "Run";
                deplacement = vitesseCourse;
            }
            else if (e.Key == Key.Left)
            {
                personnageActuel.FacingRight = false;
                animation = "Walk";
                deplacement = -vitesseMarche;
            }
            else if (e.Key == Key.Down || e.Key == Key.S)
            {
                animation = "Slide";
            }
            // COMBAT - Animations communes à tous
            else if (e.Key == Key.Space)
            {
                animation = "Attack1";
            }
            else if (e.Key == Key.A)
            {
                animation = "Attack2";
            }
            else if (e.Key == Key.Z) // AZERTY
            {
                animation = "Attack3";
            }
            else if (e.Key == Key.E)
            {
                animation = "Block";
            }
            // ANIMATIONS DE TEST - Communes à tous
            else if (e.Key == Key.I)
            {
                animation = "Idle";
            }
            else if (e.Key == Key.H)
            {
                animation = "Hit";
            }
            else if (e.Key == Key.D)
            {
                animation = "Death";
            }
            // CHANGEMENT DE PERSONNAGE
            else if (e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                typePersonnage = "Viking";
                CreerPersonnage(typePersonnage);
                return;
            }
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                typePersonnage = "Spearwoman";
                CreerPersonnage(typePersonnage);
                return;
            }
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3)
            {
                typePersonnage = "FireWarrior";
                CreerPersonnage(typePersonnage);
                return;
            }

            // Appliquer l'animation si une action a été détectée
            if (!string.IsNullOrEmpty(animation))
            {
                personnageActuel.JouerAnimation(animation);
            }

            // Appliquer le déplacement si nécessaire
            if (deplacement != 0)
            {
                double nouvellePosition = GetSafeLeft() + deplacement;
                // Limiter aux bords du canvas
                nouvellePosition = Math.Max(0, Math.Min(nouvellePosition, 1200));
                Canvas.SetLeft(imgViking, nouvellePosition);
            }
        }
    }
}