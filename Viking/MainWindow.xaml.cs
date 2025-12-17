using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace VikingGame
{
    public partial class MainWindow : Window
    {
        private static MediaPlayer musique;
        public MainWindow()
        {
            InitializeComponent();
            InitMusique();
        }
        private void InitMusique()
        {
            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "P:\\R1_01_InitiationDev_C#\\Valhalla\\Viking\\Viking\\Sons\\musique_de_fond.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Volume = 0.5;
            musique.Play();
        }
        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }


        // Méthode pour changer de page
        public void ShowPage(UserControl page)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(page);
        }
    }
}