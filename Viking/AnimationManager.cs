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
        private readonly Dictionary<string, BitmapSource[]> animations = new();
        private readonly DispatcherTimer timer;
        private int currentFrame = 0;
        private string currentAnimation = "Idle";
        private readonly Image imgTarget;
        private bool facingRight = true;

        public bool FacingRight
        {
            get => facingRight;
            set
            {
                if (facingRight != value)
                {
                    facingRight = value;
                    UpdateImageTransform();
                }
            }
        }

        public AnimationManager(
            Image targetImage,
            BitmapImage spriteSheet,
            Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)> animationMap)
        {
            imgTarget = targetImage ?? throw new ArgumentNullException(nameof(targetImage));
            if (spriteSheet == null) throw new ArgumentNullException(nameof(spriteSheet));

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
                    // Frame normale
                    var frameCrop = new CroppedBitmap(
                        spriteSheet,
                        new Int32Rect(i * frameW, row * frameH, frameW, frameH)
                    );

                    if (!frameCrop.IsFrozen)
                    {
                        frameCrop.Freeze();
                    }

                    frames[i] = frameCrop;
                }

                animations[name] = frames;
            }

            // Configuration de l'image cible
            imgTarget.Stretch = Stretch.None;
            imgTarget.SnapsToDevicePixels = true;
            imgTarget.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(imgTarget, BitmapScalingMode.NearestNeighbor);

            // Initialiser la transformation
            UpdateImageTransform();

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(120) };
            timer.Tick += Animate;
            timer.Start();
        }

        private void UpdateImageTransform()
        {
            // Créer une transformation qui retourne l'image si nécessaire
            var scaleTransform = new ScaleTransform(
                facingRight ? 1 : -1,  // ScaleX: 1 = normal, -1 = retourné
                1,                       // ScaleY: toujours 1
                imgTarget.Width / 2,     // Centre X de la transformation
                imgTarget.Height / 2     // Centre Y de la transformation
            );

            imgTarget.RenderTransform = scaleTransform;
            imgTarget.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void Animate(object sender, EventArgs e)
        {
            if (!animations.TryGetValue(currentAnimation, out var frames) || frames.Length == 0)
                return;

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