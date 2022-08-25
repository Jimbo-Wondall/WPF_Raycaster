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
    public static class Map
    {
        public static Rectangle[,] mapRects;
        public static Rectangle[] spriteRects;
        public static Rectangle playerRect;
        public static Line playerLine;
        public static int cellSize;
        public static int selectedWallType = 1;
        public static Canvas canvasMap = new Canvas();

        public static void Render()
        {
            int width = MainWindow.screenHeight;
            canvasMap.Height = width;
            canvasMap.Width = width;

            cellSize = width / World.width;

            mapRects = new Rectangle[World.height, World.width];
            for (int i = 0; i < World.height; i++)
            {
                for (int j = 0; j < World.width; j++)
                {
                    mapRects[i, j] = new Rectangle();
                    UpdateCell(i, j);
                    
                    mapRects[i, j].Tag = new Vector(j, i);
                    mapRects[i, j].MouseDown += Map_MouseDown;
                    mapRects[i, j].MouseEnter += Map_MouseEnter;
                    Canvas.SetLeft(mapRects[i, j], i * cellSize);
                    Canvas.SetTop(mapRects[i, j], j * cellSize);
                    canvasMap.Children.Add(mapRects[i, j]);
                }
            }

            spriteRects = new Rectangle[World.enemies.Length];
            for (int i = 0; i < spriteRects.Length; i++)
            {
                spriteRects[i] = new Rectangle()
                {
                    Height = 10,
                    Width = 10,
                    Fill = Brushes.Red
                };
                spriteRects[i].Tag = World.enemies[i].ID;
                canvasMap.Children.Add(spriteRects[i]);
            }

            playerRect = new Rectangle()
            {
                Height = 10,
                Width = 10,
                Fill = Brushes.Yellow
            };
            canvasMap.Children.Add(playerRect);
            
            playerLine = new Line()
            {
                X1 = 0, Y1 = 0,
                X2 = Player.viewVect.X, Y2 = Player.viewVect.Y,
                Stroke = Brushes.Yellow, StrokeThickness = 4
            };
            canvasMap.Children.Add(playerLine);
        }

        private static void Map_MouseEnter(object sender, MouseEventArgs e)
        {
            var mouseWasDownOn = e.Source as FrameworkElement;
            if (mouseWasDownOn != null)
            {
                Vector tag = (Vector)mouseWasDownOn.Tag;
                if (e.LeftButton.Equals(MouseButtonState.Pressed))
                {
                    ChangeCell((int)tag.Y, (int)tag.X, selectedWallType);
                }
                else if (e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    ChangeCell((int)tag.Y, (int)tag.X, 0);
                }
            }
        }

        private static void Map_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Map_MouseEnter(sender, e);
        }
        private static void ChangeCell(int y, int x, int state)
        {
            World.mapData[y, x] = state;
            UpdateCell(y, x);
        }
        private static void UpdateCell(int y, int x)
        {
            mapRects[y, x].Width = cellSize;
            mapRects[y, x].Height = cellSize;
            mapRects[y, x].Fill = World.mapData[y, x] == 0 ? Brushes.Black : OptionsPane.wallColors[World.mapData[y, x] - 1];
        }

        public static void Tick()
        {
            GetDesiredWall();

            Canvas.SetTop(playerRect, Player.position.Y * cellSize - playerRect.Height / 2);
            Canvas.SetLeft(playerRect, Player.position.X * cellSize - playerRect.Width / 2);

            Canvas.SetTop(playerLine, Player.position.Y * cellSize);
            Canvas.SetLeft(playerLine, Player.position.X * cellSize);
            playerLine.X2 = Player.viewVect.X * 20;
            playerLine.Y2 = Player.viewVect.Y * 20;

            foreach (var item in spriteRects)
            {
                Canvas.SetTop(item, World.enemies[(int)item.Tag].position.Y * cellSize - item.Height / 2);
                Canvas.SetLeft(item, World.enemies[(int)item.Tag].position.X * cellSize - item.Width / 2);
            }
        }
        private static void GetDesiredWall()
        {
            if (Keyboard.IsKeyDown(Key.NumPad1))
            {
                selectedWallType = 1;
            }
            if (Keyboard.IsKeyDown(Key.NumPad2))
            {
                selectedWallType = 2;
            }
            if (Keyboard.IsKeyDown(Key.NumPad3))
            {
                selectedWallType = 3;
            }
            if (Keyboard.IsKeyDown(Key.NumPad4))
            {
                selectedWallType = 4;
            }
            if (Keyboard.IsKeyDown(Key.NumPad5))
            {
                selectedWallType = 5;
            }
            if (Keyboard.IsKeyDown(Key.NumPad6))
            {
                selectedWallType = 6;
            }
            if (Keyboard.IsKeyDown(Key.NumPad7))
            {
                selectedWallType = 7;
            }
            if (Keyboard.IsKeyDown(Key.NumPad8))
            {
                selectedWallType = 8;
            }
            if (Keyboard.IsKeyDown(Key.NumPad9))
            {
                selectedWallType = 9;
            }
        }
    }
}