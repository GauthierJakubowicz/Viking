using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using VikingGame;

namespace Viking
{
    public partial class UCJeu : UserControl
    {
        private Personnage joueur1 = null!;
        private Personnage joueur2 = null!;
        private double vitesseMarche = 3;
        private double vitesseCourse = 6;

        // Système de combat simplifié
        private bool joueur1EstEnAttaque = false;
        private bool joueur2EstEnAttaque = false;
        private bool joueur1Bloque = false;
        private bool joueur2Bloque = false;
        private bool partieTerminee = false;

        // Constructeur
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
                parentWindow.KeyUp += ParentWindow_KeyUp;
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

        private bool JoueursEnContact()
        {
            double posJ1 = GetSafeLeft(imgJoueur1);
            double posJ2 = GetSafeLeft(imgJoueur2);

            // Distance de contact pour une attaque
            double distanceContact = 150;

            return Math.Abs(posJ1 - posJ2) < distanceContact;
        }

        private void VerifierCoup()
        {
            if (partieTerminee || !JoueursEnContact())
                return;

            // Joueur 1 attaque Joueur 2
            if (joueur1EstEnAttaque && !joueur2Bloque)
            {
                FinDePartie("JOUEUR 1");
                return;
            }

            // Joueur 2 attaque Joueur 1
            if (joueur2EstEnAttaque && !joueur1Bloque)
            {
                FinDePartie("JOUEUR 2");
                return;
            }
        }

        private void FinDePartie(string gagnant)
        {
            if (partieTerminee) return;
            partieTerminee = true;

            // Désactiver les contrôles
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.KeyDown -= ParentWindow_KeyDown;
                parentWindow.KeyUp -= ParentWindow_KeyUp;
            }

            // Jouer l'animation de mort du perdant
            if (gagnant == "JOUEUR 1")
            {
                joueur2.JouerAnimation("Death");
            }
            else
            {
                joueur1.JouerAnimation("Death");
            }

            // Afficher le message de victoire après un court délai
            var timerVictoire = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            timerVictoire.Tick += (s, e) =>
            {
                AfficherEcranVictoire(gagnant);
                timerVictoire.Stop();
            };
            timerVictoire.Start();
        }

        private void AfficherEcranVictoire(string gagnant)
        {
            // Créer un overlay semi-transparent
            var overlay = new Rectangle
            {
                Width = 1280,
                Height = 720,
                Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0))
            };
            Canvas.SetLeft(overlay, 0);
            Canvas.SetTop(overlay, 0);
            GameCanvas.Children.Add(overlay);

            // Créer le panneau de victoire
            var panneauVictoire = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                CornerRadius = new CornerRadius(15),
                BorderBrush = new SolidColorBrush(Color.FromRgb(241, 196, 15)),
                BorderThickness = new Thickness(5),
                Padding = new Thickness(50)
            };

            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Texte de victoire
            var textVictoire = new TextBlock
            {
                Text = $"🏆 {gagnant} GAGNE ! 🏆",
                FontSize = 64,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(241, 196, 15)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 40),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 320,
                    ShadowDepth = 5,
                    BlurRadius = 10
                }
            };

            // Bouton Rejouer
            var btnRejouer = CreerBouton("🔄 REJOUER", Color.FromRgb(46, 204, 113));
            btnRejouer.Click += (s, e) =>
            {
                var ucChoix = new UCChoixPerso();
                ((MainWindow)Application.Current.MainWindow).ShowPage(ucChoix);
            };

            // Bouton Menu
            var btnMenu = CreerBouton("🏠 MENU PRINCIPAL", Color.FromRgb(231, 76, 60));
            btnMenu.Click += (s, e) =>
            {
                var ucMenu = new UCMenu();
                ((MainWindow)Application.Current.MainWindow).ShowPage(ucMenu);
            };

            stackPanel.Children.Add(textVictoire);
            stackPanel.Children.Add(btnRejouer);
            stackPanel.Children.Add(btnMenu);
            panneauVictoire.Child = stackPanel;

            Canvas.SetLeft(panneauVictoire, 290);
            Canvas.SetTop(panneauVictoire, 180);
            GameCanvas.Children.Add(panneauVictoire);
        }

        private Button CreerBouton(string texte, Color couleur)
        {
            var btn = new Button
            {
                Content = texte,
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(40, 15),
                Background = new SolidColorBrush(couleur),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Margin = new Thickness(0, 0, 0, 15),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 320,
                    ShadowDepth = 3,
                    BlurRadius = 5
                }
            };

            // Style avec effet hover
            btn.MouseEnter += (s, e) =>
            {
                btn.Background = new SolidColorBrush(Color.FromRgb(
                    (byte)(couleur.R * 0.8),
                    (byte)(couleur.G * 0.8),
                    (byte)(couleur.B * 0.8)
                ));
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.Background = new SolidColorBrush(couleur);
            };

            return btn;
        }

        private void ParentWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (partieTerminee) return;

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
                joueur1EstEnAttaque = true;
                VerifierCoup();
            }
            else if (e.Key == Key.A)
            {
                animJ1 = "Attack2";
                joueur1EstEnAttaque = true;
                VerifierCoup();
            }
            else if (e.Key == Key.Z)
            {
                animJ1 = "Attack3";
                joueur1EstEnAttaque = true;
                VerifierCoup();
            }
            else if (e.Key == Key.E)
            {
                animJ1 = "Block";
                joueur1Bloque = true;
            }

            if (!string.IsNullOrEmpty(animJ1))
            {
                joueur1.JouerAnimation(animJ1);
            }

            if (deplacementJ1 != 0)
            {
                double nouvellePosition = GetSafeLeft(imgJoueur1) + deplacementJ1;
                nouvellePosition = Math.Max(0, Math.Min(nouvellePosition, 1100));
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
                joueur2EstEnAttaque = true;
                VerifierCoup();
            }
            else if (e.Key == Key.NumPad1)
            {
                animJ2 = "Attack2";
                joueur2EstEnAttaque = true;
                VerifierCoup();
            }
            else if (e.Key == Key.NumPad2)
            {
                animJ2 = "Attack3";
                joueur2EstEnAttaque = true;
                VerifierCoup();
            }
            else if (e.Key == Key.NumPad3 || e.Key == Key.RightShift)
            {
                animJ2 = "Block";
                joueur2Bloque = true;
            }

            if (!string.IsNullOrEmpty(animJ2))
            {
                joueur2.JouerAnimation(animJ2);
            }

            if (deplacementJ2 != 0)
            {
                double nouvellePosition = GetSafeLeft(imgJoueur2) + deplacementJ2;
                nouvellePosition = Math.Max(0, Math.Min(nouvellePosition, 1100));
                Canvas.SetLeft(imgJoueur2, nouvellePosition);
            }
        }

        private void ParentWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (partieTerminee) return;

            // Relâcher les attaques et blocages
            if (e.Key == Key.Space || e.Key == Key.A || e.Key == Key.Z)
            {
                joueur1EstEnAttaque = false;
            }

            if (e.Key == Key.E)
            {
                joueur1Bloque = false;
                joueur1.JouerAnimation("Idle");
            }

            if (e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.RightCtrl)
            {
                joueur2EstEnAttaque = false;
            }

            if (e.Key == Key.NumPad3 || e.Key == Key.RightShift)
            {
                joueur2Bloque = false;
                joueur2.JouerAnimation("Idle");
            }
        }
    }
}