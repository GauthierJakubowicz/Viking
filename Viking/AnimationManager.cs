using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public AnimationManager(Image targetImage, int frameWidth, int frameHeight, BitmapImage spriteSheet, Dictionary<string, (int row, int frameCount)> animationMap)
        {
            imgTarget = targetImage ?? throw new ArgumentNullException(nameof(targetImage));

            if (spriteSheet == null || spriteSheet.PixelWidth == 0 || spriteSheet.PixelHeight == 0)
                throw new Exception("SpriteSheet non chargée ou invalide !");

            foreach (var anim in animationMap)
            {
                string name = anim.Key;
                int row = anim.Value.row;
                int count = anim.Value.frameCount;

                BitmapSource[] frames = new BitmapSource[count];
                for (int i = 0; i < count; i++)
                {
                    // Vérification que la découpe reste dans les limites de la spritesheet
                    if ((i + 1) * frameWidth <= spriteSheet.PixelWidth &&
                        (row + 1) * frameHeight <= spriteSheet.PixelHeight)
                    {
                        frames[i] = new CroppedBitmap(spriteSheet, new Int32Rect(i * frameWidth, row * frameHeight, frameWidth, frameHeight));
                    }
                }
                animations[name] = frames;
            }

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(120) // vitesse ajustée pour éviter scintillement
            };
            timer.Tick += Animate;
            timer.Start();
        }

        private void Animate(object sender, EventArgs e)
        {
            if (!animations.ContainsKey(currentAnimation)) return;

            var frames = animations[currentAnimation];
            if (frames == null || frames.Length == 0) return;

            var frame = frames[currentFrame];
            if (frame != null)
                imgTarget.Source = frame;

            currentFrame = (currentFrame + 1) % frames.Length;
        }

        public void Play(string animationName)
        {
            if (string.IsNullOrWhiteSpace(animationName)) return;
            if (animationName == currentAnimation) return;

            if (animations.ContainsKey(animationName))
            {
                currentAnimation = animationName;
                currentFrame = 0;
            }
        }
    }
}
