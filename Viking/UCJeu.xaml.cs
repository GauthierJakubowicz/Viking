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
        private Personnage viking;

        public UCJeu()
        {
            InitializeComponent();
            Loaded += UCJeu_Loaded;
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            // Dictionnaire des animations du Viking (uniforme 124x84)
            var vikingAnimationMap = new Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)>
            {
                { "Idle", (0, 8, 124, 84) },
                { "Walk", (1, 8, 124, 84) },
                { "Run", (2, 8, 124, 84) },
                { "Slide", (3, 7, 124, 84) },
                { "Crouch", (4, 2, 124, 84) },
                { "CrouchAttack", (5, 5, 124, 84) },
                { "Jump", (6, 1, 124, 84) },
                { "JumpToFall", (7, 3, 124, 84) },
                { "Fall", (8, 1, 124, 84) },
                { "JumpAttack", (9, 6, 124, 84) },
                { "IdleBlock", (10, 8, 124, 84) },
                { "Block", (11, 5, 124, 84) },
                { "Attack1", (12, 4, 124, 84) },
                { "Attack2", (13, 4, 124, 84) },
                { "Attack3", (14, 4, 124, 84) },
                { "Spell", (15, 12, 124, 84) },
                { "SpellSlam", (16, 11, 124, 84) },
                { "LadderClimb", (17, 8, 124, 84) },
                { "WallHang", (18, 6, 124, 84) },
                { "WallClimb", (19, 5, 124, 84) },
                { "TransformationOut", (20, 8, 124, 84) },
                { "Dash", (21, 5, 124, 84) },
                { "Hit", (22, 4, 124, 84) },
                { "Death", (23, 12, 124, 84) }
            };

            // Création du personnage Viking
            viking = new Personnage("Viking", "Viking Portrait.png", "Viking-Sheet.png", imgViking, vikingAnimationMap);

            // Empêcher le zoom de l’image
            imgViking.Stretch = Stretch.None;
            imgViking.Width = double.NaN;
            imgViking.Height = double.NaN;
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
                viking.JouerAnimation("Run");
                Canvas.SetLeft(imgViking, Canvas.GetLeft(imgViking) + 5);
            }
            else if (e.Key == Key.Left)
            {
                viking.JouerAnimation("Walk");
                Canvas.SetLeft(imgViking, Canvas.GetLeft(imgViking) - 5);
            }
            else if (e.Key == Key.Space)
            {
                viking.JouerAnimation("Attack1");
            }
            else if (e.Key == Key.D)
            {
                viking.JouerAnimation("Death");
            }
            else if (e.Key == Key.I)
            {
                viking.JouerAnimation("Idle");
            }
        }
    }
}
