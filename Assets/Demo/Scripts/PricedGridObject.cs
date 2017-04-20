using UnityEngine;
using BuildGrid;

public class PricedGridObject : GridObject
{
    [SerializeField] private int price;

    public int Price { get { return price; } }
}
