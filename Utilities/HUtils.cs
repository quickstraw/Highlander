using Microsoft.Xna.Framework;
using System;

namespace Highlander.Utilities
{
    public class HUtils
    {
        // based on the math here:
        // http://math.stackexchange.com/a/1367732
        // based on the code from:
        // https://gist.github.com/jupdike/bfe5eb23d1c395d8a0a1a4ddd94882ac
        public static int Intersect(Vector2 circleA, float radiusA, Vector2 circleB, float radiusB, out Vector2[] intersections)
        {

            float centerDx = circleA.X - circleB.X;
            float centerDy = circleB.Y - circleB.Y;
            float r = (float) Math.Sqrt(centerDx * centerDx + centerDy * centerDy);

            // no intersection
            if (!(Math.Abs(radiusA - radiusB) <= r && r <= radiusA + radiusB))
            {
                intersections = new Vector2[0];
                return 0;
            }

            float r2d = r * r;
            float r4d = r2d * r2d;
            float rASquared = radiusA * radiusA;
            float rBSquared = radiusB * radiusB;
            float a = (rASquared - rBSquared) / (2 * r2d);
            float r2r2 = (rASquared - rBSquared);
            float c = (float) Math.Sqrt(2 * (rASquared + rBSquared) / r2d - (r2r2 * r2r2) / r4d - 1);

            float fx = (circleA.X + circleB.X) / 2 + a * (circleB.X - circleA.X);
            float gx = c * (circleB.Y - circleA.Y) / 2;
            float ix1 = fx + gx;
            float ix2 = fx - gx;

            float fy = (circleA.Y + circleB.Y) / 2 + a * (circleB.Y - circleA.Y);
            float gy = c * (circleA.X - circleB.X) / 2;
            float iy1 = fy + gy;
            float iy2 = fy - gy;

            // if gy == 0 and gx == 0 then the circles are tangent and there is only one solution
            if (Math.Abs(gx) < float.Epsilon && Math.Abs(gy) < float.Epsilon)
            {
                intersections = new[] {
                    new Vector2(ix1, iy1)
                };
                return 1;
            }

            intersections = new[] {
                new Vector2(ix1, iy1),
                new Vector2(ix2, iy2),
            };
            return 2;
        }

    }
}
