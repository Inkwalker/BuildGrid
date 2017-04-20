using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BuildGrid
{
    public class Grid : MonoBehaviour
    {
        private readonly Color gizmosGridColor     = new Color(1.0f, 1.0f, 1.0f, 0.15f);
        private readonly Color gizmosBorderColor   = new Color(   0,    0, 1.0f, 0.5f);
        private readonly Color gizmosOccupiedColor = new Color(   0, 1.0f,    0, 0.15f);

        [SerializeField] private uint width = 10;
        [SerializeField] private uint height = 10;
        [SerializeField] private float tileSize = 1f;

        public uint Width { get { return width; } }
        public uint Height { get { return height; } }

        public Tile this[int x, int y]
        {
            get
            {
                if (IsValidPosition(x, y))
                {
                    return tiles[x, y];
                }
                else
                {
                    return null;
                }
            }
        }

        public Tile this[GridCoordinate pos]
        {
            get
            {
                return this[pos.X, pos.Y];
            }
        }

        public GridObjectEvent ObjectPlaced;
        public GridObjectEvent ObjectRemoved;

        private Tile[,] tiles;
        private List<GridObject> objects;

        private void Awake()
        {
            objects = new List<GridObject>();
            tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = new Tile(x, y);
                }
            }
        }

        public bool CanPlaceObject(Vector2 localPosition, GridObject obj)
        {
            Vector2 zeroPos = localPosition - obj.Origin + new Vector2(tileSize * 0.5f, tileSize * 0.5f);
            GridCoordinate zeroTilePositon = LocalToGrid(zeroPos);

            for (int x = 0; x < obj.Width; x++)
            {
                for (int y = 0; y < obj.Height; y++)
                {
                    int tileX = zeroTilePositon.X + x;
                    int tileY = zeroTilePositon.Y + y;

                    if (IsValidPosition(tileX, tileY) == false) return false;
                    if (tiles[tileX, tileY].CanAddObject(obj) == false) return false;
                }
            }

            return true;
        }

        public bool CanPlaceObject(Vector3 worldPosition, GridObject obj)
        {
            Vector2 local = WorldToLocal(worldPosition);
            return CanPlaceObject(local, obj);
        }

        public bool PlaceObject(Vector2 localPosition, GridObject obj)
        {
            Vector2 zeroPos = localPosition - obj.Origin + new Vector2(tileSize * 0.5f, tileSize * 0.5f);
            GridCoordinate zeroTilePositon = LocalToGrid(zeroPos);

            if (CanPlaceObject(localPosition, obj))
            {
                for (int x = 0; x < obj.Width; x++)
                {
                    for (int y = 0; y < obj.Height; y++)
                    {
                        int tileX = zeroTilePositon.X + x;
                        int tileY = zeroTilePositon.Y + y;

                        tiles[tileX, tileY].AddObject(obj);
                    }
                }

                obj.transform.parent = transform;

                objects.Add(obj);
                OnObjectPlaced(obj);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PlaceObject(Vector3 worldPosition, GridObject obj)
        {
            Vector2 local = WorldToLocal(worldPosition);
            return PlaceObject(local, obj);
        }

        public bool RemoveObject(GridObject obj)
        {
            if (objects.Remove(obj))
            {
                obj.RemoveFromTiles();

                obj.transform.parent = null;

                OnObjectRemoved(obj);

                return true;
            }
            else
            {
                return false;
            }
        }

        public Tile[] GetNeighbors(Tile tile, bool returnDiagonal)
        {
            List<Tile> result = new List<Tile>();

            for (int x = tile.X - 1; x < tile.X + 2; x++)
            {
                for (int y = tile.Y - 1; y < tile.Y + 2; y++)
                {
                    if (IsValidPosition(x, y) == false) continue;
                    if (x == tile.X && y == tile.Y) continue;
                    if ((x != tile.X && y != tile.Y) && returnDiagonal == false) continue;

                    result.Add(tiles[x, y]);
                }
            }

            return result.ToArray();
        }

        #region Coordinates

        public bool IsValidPosition(int x, int y)
        {
            return
                x >= 0 && x < Width &&
                y >= 0 && y < Height;
        }

        public bool IsValidPosition(GridCoordinate pos)
        {
            return IsValidPosition(pos.X, pos.Y);
        }

        public Vector2 SnapObjectLocalPosition(Vector2 localPosition, GridObject obj)
        {
            Vector2 halfTile = new Vector2(tileSize * 0.5f, tileSize * 0.5f);
            Vector2 corner = localPosition - obj.Origin + halfTile;
            GridCoordinate cornerGridPos = LocalToGrid(corner);
            Vector2 cornerSnapPos = GridToLocal(cornerGridPos) - halfTile;

            return cornerSnapPos + obj.Origin;
        }

        public Vector3 SnapObjectWorldPosition(Vector3 worldPosition, GridObject obj)
        {
            Vector2 local = WorldToLocal(worldPosition);
            Vector2 localSnapped = SnapObjectLocalPosition(local, obj);
            Vector3 worldSnapped = LocalToWorld(localSnapped);

            return worldSnapped;
        }

        public GridCoordinate LocalToGrid(Vector2 position)
        {
            int x = Mathf.FloorToInt(position.x / tileSize);
            int y = Mathf.FloorToInt(position.y / tileSize);

            return new GridCoordinate(x, y);
        }

        public Vector2 GridToLocal(GridCoordinate position)
        {
            float offset = 0.5f;
            Vector2 localPosition = new Vector2(
                (position.X + offset) * tileSize,
                (position.Y + offset) * tileSize
            );

            return localPosition;
        }

        public GridCoordinate WorldToGrid(Vector3 position)
        {
            Vector2 local = WorldToLocal(position);
            return LocalToGrid(local);
        }

        public Vector3 GridToWorld(int x, int y)
        {
            float offset = tileSize * 0.5f;
            Vector3 localPosition = new Vector3(x + offset, 0, y + offset);
            Vector3 worldPosition = transform.TransformPoint(localPosition);

            return worldPosition;
        }

        public Vector3 GridToWorld(GridCoordinate pos)
        {
            return GridToWorld(pos.X, pos.Y);
        }

        public Vector2 WorldToLocal(Vector3 position)
        {
            Vector3 local = transform.InverseTransformPoint(position);
            return new Vector2(local.x, local.z);
        }

        public Vector3 LocalToWorld(Vector2 localPosition)
        {
            Vector3 local3D = new Vector3(localPosition.x, 0, localPosition.y);
            Vector3 worldPosition = transform.TransformPoint(local3D);

            return worldPosition;
        }

        #endregion

        #region Events

        protected virtual void OnObjectPlaced(GridObject obj)
        {
            if (ObjectPlaced != null)
            {
                ObjectPlaced.Invoke(obj);
            }
        }

        protected virtual void OnObjectRemoved(GridObject obj)
        {
            if (ObjectRemoved != null)
            {
                ObjectRemoved.Invoke(obj);
            }
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            Vector3 p0 = transform.TransformPoint(Vector3.zero);
            Vector3 p1 = transform.TransformPoint(Vector3.forward * height * tileSize);
            Vector3 p2 = transform.TransformPoint(Vector3.right * width * tileSize);
            Vector3 p3 = transform.TransformPoint(Vector3.forward * height * tileSize + Vector3.right * width * tileSize);

            Gizmos.color = gizmosBorderColor;
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p3);
            Gizmos.DrawLine(p0, p2);
            Gizmos.DrawLine(p2, p3);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmosOccupiedColor;

            //Draw occupied tiles
            if (tiles != null)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (tiles[x, y].Occupied)
                        {
                            Gizmos.DrawCube(GridToWorld(x, y), new Vector3(tileSize, 0.05f, tileSize));
                        }
                    }
                }
            }

            Gizmos.color = gizmosGridColor;
            //Draw vertical grid lines
            for (int x = 1; x < width; x++)
            {
                Vector3 p0 = transform.TransformPoint(new Vector3(x * tileSize, 0, 0));
                Vector3 p1 = transform.TransformPoint(new Vector3(x * tileSize, 0, height * tileSize));

                Gizmos.DrawLine(p0, p1);
            }

            //Draw horizontal grid lines
            for (int y = 1; y < height; y++)
            {
                Vector3 p0 = transform.TransformPoint(new Vector3(0, 0, y * tileSize));
                Vector3 p1 = transform.TransformPoint(new Vector3(width * tileSize, 0, y * tileSize));

                Gizmos.DrawLine(p0, p1);
            }
        }

        #endregion

        [Serializable] public class GridObjectEvent : UnityEvent<GridObject> { }

    }
}
