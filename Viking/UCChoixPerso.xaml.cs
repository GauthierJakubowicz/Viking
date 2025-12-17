using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VikingGame;

namespace Viking
{
    public partial class UCChoixPerso : UserControl
    {
        private string? joueur1Selection = null;
        private string? joueur2Selection = null;

        public UCChoixPerso()
        {
            InitializeComponent();
        }

        // ==================== JOUEUR 1 ====================
        private void ImgViking1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectionnerJoueur1("Viking");
        }

        private void ImgSpearwoman1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectionnerJoueur1("Spearwoman");
        }

        private void ImgFireWarrior1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectionnerJoueur1("FireWarrior");
        }

        private void SelectionnerJoueur1(string personnage)
        {
            joueur1Selection = personnage;

            // Indiquer visuellement la sélection (bordure verte)
            ResetBorduresJoueur1();

            switch (personnage)
            {
                case "Viking":
                    BorderViking1.BorderBrush = Brushes.Green;
                    BorderViking1.BorderThickness = new Thickness(3);
                    break;
                case "Spearwoman":
                    BorderSpearwoman1.BorderBrush = Brushes.Green;
                    BorderSpearwoman1.BorderThickness = new Thickness(3);
                    break;
                case "FireWarrior":
                    BorderFireWarrior1.BorderBrush = Brushes.Green;
                    BorderFireWarrior1.BorderThickness = new Thickness(3);
                    break;
            }

            TxtJoueur1.Text = $"Joueur 1: {personnage}";
            VerifierDeuxJoueursChoisis();
        }

        private void ResetBorduresJoueur1()
        {
            BorderViking1.BorderBrush = Brushes.Transparent;
            BorderViking1.BorderThickness = new Thickness(0);
            BorderSpearwoman1.BorderBrush = Brushes.Transparent;
            BorderSpearwoman1.BorderThickness = new Thickness(0);
            BorderFireWarrior1.BorderBrush = Brushes.Transparent;
            BorderFireWarrior1.BorderThickness = new Thickness(0);
        }

        // ==================== JOUEUR 2 ====================
        private void ImgViking2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectionnerJoueur2("Viking");
        }

        private void ImgSpearwoman2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectionnerJoueur2("Spearwoman");
        }

        private void ImgFireWarrior2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectionnerJoueur2("FireWarrior");
        }

        private void SelectionnerJoueur2(string personnage)
        {
            joueur2Selection = personnage;

            // Indiquer visuellement la sélection (bordure bleue)
            ResetBorduresJoueur2();

            switch (personnage)
            {
                case "Viking":
                    BorderViking2.BorderBrush = Brushes.Blue;
                    BorderViking2.BorderThickness = new Thickness(3);
                    break;
                case "Spearwoman":
                    BorderSpearwoman2.BorderBrush = Brushes.Blue;
                    BorderSpearwoman2.BorderThickness = new Thickness(3);
                    break;
                case "FireWarrior":
                    BorderFireWarrior2.BorderBrush = Brushes.Blue;
                    BorderFireWarrior2.BorderThickness = new Thickness(3);
                    break;
            }

            TxtJoueur2.Text = $"Joueur 2: {personnage}";
            VerifierDeuxJoueursChoisis();
        }

        private void ResetBorduresJoueur2()
        {
            BorderViking2.BorderBrush = Brushes.Transparent;
            BorderViking2.BorderThickness = new Thickness(0);
            BorderSpearwoman2.BorderBrush = Brushes.Transparent;
            BorderSpearwoman2.BorderThickness = new Thickness(0);
            BorderFireWarrior2.BorderBrush = Brushes.Transparent;
            BorderFireWarrior2.BorderThickness = new Thickness(0);
        }

        // ==================== LANCER LE JEU ====================
        private void VerifierDeuxJoueursChoisis()
        {
            if (joueur1Selection != null && joueur2Selection != null)
            {
                BtnCommencer.IsEnabled = true;
                BtnCommencer.Opacity = 1.0;
            }
        }

        private void BtnCommencer_Click(object sender, RoutedEventArgs e)
        {
            if (joueur1Selection != null && joueur2Selection != null)
            {
                var ucJeu = new UCJeu(joueur1Selection, joueur2Selection);
                ((MainWindow)Application.Current.MainWindow).ShowPage(ucJeu);
            }
        }
    }
}