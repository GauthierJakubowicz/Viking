using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            // Dictionnaire des animations du Viking
            var vikingAnimationMap = new Dictionary<string, (int, int)>
            {
                { "Idle", (0, 8) },
                { "Walk", (1, 8) },
                { "Run", (2, 8) },
                { "Slide", (3, 7) },
                { "Crouch", (4, 2) },
                { "CrouchAttack", (5, 5) },
                { "Jump", (6, 1) },
                { "JumpToFall", (7, 3) },
                { "Fall", (8, 1) },
                { "JumpAttack", (9, 6) },
                { "IdleBlock", (10, 8) },
                { "Block", (11, 5) },
                { "Attack1", (12, 4) },
                { "Attack2", (13, 4) },
                { "Attack3", (14, 4) },
                { "Spell", (15, 12) },
                { "SpellSlam", (16, 11) },
                { "LadderClimb", (17, 8) },
                { "WallHang", (18, 6) },
                { "WallClimb", (19, 5) },
                { "TransformationOut", (20, 8) },
                { "Dash", (21, 5) },
                { "Hit", (22, 4) },
                { "Death", (23, 12) }
            };

            // Création du personnage Viking
            viking = new Personnage("Viking", "Viking Portrait.png", "Viking-Sheet.png", imgViking, 34, 46, vikingAnimationMap);


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
