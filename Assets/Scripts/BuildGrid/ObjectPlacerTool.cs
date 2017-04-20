using UnityEngine;

namespace BuildGrid
{
    public abstract class ObjectPlacerTool : ScriptableObject
    {
        protected Grid grid;

        public virtual void Init(Grid grid)
        {
            this.grid = grid;
        }

        public virtual void Update() { }
        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }

        protected bool RaycastMousePosition(out Vector3 worldMousePosition)
        {
            Vector3 mousePos = Input.mousePosition;
            Plane ground = new Plane(grid.transform.up, grid.transform.position);
            Ray cameraRay = Camera.main.ScreenPointToRay(mousePos);

            float distance;
            if (ground.Raycast(cameraRay, out distance))
            {
                worldMousePosition = cameraRay.GetPoint(distance);
                return true;
            }

            worldMousePosition = Vector3.zero;
            return false;
        }
    }
}
