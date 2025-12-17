using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VikingGame
{
    public class Personnage
    {
        private readonly AnimationManager animator;

        public Personnage(
            string name,
            string portraitFile,
            string spriteSheetFile,
            Image targetImage,
            Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)> animationMap)
        {
            var spriteSheet = new BitmapImage();
            spriteSheet.BeginInit();

            // Chargement en ressource WPF (assembly Viking)
            spriteSheet.UriSource = new Uri($"pack://application:,,,/Viking;component/img/{spriteSheetFile}");
            spriteSheet.CacheOption = BitmapCacheOption.OnLoad;
            spriteSheet.CreateOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreImageCache;
            spriteSheet.EndInit();

            animator = new AnimationManager(targetImage, spriteSheet, animationMap);
        }

        public void JouerAnimation(string animationName) => animator.Play(animationName);
    }
}
