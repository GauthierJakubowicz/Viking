using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VikingGame
{
    public class Personnage
    {
        public string Name { get; private set; }
        public AnimationManager Animator { get; private set; }

        public Personnage(
            string name,
            string spriteSheetFile,
            Image targetImage,
            Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)> animationMap)
        {
            Name = name;

            var spriteSheet = new BitmapImage();
            spriteSheet.BeginInit();
            spriteSheet.UriSource = new Uri($"pack://application:,,,/Viking;component/img/{spriteSheetFile}");
            spriteSheet.CacheOption = BitmapCacheOption.OnLoad;
            spriteSheet.CreateOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreImageCache;
            spriteSheet.EndInit();

            Animator = new AnimationManager(targetImage, spriteSheet, animationMap);
        }

        public void JouerAnimation(string animationName) => Animator.Play(animationName);

        public bool FacingRight
        {
            get => Animator.FacingRight;
            set => Animator.FacingRight = value;
        }

        // Factory methods pour créer les différents personnages
        public static Personnage CreerViking(Image targetImage)
        {
            var animationMap = new Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)>
            {
                { "Idle", (0, 8, 124, 84) },
                { "Walk", (1, 8, 124, 84) },
                { "Run", (2, 8, 124, 84) },
                { "Slide", (3, 7, 124, 84) },
                { "Crouch", (4, 2, 124, 84) },
                { "CrouchAttack", (5, 5, 124, 84) },
                { "Jump", (6, 1, 124, 84) },
                { "JumpToFall", (7, 3, 124, 84) },
                { "Fall", (8, 1, 124, 84) },
                { "JumpAttack", (9, 6, 124, 84) },
                { "IdleBlock", (10, 8, 124, 84) },
                { "Block", (11, 5, 124, 84) },
                { "Attack1", (12, 4, 124, 84) },
                { "Attack2", (13, 4, 124, 84) },
                { "Attack3", (14, 4, 124, 84) },
                { "Spell", (15, 12, 124, 84) },
                { "SpellSlam", (16, 11, 124, 84) },
                { "LadderClimb", (17, 8, 124, 84) },
                { "WallHang", (18, 6, 124, 84) },
                { "WallClimb", (19, 5, 124, 84) },
                { "TransformationOut", (20, 8, 124, 84) },
                { "Dash", (21, 5, 124, 84) },
                { "Hit", (22, 4, 124, 84) },
                { "Death", (23, 12, 124, 84) }
            };

            return new Personnage("Viking", "Viking-Sheet.png", targetImage, animationMap);
        }

        public static Personnage CreerSpearwoman(Image targetImage)
        {
            var animationMap = new Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)>
            {
                { "Idle", (0, 8, 124, 84) },
                { "Walk", (1, 8, 124, 84) },
                { "Run", (2, 8, 124, 84) },
                { "Slide", (3, 8, 124, 84) },
                { "Jump", (4, 3, 124, 84) },
                { "Fall", (5, 3, 124, 84) },
                { "Attack1", (6, 5, 124, 84) },
                { "Attack2", (7, 4, 124, 84) },
                { "Attack3", (8, 5, 124, 84) },
                { "JumpAttack", (9, 11, 124, 84) },
                { "Block", (10, 4, 124, 84) },
                { "Hit", (11, 4, 124, 84) },
                { "Death", (12, 4, 124, 84) }
            };

            return new Personnage("Spearwoman", "Spearwoman-Sheet.png", targetImage, animationMap);
        }

        public static Personnage CreerFireWarrior(Image targetImage)
        {
            var animationMap = new Dictionary<string, (int row, int frameCount, int frameWidth, int frameHeight)>
            {
                { "Idle", (0, 8, 124, 84) },
                { "Walk", (1, 8, 124, 84) },
                { "Run", (2, 8, 124, 84) },
                { "Slide", (3, 7, 124, 84) },
                { "Crouch", (4, 6, 124, 84) },
                { "Jump", (5, 3, 124, 84) },
                { "Fall", (6, 3, 124, 84) },
                { "Attack1", (7, 5, 124, 84) },
                { "Attack2", (8, 4, 124, 84) },
                { "Attack3", (9, 6, 124, 84) },
                { "JumpAttack", (10, 13, 124, 84) },
                { "Block", (11, 4, 124, 84) },
                { "Hit", (12, 4, 124, 84) },
                { "Death", (13, 12, 124, 84) }
            };

            return new Personnage("Fire Warrior", "Fire_warrior-Sheet.png", targetImage, animationMap);
        }
    }
}