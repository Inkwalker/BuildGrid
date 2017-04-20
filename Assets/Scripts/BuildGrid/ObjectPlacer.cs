using UnityEngine;

namespace BuildGrid
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private Grid grid;

        private ObjectPlacerTool activeTool;

        private void Update()
        {
            if (activeTool != null)
            {
                activeTool.Update();
            }
        }

        public void SetTool(ObjectPlacerTool tool)
        {
            if (activeTool != null)
            {
                activeTool.OnDeactivate();
            }

            tool.Init(grid);
            tool.OnActivate();

            activeTool = tool;
        }
    }
}
