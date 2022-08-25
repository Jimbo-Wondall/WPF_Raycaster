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
    public static class World
    {
        public static int height;
        public static int width;
        public static int[,] mapData;
        public static Sprite[] enemies;
        public static int wallAmount = 0;
        public static Wall[] wallTypes;

        public static void New(int[,] inputMap)
        {
            mapData = inputMap;
            height = mapData.GetLength(0);
            width = mapData.GetLength(1);

            List<Vector> enemyList = new List<Vector>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (mapData[i,j] == -1)
                    {
                        enemyList.Add(new Vector(i + 0.5, j + 0.5));
                        mapData[i, j] = 0;
                    }
                }
            }
            enemies = new Sprite[enemyList.Count];
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemies[i] = new Sprite(enemyList[i].X, enemyList[i].Y, i);
            }

            foreach (var item in mapData)
            {
                wallAmount = item > wallAmount ? item : wallAmount;
            }

            wallTypes = new Wall[wallAmount];
            for (int i = 0; i < wallAmount; i++)
            {
                wallTypes[i] = new Wall();
                wallTypes[i].Randomise();

            }
        }
        public static void Tick()
        {
            foreach (var item in enemies)
            {
                item.Tick();
            }
        }
        public static void LoadFromFile(string filepath)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filepath);
                mapData = new int[lines.Length, lines[0].Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    for (int j = 0; j < lines[0].Length; j++)
                    {
                        mapData[i, j] = Convert.ToInt32(lines[i][j].ToString());
                    }
                }
            }
            catch (Exception)
            {
                //invalid
            }
        }
    }
    public class Wall
    {
        public double height;
        public SolidColorBrush colour;
        Random rand = new Random();

        public void Randomise()
        {
            SolidColorBrush c = new SolidColorBrush();
            c.Color = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
            colour = c;
            height = rand.Next(15) / 10;
        }
    }
}
