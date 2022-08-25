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
    public static class Player
    {
        public static Vector viewVect;
        public static Vector position;
        public static double height;
        public static double jumpSpeed;
        public static double yPos;
        static double yVel;
        public static Vector acceleration;
        public static Vector velocity;
        static double friction = 0.9;
        static bool onGround = true;
        static bool crouching = false;
        static double maxSpeed = 0.005;

        static Player()
        {
            position.Y = 1.5;
            position.X = 1.5;
            viewVect = VectControl.FromRad(3);
            jumpSpeed = 20;
        }

        public static void Tick()
        {
            GetLookDir();
            if (onGround)
            {
                acceleration = Vector.Multiply(maxSpeed / (Convert.ToInt32(crouching) + 1), DesiredDirection());
                velocity += acceleration;

                if (velocity.Length > 0.001)
                {
                    velocity *= friction;
                }
                else
                {
                    velocity = new Vector(0, 0);
                }
            }
            else
            {
                yVel--;
                yPos += yVel;
                if (yPos <= 0)
                {
                    onGround = true;
                    yPos = 0;
                }
            }
            position += CheckCollision(position, velocity);
        }
        public static void GetLookDir()
        {
            if (Keyboard.IsKeyDown(Key.Left))
            {
                viewVect = VectControl.Rotate(viewVect, -0.05);
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                viewVect = VectControl.Rotate(viewVect, 0.05);
            }
        }
        private static Vector DesiredDirection()
        {
            if (Keyboard.IsKeyDown(Key.Space))
            {
                onGround = false;
                yVel = jumpSpeed;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                height = 0.85;
                crouching = true;
            }
            else
            {
                height = 0.7;
                crouching = false;
            }

            Vector dir = new Vector(0, 0);
            if (Keyboard.IsKeyDown(Key.W))
            {
                dir.Y--;
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                dir.X--;
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                dir.Y++;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                dir.X++;
            }
            if (!dir.Equals(new Vector(0, 0)))
            {
                dir.Normalize();
                dir = VectControl.Rotate(dir, VectControl.GetAngle(viewVect) + (Math.PI / 2));
            }
            return dir;
        }
        private static Vector CheckCollision(Vector cPos, Vector vel)
        {
            Vector nextPos = cPos + vel;
            Vector newVel = vel;
            try
            {
                if (World.mapData[(int)cPos.X, (int)nextPos.Y] > 0)
                {
                    newVel.Y = 0;
                }
                if (World.mapData[(int)nextPos.X, (int)cPos.Y] > 0)
                {
                    newVel.X = 0;
                }
            }
            catch (Exception) { }

            if ((int)nextPos.X < 0 || (int)nextPos.X > World.width)
            {
                newVel.X = 0;
            }
            if ((int)nextPos.Y < 0 || (int)nextPos.Y > World.height)
            {
                newVel.Y = 0;
            }
            return newVel;
        }
    }
}
