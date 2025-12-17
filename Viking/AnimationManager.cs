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

        private const int CellWidth = 124;
        private const int CellHeight = 84;

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

                // --- Étape 1 : calcul des bornes globales ---
                int globalLeft = frameW, globalRight = 0, globalTop = frameH, globalBottom = 0;

                for (int i = 0; i < count; i++)
                {
                    var frameCrop = new CroppedBitmap(spriteSheet, new Int32Rect(i * frameW, row * frameH, frameW, frameH));
                    var pixels = new byte[frameW * frameH * 4];
                    frameCrop.CopyPixels(pixels, frameW * 4, 0);

                    int left = frameW, right = 0, top = frameH, bottom = 0;

                    for (int y = 0; y < frameH; y++)
                    {
                        for (int x = 0; x < frameW; x++)
                        {
                            byte alpha = pixels[(y * frameW + x) * 4 + 3];
                            if (alpha != 0)
                            {
                                if (x < left) left = x;
                                if (x > right) right = x;
                                if (y < top) top = y;
                                if (y > bottom) bottom = y;
                            }
                        }
                    }

                    if (left < globalLeft) globalLeft = left;
                    if (right > globalRight) globalRight = right;
                    if (top < globalTop) globalTop = top;
                    if (bottom > globalBottom) globalBottom = bottom;
                }

                int spriteW = globalRight - globalLeft + 1;
                int spriteH = globalBottom - globalTop + 1;
                int offsetX = (CellWidth - spriteW) / 2 - globalLeft;
                int offsetY = (CellHeight - spriteH) / 2 - globalTop;

                // --- Étape 2 : création des frames centrées avec offsets globaux ---
                for (int i = 0; i < count; i++)
                {
                    var frameCrop = new CroppedBitmap(spriteSheet, new Int32Rect(i * frameW, row * frameH, frameW, frameH));

                    var dv = new DrawingVisual();
                    using (var dc = dv.RenderOpen())
                    {
                        dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, CellWidth, CellHeight));
                        dc.DrawImage(frameCrop, new Rect(offsetX, offsetY, frameW, frameH));
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
