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
        private Personnage joueur1;
        private Personnage joueur2;
        private double vitesseMarche = 3;
        private double vitesseCourse = 6;

        // Constructeur qui reçoit les deux personnages choisis
        public UCJeu(string typeJoueur1, string typeJoueur2)
        {
            InitializeComponent();
            Loaded += (s, e) => UCJeu_Loaded(typeJoueur1, typeJoueur2);
        }

        private void UCJeu_Loaded(string typeJoueur1, string typeJoueur2)
        {
            // Créer le joueur 1 (à gauche)
            joueur1 = CreerPersonnage(typeJoueur1, imgJoueur1);
            Canvas.SetLeft(imgJoueur1, 100);
            Canvas.SetTop(imgJoueur1, 300);
            ConfigurerImage(imgJoueur1);

            // Créer le joueur 2 (à droite)
            joueur2 = CreerPersonnage(typeJoueur2, imgJoueur2);
            Canvas.SetLeft(imgJoueur2, 1000);
            Canvas.SetTop(imgJoueur2, 300);
            joueur2.FacingRight = false;
            ConfigurerImage(imgJoueur2);

            // Brancher les touches clavier
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.KeyDown += ParentWindow_KeyDown;
            }
        }

        private void ConfigurerImage(Image img)
        {
            img.Stretch = Stretch.None;
            img.SnapsToDevicePixels = true;
            img.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
        }

        private Personnage CreerPersonnage(string type, Image targetImage)
        {
            switch (type)
            {
                case "Viking":
                    return Personnage.CreerViking(targetImage);
                case "Spearwoman":
                    return Personnage.CreerSpearwoman(targetImage);
                case "FireWarrior":
                    return Personnage.CreerFireWarrior(targetImage);
                default:
                    return Personnage.CreerViking(targetImage);
            }
        }

        private double GetSafeLeft(Image img)
        {
            double left = Canvas.GetLeft(img);
            return double.IsNaN(left) ? 100 : left;
        }

        private void ParentWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // ==================== JOUEUR 1 ====================
            string animJ1 = "";
            double deplacementJ1 = 0;

            if (e.Key == Key.D)
            {
                joueur1.FacingRight = true;
                animJ1 = "Run";
                deplacementJ1 = vitesseCourse;
            }
            else if (e.Key == Key.Q)
            {
                joueur1.FacingRight = false;
                animJ1 = "Walk";
                deplacementJ1 = -vitesseMarche;
            }
            else if (e.Key == Key.S)
            {
                animJ1 = "Slide";
            }
            else if (e.Key == Key.Space)
            {
                animJ1 = "Attack1";
            }
            else if (e.Key == Key.A)
            {
                animJ1 = "Attack2";
            }
            else if (e.Key == Key.Z)
            {
                animJ1 = "Attack3";
            }
            else if (e.Key == Key.E)
            {
                animJ1 = "Block";
            }
            else if (e.Key == Key.R)
            {
                animJ1 = "Hit";
            }
            else if (e.Key == Key.F)
            {
                animJ1 = "Death";
            }
            else if (e.Key == Key.T)
            {
                animJ1 = "Idle";
            }

            if (!string.IsNullOrEmpty(animJ1))
            {
                joueur1.JouerAnimation(animJ1);
            }

            if (deplacementJ1 != 0)
            {
                double nouvellePosition = GetSafeLeft(imgJoueur1) + deplacementJ1;
                nouvellePosition = Math.Max(0, Math.Min(nouvellePosition, 1200));
                Canvas.SetLeft(imgJoueur1, nouvellePosition);
            }

            // ==================== JOUEUR 2 ====================
            string animJ2 = "";
            double deplacementJ2 = 0;

            if (e.Key == Key.Right)
            {
                joueur2.FacingRight = true;
                animJ2 = "Run";
                deplacementJ2 = vitesseCourse;
            }
            else if (e.Key == Key.Left)
            {
                joueur2.FacingRight = false;
                animJ2 = "Walk";
                deplacementJ2 = -vitesseMarche;
            }
            else if (e.Key == Key.Down)
            {
                animJ2 = "Slide";
            }
            else if (e.Key == Key.NumPad0 || e.Key == Key.RightCtrl)
            {
                animJ2 = "Attack1";
            }
            else if (e.Key == Key.NumPad1)
            {
                animJ2 = "Attack2";
            }
            else if (e.Key == Key.NumPad2)
            {
                animJ2 = "Attack3";
            }
            else if (e.Key == Key.NumPad3 || e.Key == Key.RightShift)
            {
                animJ2 = "Block";
            }
            else if (e.Key == Key.NumPad4)
            {
                animJ2 = "Hit";
            }
            else if (e.Key == Key.NumPad5)
            {
                animJ2 = "Death";
            }
            else if (e.Key == Key.NumPad6)
            {
                animJ2 = "Idle";
            }

            if (!string.IsNullOrEmpty(animJ2))
            {
                joueur2.JouerAnimation(animJ2);
            }

            if (deplacementJ2 != 0)
            {
                double nouvellePosition = GetSafeLeft(imgJoueur2) + deplacementJ2;
                nouvellePosition = Math.Max(0, Math.Min(nouvellePosition, 1200));
                Canvas.SetLeft(imgJoueur2, nouvellePosition);
            }
        }
    }
}