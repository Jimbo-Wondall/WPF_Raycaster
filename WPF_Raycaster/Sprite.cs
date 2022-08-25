using System.Collections.Generic;
using System;
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
    public class Sprite
    {
        public int ID;
        public Vector position;
        public bool inView;
        public Image image;
        public double imageRatio;
        public double desiredHeight = 0.2;
        
        public double viewDir;
        public Vector viewVect;
        Vector velocity;
        double friction = 0.9;
        double maxSpeed = 0.05;

        public Sprite(double x, double y, int id)
        {
            ID = id;
            position.Y = y;
            position.X = x;
            
            image = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\kingt\source\repos\WPF_Raycaster\WPF_Raycaster\Images\enemy_sprite_1.png", UriKind.Absolute))
            };
            imageRatio = image.Width / image.Height;
        }

        public void Tick()
        {
            inView = Math.Abs(Vector.AngleBetween(position - Player.position, Player.viewVect) * (Math.PI / 180)) < Screen.fieldOfView / 2;
            
            viewVect = Player.position - position;
            viewVect.Normalize();
            if (true)
            {
                velocity += viewVect * (maxSpeed / 30);
            }
            velocity = (velocity.Length > 0.001) ? velocity *= friction : new Vector(0, 0);
            position += CheckCollision(position, velocity);
        }
        private static Vector CheckCollision(Vector cPos, Vector vel)
        {
            Vector nextPos = cPos + vel;
            Vector newVel = vel;
            if (World.mapData[(int)cPos.X, (int)nextPos.Y] > 0)
            {
                newVel.Y = 0;
            }
            if (World.mapData[(int)nextPos.X, (int)cPos.Y] > 0)
            {
                newVel.X = 0;
            }
            return newVel;
        }
    }
}
