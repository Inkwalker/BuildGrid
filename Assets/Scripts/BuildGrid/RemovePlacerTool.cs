using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BuildGrid
{
    [CreateAssetMenu(menuName = "Object Placer Tools / Remove tool")]
    public class RemovePlacerTool : ObjectPlacerTool
    {
        public override void Update()
        {
            Vector3 mouse;
            if(RaycastMousePosition(out mouse))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        GridCoordinate tilePos = grid.WorldToGrid(mouse);
                        Tile tile = grid[tilePos];

                        if (tile != null)
                        {
                            foreach (var obj in tile.Objects)
                            {
                                grid.RemoveObject(obj);
                                OnObjectRemoved(obj);
                                DestroyGridObject(obj);
                            }
                        }
                    }
                }
            }
        }

        private void DestroyGridObject(GridObject obj)
        {
            Destroy(obj.gameObject);
        }

        protected virtual void OnObjectRemoved(GridObject obj) { }
    }
}
