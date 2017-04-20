using BuildGrid;

public class PricedRemovePlacerTool : RemovePlacerTool
{
    protected override void OnObjectRemoved(GridObject obj)
    {
        PricedGridObject pObj = obj as PricedGridObject;

        if (pObj != null)
        {
            Bank.Money += pObj.Price;
        }
    }
}
