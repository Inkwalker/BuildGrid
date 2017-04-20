using UnityEngine;
using UnityEngine.EventSystems;

namespace BuildGrid
{
    [CreateAssetMenu(menuName = "Object Placer Tools / Point tool")]
    public class PointPlacerTool : ObjectPlacerTool
    {
        [SerializeField] private GridObject objectPrefab;

        private GridObject ghost;
        private int rotations;

        public override void OnActivate()
        {
            ghost = CreateGhost();
        }

        public override void OnDeactivate()
        {
            Destroy(ghost.gameObject);
        }

        public override void Update()
        {
            Vector3 mouse;
            if (RaycastMousePosition(out mouse))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ghost.Rotate(true);
                    rotations = (rotations + 1) % 4;
                }

                Vector3 snappedPosition = grid.SnapObjectWorldPosition(mouse, ghost);
                ghost.transform.position = snappedPosition;

                if (Input.GetMouseButtonDown(0) && CanPlaceObject(snappedPosition, ghost))
                {
                    if (EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        if (grid.PlaceObject(snappedPosition, ghost))
                        {
                            OnObjectPlaced(ghost);
                            ghost = CreateGhost();
                            ghost.transform.position = snappedPosition;
                        }
                    }
                }
            }
        }

        protected virtual bool CanPlaceObject(Vector3 position, GridObject obj)
        {
            return grid.CanPlaceObject(position, obj);
        }

        protected virtual void OnObjectPlaced(GridObject obj) { }

        private GridObject CreateGhost()
        {
            GridObject obj = Instantiate(objectPrefab);

            for (int i = 0; i < rotations; i++)
            {
                obj.Rotate(true);
            }

            return obj;
        }
    }
}
