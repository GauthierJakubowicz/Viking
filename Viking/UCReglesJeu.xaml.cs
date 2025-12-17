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

namespace Viking
{
    public partial class UCReglesJeu : UserControl
    {
        public UCReglesJeu()
        {
            InitializeComponent();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            // Retourner au menu principal
            ((VikingGame.MainWindow)Application.Current.MainWindow).ShowPage(new VikingGame.UCMenu());
        }
    }
}