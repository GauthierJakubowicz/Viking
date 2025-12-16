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
        private Dictionary<string, BitmapSource[]> animations = new Dictionary<string, BitmapSource[]>();
        private DispatcherTimer timer;
        private int currentFrame = 0;
        private string currentAnimation = "Idle";
        private Image imgTarget;

        public AnimationManager(Image targetImage, int frameWidth, int frameHeight, BitmapImage spriteSheet, Dictionary<string, (int row, int frameCount)> animationMap)
        {
            imgTarget = targetImage;

            foreach (var anim in animationMap)
            {
                string name = anim.Key;
                int row = anim.Value.row;
                int count = anim.Value.frameCount;

                BitmapSource[] frames = new BitmapSource[count];
                for (int i = 0; i < count; i++)
                {
                    frames[i] = new CroppedBitmap(spriteSheet, new Int32Rect(i * frameWidth, row * frameHeight, frameWidth, frameHeight));
                }
                animations[name] = frames;
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Animate;
            timer.Start();
        }

        private void Animate(object sender, EventArgs e)
        {
            if (!animations.ContainsKey(currentAnimation)) return;

            imgTarget.Source = animations[currentAnimation][currentFrame];
            currentFrame = (currentFrame + 1) % animations[currentAnimation].Length;
        }

        public void Play(string animationName)
        {
            if (animationName == currentAnimation) return;

            if (animations.ContainsKey(animationName))
            {
                currentAnimation = animationName;
                currentFrame = 0;
            }
        }
    }
}