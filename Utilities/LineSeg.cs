using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander.Utilities
{
    class LineSeg
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }

        public LineSeg(Vector2 P1, Vector2 P2)
        {
            this.P1 = P1;
            this.P2 = P2;
        }

        public bool Collides(Rectangle r)
        {
            Vector2 r1 = new Vector2(r.X, r.Y);
            Vector2 r2 = new Vector2(r.X + r.Width, r.Y);
            Vector2 r3 = new Vector2(r.X + r.Width, r.Y - r.Height);
            Vector2 r4 = new Vector2(r.X, r.Y - r.Height);

            LineSeg l1 = new LineSeg(r1, r2);
            LineSeg l2 = new LineSeg(r2, r3);
            LineSeg l3 = new LineSeg(r3, r4);
            LineSeg l4 = new LineSeg(r4, r1);

            return Intersects(l1) || Intersects(l2) || Intersects(l3) || Intersects(l4);
        }

        public bool Intersects(LineSeg line)
        {
            bool intersects = false;

            Vector2 q1 = line.P1;
            Vector2 q2 = line.P2;

            int o1 = getOrientation(P1, P2, q1);
            int o2 = getOrientation(P1, P2, q2);
            int o3 = getOrientation(q1, q2, P1);
            int o4 = getOrientation(q1, q2, P2);

            if (o1 != o2 && o3 != o4)
            {
                intersects = true;
            }

            // Collinear
            if (o1 == 0 && OnSeg(q1))
            {
                intersects = true;
            }
            else if (o2 == 0 && OnSeg(q2))
            {
                intersects = true;
            }
            else if (line.OnSegment(P1))
            {
                intersects = true;
            }
            else if (line.OnSegment(P2))
            {
                intersects = true;
            }

            return intersects;
        }

        private bool OnSeg(Vector2 q)
        {
            bool onSeg = false;
            bool upperX = q.X <= Math.Max(P1.X, P2.X);
            bool lowerX = q.X >= Math.Min(P1.X, P2.X);
            bool upperY = q.X <= Math.Max(P1.Y, P2.Y);
            bool lowerY = q.X >= Math.Min(P1.Y, P2.Y);

            if (upperX && lowerX && upperY && lowerY)
            {
                onSeg = true;
            }

            return onSeg;
        }

        public bool OnSegment(Vector2 q)
        {
            bool onSeg = false;
            int orientation = getOrientation(P1, P2, q);
            if (orientation == 0)
            {
                onSeg = OnSeg(q);
            }
            return onSeg;
        }

        /**
         * @param p1
         * @param p2
         * @param p3
         * @return 0 = collinear, 1 = clockwise, 2 = counterclockwise
         */
        private int getOrientation(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            int orientation;

            float diffX1 = p2.X - p1.X;
            float diffY1 = p2.Y - p1.Y;

            float diffX2 = p3.X - p2.X;
            float diffY2 = p3.Y - p2.Y;

            float term0 = diffY1 * diffX2;
            float term1 = diffY2 * diffX1;
            float expr = term0 - term1;

            if (term0 == float.PositiveInfinity)
            {
                if (term1 == float.PositiveInfinity)
                {
                    expr = 0;
                }
                else if (term1 == float.NegativeInfinity)
                {
                    expr = 1;
                }
                else
                {
                    expr = 1;
                }
            }
            else if (term0 == float.NegativeInfinity)
            {
                if (term1 == float.PositiveInfinity)
                {
                    expr = -1;
                }
                else if (term1 == float.NegativeInfinity)
                {
                    expr = 0;
                }
                else
                {
                    expr = -1;
                }
            }
            else if (term1 == float.PositiveInfinity)
            {
                expr = -1;
            }
            else if (term1 == float.NegativeInfinity)
            {
                expr = 1;
            }

            if (expr < 0)
            {
                orientation = 2;
            }
            else if (expr > 0)
            {
                orientation = 1;
            }
            else
            {
                orientation = 0;
            }

            return orientation;
        }
    }
}
