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

        public UCJeu()
        {
            InitializeComponent();
            Loaded += UCJeu_Loaded;
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            // Créer le Viking par défaut
            // Vous pouvez changer pour créer Spearwoman ou FireWarrior
            personnageActuel = Personnage.CreerViking(imgViking);

            // Pour changer de personnage, utilisez :
            // personnageActuel = Personnage.CreerSpearwoman(imgViking);
            // personnageActuel = Personnage.CreerFireWarrior(imgViking);

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

        private void ParentWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                personnageActuel.FacingRight = true;
                personnageActuel.JouerAnimation("Run");
                Canvas.SetLeft(imgViking, Canvas.GetLeft(imgViking) + 5);
            }
            else if (e.Key == Key.Left)
            {
                personnageActuel.FacingRight = false;
                personnageActuel.JouerAnimation("Walk");
                Canvas.SetLeft(imgViking, Canvas.GetLeft(imgViking) - 5);
            }
            else if (e.Key == Key.Space)
            {
                personnageActuel.JouerAnimation("Attack1");
            }
            else if (e.Key == Key.D)
            {
                personnageActuel.JouerAnimation("Death");
            }
            else if (e.Key == Key.I)
            {
                personnageActuel.JouerAnimation("Idle");
            }
            else if (e.Key == Key.J)
            {
                personnageActuel.JouerAnimation("Jump");
            }
            // Touches pour changer de personnage
            else if (e.Key == Key.D1)
            {
                personnageActuel = Personnage.CreerViking(imgViking);
            }
            else if (e.Key == Key.D2)
            {
                personnageActuel = Personnage.CreerSpearwoman(imgViking);
            }
            else if (e.Key == Key.D3)
            {
                personnageActuel = Personnage.CreerFireWarrior(imgViking);
            }
        }
    }
}