using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace WPF_Raycaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dTimer;
        public static Canvas canvasScreen;
        public static int screenHeight = 600;
        public static int screenWidth = 800;
        

        public MainWindow()
        {
            InitializeComponent();
            int[,] defaultMap = new int[,]
            {
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,7,7,7,7,7,7,7},
                {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
                {4,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
                {4,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
                {4,0,3,0,0,0,0,0,0,0,0,0,0,0,0,-1,7,0,0,0,0,0,0,7},
                {4,0,4,0,0,0,0,5,5,5,5,5,5,5,5,5,7,7,0,7,7,7,7,7},
                {4,0,5,0,0,0,0,5,0,5,0,5,0,5,0,5,7,0,0,0,7,7,7,1},
                {4,0,6,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
                {4,0,7,0,0,0,0,0,-1,0,0,0,0,0,0,0,0,0,0,0,7,7,7,1},
                {4,0,8,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
                {4,0,0,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,7,7,7,1},
                {4,0,0,0,0,0,0,5,5,5,5,0,5,5,5,5,7,7,7,7,7,7,7,1},
                {6,6,6,6,6,6,6,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
                {8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                {6,6,6,6,6,6,0,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
                {4,4,4,4,4,4,0,4,4,4,6,-1,6,2,2,2,2,2,2,2,3,3,3,3},
                {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
                {4,0,0,0,0,0,0,0,0,0,0,0,6,2,0,0,5,0,0,2,0,0,0,2},
                {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
                {4,0,6,0,6,0,0,0,0,4,6,0,0,0,0,0,5,0,0,0,0,0,0,2},
                {4,0,0,5,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
                {4,0,6,0,6,0,0,0,0,4,6,0,6,2,0,0,5,0,0,2,0,0,0,2},
                {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,-1,2},
                {4,4,4,4,4,4,4,4,4,4,1,1,1,2,2,2,2,2,2,3,3,3,3,3}
            };


            World.New(defaultMap);

            OptionsPane.Render(screenHeight, 100);
            Grid.SetColumn(OptionsPane.canvasOptionsPane, 2);
            gridWindow.Children.Add(OptionsPane.canvasOptionsPane);

            canvasScreen = Screen.Render(screenHeight, screenWidth);
            Grid.SetColumn(canvasScreen, 0);
            gridWindow.Children.Add(canvasScreen);

            Map.Render();
            Grid.SetColumn(Map.canvasMap, 1);
            gridWindow.Children.Add(Map.canvasMap);

            StartGame();
        }

        void StartGame()
        {
            dTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 10) };
            dTimer.Tick += DTimer_Tick;
            dTimer.Start();
        }

        private void DTimer_Tick(object sender, EventArgs e)
        {
            Player.Tick();
            Map.Tick();
            World.Tick();
            Screen.Tick();
        }
    }
}
