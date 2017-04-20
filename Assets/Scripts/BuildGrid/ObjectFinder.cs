using UnityEngine;

namespace BuildGrid
{
    public class ObjectFinder : MonoBehaviour
    {
        private void Start()
        {
            Grid grid = GetComponent<Grid>();

            if (grid != null)
            {
                var objects = GetComponentsInChildren<GridObject>();

                foreach (var obj in objects)
                {
                    Vector3 snappedPosition = grid.SnapObjectWorldPosition(obj.transform.position, obj);
                    obj.transform.position = snappedPosition;
                    grid.PlaceObject(snappedPosition, obj);
                }
            }
        }
    }
}
