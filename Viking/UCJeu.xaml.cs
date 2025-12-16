using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace VikingGame
{
    public partial class UCJeu : UserControl
    {
        private AnimationManager animManager;

        public UCJeu()
        {
            InitializeComponent();
            Loaded += UCJeu_Loaded;
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage spriteSheet = new BitmapImage(
                new Uri("pack://application:,,,/VikingGame;component/Viking/viking.png")
            );

            Dictionary<string, (int row, int frameCount)> animationMap = new Dictionary<string, (int, int)>
            {
                { "Idle", (0, 8) },
                { "Walk", (1, 8) },
                { "Run", (2, 8) },
                { "Attack1", (3, 4) },
                { "Death", (4, 12) }
            };

            animManager = new AnimationManager(imgViking, 33, 46, spriteSheet, animationMap);

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
                animManager.Play("Run");
                Canvas.SetLeft(imgViking, Canvas.GetLeft(imgViking) + 5);
            }
            else if (e.Key == Key.Left)
            {
                animManager.Play("Walk");
                Canvas.SetLeft(imgViking, Canvas.GetLeft(imgViking) - 5);
            }
            else if (e.Key == Key.Space)
            {
                animManager.Play("Attack1");
            }
            else if (e.Key == Key.D)
            {
                animManager.Play("Death");
            }
            else if (e.Key == Key.I)
            {
                animManager.Play("Idle");
            }
        }
    }
}