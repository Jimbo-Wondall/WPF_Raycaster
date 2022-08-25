using System;
using System.Collections.Generic;
using System.IO;
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
    public class OptionsPane
    {
        public static List<SolidColorBrush> wallColors = new List<SolidColorBrush>();
        public static List<Rectangle> wallSwatches = new List<Rectangle>();
        public static Canvas canvasOptionsPane;
        static Button buttonNewSwatch;
        static Button buttonLoadMap;
        static Button buttonSaveMap;
        static TextBox txtboxFilePath;
        static Random rand = new Random();
        public static void Render(int height, int width)
        {
            canvasOptionsPane = new Canvas
            {
                Height = height,
                Width = width
            };
            CreateButtons();
            txtboxFilePath = new TextBox
            {
                Width = 100
            };
            Canvas.SetLeft(txtboxFilePath, 0);
            Canvas.SetBottom(txtboxFilePath, 60);
            buttonNewSwatch.Click += NewSwatch;
            canvasOptionsPane.Children.Add(txtboxFilePath);
            GenerateRandomColours(World.wallAmount);
        }
        private static void CreateButtons()
        {
            buttonNewSwatch = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Content = "New swatch"
            };
            Canvas.SetLeft(buttonNewSwatch, 0);
            Canvas.SetBottom(buttonNewSwatch, 40);
            buttonNewSwatch.Click += NewSwatch;
            canvasOptionsPane.Children.Add(buttonNewSwatch);

            buttonLoadMap = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Content = "Load map"
            };
            Canvas.SetLeft(buttonLoadMap, 0);
            Canvas.SetBottom(buttonLoadMap, 20);
            buttonLoadMap.Click += LoadMap;
            canvasOptionsPane.Children.Add(buttonLoadMap);

            buttonSaveMap = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Content = "Save map"
            };
            Canvas.SetLeft(buttonSaveMap, 0);
            Canvas.SetBottom(buttonSaveMap, 0);
            buttonSaveMap.Click += SaveMap;
            canvasOptionsPane.Children.Add(buttonSaveMap);
        }
        private static void SaveMap(object sender, RoutedEventArgs e)
        {
            string fileName;
            try
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                int c = 0;
                do
                {
                    c++;
                    fileName = "Map" + c + ".txt";
                } while (File.Exists(System.IO.Path.Combine(docPath, fileName)));

                StreamWriter streamWriter = new StreamWriter(System.IO.Path.Combine(docPath, fileName));
                string output = "";
                for (int i = 0; i < World.height; i++)
                {
                    for (int j = 0; j < World.width; j++)
                    {
                        output += World.mapData[i, j];
                        if (j != World.width - 1)
                        {
                            output += ",";
                        }
                    }
                    streamWriter.WriteLine(output);
                    output = "";
                }
                streamWriter.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        private static void LoadMap(object sender, RoutedEventArgs e)
        {
            string filePath = txtboxFilePath.Text.Trim(new char[] { ' ', '"', '.' });
            string[] lines = File.ReadAllLines(@$"{filePath}");

            int height = lines.Length;
            int width = lines[0].Split(",").Length;

            int[,] loadedMap = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                string[] splitLine = lines[i].Split(",");
                int[] separatedInts = Array.ConvertAll(splitLine, s => int.Parse(s));
                for (int j = 0; j < width; j++)
                {
                    loadedMap[i, j] = separatedInts[j];
                }
            }
            World.New(loadedMap);
            Map.Render();
        }
        private static void NewSwatch(object sender, RoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            ColourPicker picker = new ColourPicker(wallSwatches.Count + 1);
            picker.Show();

        }
        private static void SwatchMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            int id = (int)rect.Tag;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                wallSwatches[Map.selectedWallType - 1].StrokeThickness = 0;
                Map.selectedWallType = id + 1;
                wallSwatches[id].Stroke = Brushes.Black;
                wallSwatches[id].StrokeThickness = 5;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ColourPicker picker = new ColourPicker(id);
                picker.Show();
            }
        }
        public static void GenerateRandomColours(int amount)
        {
            wallColors.Clear();
            for (int i = 0; i < amount; i++)
            {
                SolidColorBrush c = new SolidColorBrush();
                c.Color = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
                //c.Color = i == 0 ? Color.FromRgb(0, 0, 0) : Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
                NewColour(c);
            }
        }
        public static void UpdateColour(int swatch, SolidColorBrush colour)
        {
            wallColors[swatch] = colour;
            wallSwatches[swatch].Fill = colour;
        }
        public static void NewColour(SolidColorBrush colour)
        {
            wallColors.Add(colour);
            Rectangle newRect = NewSwatch(colour);
            wallSwatches.Add(newRect);
            canvasOptionsPane.Children.Add(newRect);
        }
        public static Rectangle NewSwatch(SolidColorBrush colour)
        {
            Rectangle rect = new Rectangle()
            {
                Height = 50,
                Width = 50,
                Fill = colour,
                Tag = wallSwatches.Count
            };
            Canvas.SetTop(rect, wallSwatches.Count * rect.Height);
            Canvas.SetLeft(rect, 0);
            rect.MouseDown += SwatchMouseDown;
            return rect;
        }
    }
}
