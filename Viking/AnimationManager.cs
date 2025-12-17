using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VikingGame
{
    public class AnimationManager
    {
        private readonly Dictionary<string, BitmapSource[]> animations = new Dictionary<string, BitmapSource[]>();
        private readonly DispatcherTimer timer;
        private int currentFrame = 0;
        private string currentAnimation = "Idle";
        private readonly Image imgTarget;

        // Taille de cellule de la spritesheet (12 colonnes × 24 lignes)
        private const int CellWidth = 124;
        private const int CellHeight = 84;

        public AnimationManager(
            Image targetImage,
            BitmapImage spriteSheet,
            Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)> animationMap)
        {
            imgTarget = targetImage ?? throw new ArgumentNullException(nameof(targetImage));
            if (spriteSheet == null || spriteSheet.PixelWidth == 0 || spriteSheet.PixelHeight == 0)
                throw new Exception("SpriteSheet non chargée ou invalide !");

            foreach (var anim in animationMap)
            {
                string name = anim.Key;
                int row = anim.Value.row;
                int count = anim.Value.frameCount;
                int frameW = anim.Value.frameWidth;
                int frameH = anim.Value.frameHeight;

                var frames = new BitmapSource[count];
                for (int i = 0; i < count; i++)
                {
                    // Découpe la cellule complète
                    var cellCrop = new CroppedBitmap(
                        spriteSheet,
                        new Int32Rect(i * CellWidth, row * CellHeight, CellWidth, CellHeight)
                    );

                    // Crée un visuel fixe 124x84
                    var dv = new DrawingVisual();
                    using (var dc = dv.RenderOpen())
                    {
                        // Fond transparent
                        dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, CellWidth, CellHeight));

                        // Découpe utile (frameW × frameH) depuis le coin haut-gauche de la cellule
                        var usefulCrop = new CroppedBitmap(
                            cellCrop,
                            new Int32Rect(0, 0, Math.Min(frameW, CellWidth), Math.Min(frameH, CellHeight))
                        );

                        // Calcul du centrage
                        double offsetX = (CellWidth - usefulCrop.PixelWidth) / 2.0;
                        double offsetY = (CellHeight - usefulCrop.PixelHeight) / 2.0;

                        // Dessine la frame centrée
                        dc.DrawImage(usefulCrop, new Rect(offsetX, offsetY, usefulCrop.PixelWidth, usefulCrop.PixelHeight));
                    }

                    var bmp = new RenderTargetBitmap(CellWidth, CellHeight, 96, 96, PixelFormats.Pbgra32);
                    bmp.Render(dv);
                    frames[i] = bmp;
                }

                animations[name] = frames;
            }

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(120) };
            timer.Tick += Animate;
            timer.Start();
        }

        private void Animate(object sender, EventArgs e)
        {
            if (!animations.TryGetValue(currentAnimation, out var frames) || frames.Length == 0) return;
            imgTarget.Source = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
        }

        public void Play(string animationName)
        {
            if (string.IsNullOrWhiteSpace(animationName)) return;
            if (!animations.ContainsKey(animationName)) return;

            if (animationName != currentAnimation)
            {
                currentAnimation = animationName;
                currentFrame = 0;
            }
        }
    }
}
