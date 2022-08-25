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

namespace WPF_Raycaster
{
    public static class Screen
    {
        public static int resolution = 100;
        public static double resAngle;
        public static double screenHeight;
        public static double screenWidth;
        public static double screenWidthHalf;
        public static double fieldOfView = 1;
        public static double yOffset;
        public static int renderDistance = 30;
        public static Rectangle[] screenRects;
        public static Image[] spriteImages;
        public static Canvas canvasScreen;
        static Random rand = new Random();

        public static Canvas Render(int height, int width)
        {
            screenHeight = height;
            screenWidth = width;
            resAngle = fieldOfView / resolution;
            screenWidthHalf = screenWidth / 2;
            canvasScreen = new Canvas()
            {
                Height = screenHeight,
                Width = screenWidth
            };
            InitRects();
            return canvasScreen;
        }
        static void InitRects()
        {
            Rectangle floorRect = new Rectangle()
            {
                Width = screenWidth,
                Height = screenHeight / 2,
                Fill = Brushes.Black
            };
            Canvas.SetTop(floorRect, screenHeight / 2);
            Canvas.SetLeft(floorRect, 0);
            Canvas.SetZIndex(floorRect, 0);
            canvasScreen.Children.Add(floorRect);

            screenRects = new Rectangle[resolution];
            for (int i = 0; i < resolution; i++)
            {
                screenRects[i] = new Rectangle()
                {
                    Width = screenWidth / resolution,
                    Fill = Brushes.Black
                };
                Canvas.SetLeft(screenRects[i], i * (screenWidth / resolution));
                Canvas.SetZIndex(screenRects[i], 1);
                SetColHeight(i, rand.Next(50) + 10, 0, 1);
                canvasScreen.Children.Add(screenRects[i]);
            }

            spriteImages = new Image[World.enemies.Length];
            for (int i = 0; i < spriteImages.Length; i++)
            {
                spriteImages[i] = World.enemies[i].image;
                Canvas.SetBottom(spriteImages[i], 0);
                Canvas.SetLeft(spriteImages[i], 0);
                Canvas.SetZIndex(spriteImages[i], 1000);
                canvasScreen.Children.Add(spriteImages[i]);
            }
        }

        public static void Tick()
        {
            yOffset = Player.height - Player.height * (Player.yPos / ((Player.jumpSpeed * (Player.jumpSpeed + 1)) / 2));
            double resAngle = fieldOfView / resolution;
            for (int i = 0; i < resolution; i++)
            {
                double angleDiff = (i - (resolution / 2)) * resAngle;
                Vector rayDir = VectControl.Rotate(Player.viewVect, angleDiff);
                
                int mapX = (int)Player.position.X;
                int mapY = (int)Player.position.Y;

                double deltaDistX = Math.Abs(1 / rayDir.X);
                double deltaDistY = Math.Abs(1 / rayDir.Y);

                double sideDistX = (rayDir.X < 0) ? (Player.position.X - mapX) * deltaDistX : (mapX + 1.0 - Player.position.X) * deltaDistX;
                double sideDistY = (rayDir.Y < 0) ? (Player.position.Y - mapY) * deltaDistY : (mapY + 1.0 - Player.position.Y) * deltaDistY;

                double perpWallDist;

                int stepX = (rayDir.X < 0) ? -1 : 1;
                int stepY = (rayDir.Y < 0) ? -1 : 1;

                bool missed = false;
                bool hit = false;
                int side = 0;

                while (!hit)
                {
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaDistX;
                        mapX += stepX;
                        side = 0;
                    }
                    else
                    {
                        sideDistY += deltaDistY;
                        mapY += stepY;
                        side = 1;
                    }
                    if (mapX >= 0 && mapX < World.width &&
                        mapY >= 0 && mapY < World.height 
                        //&& (sideDistX < renderDistance || sideDistY < renderDistance)
                        )
                    {
                        if (World.mapData[mapX, mapY] > 0)
                        {
                            hit = true;

                        }
                    }
                    else
                    {
                        missed = true;
                        break;
                    }
                }
                if (missed)
                {
                    SetColHeight(i, 0, 0, 0);
                }
                else
                {
                    if (side == 0)
                    {
                        perpWallDist = (mapX - Player.position.X + ((1 - stepX) / 2)) / rayDir.X;
                    }
                    else
                    {
                        perpWallDist = (mapY - Player.position.Y + ((1 - stepY) / 2)) / rayDir.Y;
                    }
                    perpWallDist *= Math.Cos(angleDiff);
                    double columnHeight = screenHeight / perpWallDist;
                    SetColHeight(i, columnHeight, perpWallDist, World.wallTypes[World.mapData[mapX, mapY] - 1].height);
                    SetColColor(i, side, World.mapData[mapX, mapY] - 1, (int)(perpWallDist * 10));
                }
            }
            DrawSprites();
        }
        static void SetColHeight(int i, double height, double dist, double wallHeight)
        {
            height /= 2;
            double midPoint     = (screenHeight / 2) - (height * yOffset);
            double colBottom    = midPoint + height;
            double colTop       = midPoint - height * wallHeight;

            //double colTop = (screenHeight / 2) - ((height / 2) * yOffset);
            //double colBottom = colTop + height;
            //colTop -= wallHeight;

            if (colTop < 0)                 { colTop = 0; }
            if (colBottom > screenHeight)   { colBottom = screenHeight; }

            screenRects[i].Height = colBottom - colTop;
            Canvas.SetTop       (screenRects[i], colTop);
            Canvas.SetZIndex    (screenRects[i], 1000 - (int)(dist * 10));
        }
        static void SetColColor(int i, int side, int wallType, int depth)
        {
            SolidColorBrush color = new SolidColorBrush();
            int shadow = side * 5;

            int R = World.wallTypes[wallType].colour.Color.R - depth - shadow;
            int G = World.wallTypes[wallType].colour.Color.G - depth - shadow;
            int B = World.wallTypes[wallType].colour.Color.B - depth - shadow;

            R = (R > 255) ? 255 : R;    R = (R < 0) ? 0 : R;
            G = (G > 255) ? 255 : G;    G = (G < 0) ? 0 : G;
            B = (B > 255) ? 255 : B;    B = (B < 0) ? 0 : B;
            color.Color = Color.FromRgb((byte)R, (byte)G, (byte)B);
            screenRects[i].Fill = color;
        }

        static void DrawSprites()
        {
            Vector relativePos;
            double angle;
            double distance;
            double size;
            foreach (var item in World.enemies)
            {
                if (item.inView)
                {
                    relativePos = item.position - Player.position;
                    angle = Vector.AngleBetween(Player.viewVect, relativePos) * (Math.PI / 180);
                    distance = Math.Cos(angle) * relativePos.Length;
                    double maxHeight = (screenHeight / 2) - (screenHeight / distance * yOffset);
                    size = screenHeight / distance * World.enemies[item.ID].desiredHeight;

                    spriteImages[item.ID].Visibility = Visibility.Visible;
                    spriteImages[item.ID].Height = size;
                    spriteImages[item.ID].Width = size * World.enemies[item.ID].imageRatio;

                    Canvas.SetTop(spriteImages[item.ID], maxHeight + ((screenHeight / distance) - size));
                    Canvas.SetLeft(spriteImages[item.ID], Math.Abs(angle + (fieldOfView / 2)) * screenWidth);
                    Canvas.SetZIndex(spriteImages[item.ID], 1000 - (int)(distance * 10));
                }
                else
                {
                    spriteImages[item.ID].Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
