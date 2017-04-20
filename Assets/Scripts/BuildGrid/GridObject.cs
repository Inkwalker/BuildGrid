using System.Collections.Generic;
using UnityEngine;

namespace BuildGrid
{
    public class GridObject : MonoBehaviour
    {
        private readonly Color gizmosBorderColor = new Color(0, 1.0f, 0, 0.5f);

        [SerializeField] private float tileSize = 1f;
        [SerializeField] private uint width = 1;
        [SerializeField] private uint height = 1;
        [SerializeField] private bool obstructive = true;

        [Space]
        [SerializeField] private Transform rotationRoot;

        public uint Width { get { return width; } }
        public uint Height { get { return height; } }
        public bool Obstructive { get { return obstructive; } }
        public List<Tile> OccupiedTiles { get { return occupiedTiles; } }

        private List<Tile> occupiedTiles;

        public Vector2 Origin
        {
            get
            {
                return new Vector2(0.5f * Width, 0.5f * Height);
            }
        }

        private void Awake()
        {
            occupiedTiles = new List<Tile>();
        }

        public void RemoveFromTiles()
        {
            var tiles = occupiedTiles.ToArray();

            foreach (var tile in tiles)
            {
                tile.RemoveObject(this);
            }
        }

        public void Rotate(bool clockwise)
        {
            if (OccupiedTiles.Count > 0)
            {
                Debug.LogWarning("Rotationg placed object. Please remove the object from the grid first.");
            }

            float angle = clockwise ? 90 : -90;

            rotationRoot.Rotate(transform.up, angle);

            uint temp = height;
            height = width;
            width = temp;
        }

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmosBorderColor;

            Vector3 offset = new Vector3(Origin.x, 0, Origin.y);

            Vector3 p0 = transform.TransformPoint(Vector3.zero - offset);
            Vector3 p1 = transform.TransformPoint(Vector3.forward * height * tileSize - offset);
            Vector3 p2 = transform.TransformPoint(Vector3.right * width * tileSize - offset);
            Vector3 p3 = transform.TransformPoint(Vector3.forward * height * tileSize + Vector3.right * width * tileSize - offset);

            Gizmos.color = gizmosBorderColor;
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p3);
            Gizmos.DrawLine(p0, p2);
            Gizmos.DrawLine(p2, p3);
        }

        #endregion
    }
}
