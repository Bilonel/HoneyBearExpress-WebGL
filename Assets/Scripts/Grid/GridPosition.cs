using System;

namespace HoneyBearExpress.Grid
{
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        public int X { get; }
        public int Y { get; }
        
        public static readonly GridPosition North = new GridPosition(0, 1);
        public static readonly GridPosition South = new GridPosition(0, -1);
        public static readonly GridPosition East = new GridPosition(1, 0);
        public static readonly GridPosition West = new GridPosition(-1, 0);
        
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static bool operator ==(GridPosition a, GridPosition b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        
        public static bool operator !=(GridPosition a, GridPosition b)
        {
            return !(a == b);
        }
        
        public static GridPosition operator +(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.X + b.X, a.Y + b.Y);
        }
        
        public bool Equals(GridPosition other)
        {
            return this == other;
        }
        
        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
        
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
