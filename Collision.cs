using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Galactic_Vanguard
{
    public class Collision
    {
        public static bool BoxPoint(Rectangle r, Point p)
        {
            if (p.X >= r.X && p.X < +r.X + r.Width && p.Y >= r.Y && p.Y <= r.Y + r.Height)
            {
                return true;
            }
            return false;
        }

        public static bool BoxBox(Rectangle r1, Rectangle r2)
        {
            return r1.Intersects(r2);
            if (r1.X > r2.X + r2.Width || r1.X + r1.Width < r2.X || r1.Y > r2.Y + r2.Height || r1.Y + r1.Height > r2.Y)
            {
                return false;
            }
            return true;
        }

        public static bool CirclePoint(Point c, double r, Point p)
        {
            if (Distance(c, p) <= r)
            {
                return true;
            }
            return false;
        }

        private static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static bool BoxCircle(float radius, Vector2 center, Rectangle rectangle)
        {
            // Find the closest point to the circle within the rectangle
            float closestX = MathHelper.Clamp(center.X, rectangle.Left, rectangle.Right);
            float closestY = MathHelper.Clamp(center.Y, rectangle.Top, rectangle.Bottom);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = center.X - closestX;
            float distanceY = center.Y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (radius * radius);
        }
    }
}
