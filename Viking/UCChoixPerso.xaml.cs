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
    /// <summary>
    /// Logique d'interaction pour UCChoixPerso.xaml
    /// </summary>
    public partial class UCChoixPerso : UserControl
    {
        public UCChoixPerso()
        {
            InitializeComponent();
        }

        // Méthode appelée quand on clique sur le Viking
        private void ImgViking_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LancerJeuAvecPersonnage("Viking");
        }

        // Méthode appelée quand on clique sur la Spearwoman
        private void ImgSpearwoman_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LancerJeuAvecPersonnage("Spearwoman");
        }

        // Méthode appelée quand on clique sur le Fire Warrior
        private void ImgFireWarrior_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LancerJeuAvecPersonnage("FireWarrior");
        }

        // Méthode qui lance le jeu avec le personnage sélectionné
        private void LancerJeuAvecPersonnage(string typePersonnage)
        {
            var ucJeu = new UCJeu(typePersonnage);
            ((MainWindow)Application.Current.MainWindow).ShowPage(ucJeu);
        }
    }
}