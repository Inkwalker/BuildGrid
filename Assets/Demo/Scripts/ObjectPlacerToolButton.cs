using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BuildGrid;

public class ObjectPlacerToolButton : MonoBehaviour
{
    [SerializeField] private ObjectPlacerTool tool;

    public ToolSelectedEvent ToolSelected;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        if (ToolSelected != null)
        {
            ToolSelected.Invoke(tool);
        }
    }

    [System.Serializable]
    public class ToolSelectedEvent : UnityEvent<ObjectPlacerTool> { }
}
