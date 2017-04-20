using UnityEngine;

namespace BuildGrid
{
    [System.Serializable]
    public struct GridCoordinate
    {
        [SerializeField] private int x;
        [SerializeField] private int y;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public GridCoordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public GridCoordinate(Vector2 vectorPosition)
        {
            x = Mathf.FloorToInt(vectorPosition.x);
            y = Mathf.FloorToInt(vectorPosition.y);
        }

        public static GridCoordinate operator + (GridCoordinate c1, GridCoordinate c2)
        {
            return new GridCoordinate(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static GridCoordinate operator - (GridCoordinate c1, GridCoordinate c2)
        {
            return new GridCoordinate(c1.X - c2.X, c1.Y - c2.Y);
        }

        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}", X, Y);
        }
    }
}
