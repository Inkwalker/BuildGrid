using UnityEngine;
using System.Collections.Generic;
using System;

namespace BuildGrid
{
    public class Tile
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public GridObject[] Objects { get { return objects.ToArray(); } }
        public bool Occupied { get { return objects.Count > 0; } }

        public event Action<GridObject> ObjectAdded;
        public event Action<GridObject> ObjectRemoved;

        private List<GridObject> objects = new List<GridObject>();

        public Tile(Vector2 position)
        {
            X = Mathf.FloorToInt(position.x);
            Y = Mathf.FloorToInt(position.y);
        }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool AddObject(GridObject obj)
        {
            if (CanAddObject(obj))
            {
                objects.Add(obj);
                obj.OccupiedTiles.Add(this);

                OnObjectAdded(obj);
                return true;
            }

            return false;
        }

        public bool RemoveObject(GridObject obj)
        {
            if (objects.Remove(obj))
            {
                obj.OccupiedTiles.Remove(this);

                OnObjectRemoved(obj);
                return true;
            }

            return false;
        }

        public void RemoveAllObjects()
        {
            var objectsToRemove = Objects;

            for (int i = 0; i < objectsToRemove.Length; i++)
            {
                RemoveObject(objectsToRemove[i]);
            }
        }

        public bool Contains(GridObject obj)
        {
            return objects.Contains(obj);
        }

        public bool CanAddObject(GridObject obj)
        {
            if (Contains(obj)) return false;
            if (obj.Obstructive)
            {
                bool tileObstructed = false;

                for (int i = 0; i < objects.Count; i++)
                {
                    tileObstructed = objects[i].Obstructive;

                    if (tileObstructed) break;
                }

                return !tileObstructed;
            }

            return true;            
        }

        protected void OnObjectAdded(GridObject obj)
        {
            if (ObjectAdded != null)
            {
                ObjectAdded(obj);
            }
        }

        protected void OnObjectRemoved(GridObject obj)
        {
            if (ObjectRemoved != null)
            {
                ObjectRemoved(obj);
            }
        }
    }
}
