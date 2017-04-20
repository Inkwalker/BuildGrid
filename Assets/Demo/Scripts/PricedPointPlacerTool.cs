using UnityEngine;
using BuildGrid;

public class PricedPointPlacerTool : PointPlacerTool
{
    protected override bool CanPlaceObject(Vector3 position, GridObject obj)
    {
        PricedGridObject pObj = obj as PricedGridObject;

        if (pObj != null && Bank.Money >= pObj.Price)
        {
            return base.CanPlaceObject(position, obj);
        }
        else
        {
            return false;
        }
    }

    protected override void OnObjectPlaced(GridObject obj)
    {
        PricedGridObject pObj = obj as PricedGridObject;

        if (pObj != null)
        {
            Bank.Money -= pObj.Price;
        }
    }
}
