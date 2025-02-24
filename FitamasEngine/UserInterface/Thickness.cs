using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public struct Thickness
    {
        private static Thickness zero = new Thickness(0);

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public static Thickness Zero => zero;

        public Thickness(int uniformLength)
        {
            Left = Top = Right = Bottom = uniformLength;
        }

        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public bool Equals(Thickness other)
        {
            if (Left == other.Left && Top == other.Top && Right == other.Right)
            {
                return Bottom == other.Bottom;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Thickness)
            {
                return this == (Thickness)obj;
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(64);
            stringBuilder.AppendFormat("{0}, {1}, {2}, {3}", Left, Top, Right, Bottom);
            return stringBuilder.ToString();
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
        }

        public static bool operator ==(Thickness t1, Thickness t2)
        {
            return (t1.Left == t2.Left) &&
                   (t1.Top == t2.Top) &&
                   (t1.Right == t2.Right) &&
                   (t1.Bottom == t2.Bottom);
        }

        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return !(t1 == t2);
        }
    }
}
