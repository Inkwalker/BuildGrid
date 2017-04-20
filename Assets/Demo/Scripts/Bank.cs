using UnityEngine;

public class Bank : MonoBehaviour
{
    public static int Money { get; set; }

    [SerializeField] private int startCurrency = 1000;

    private void Awake()
    {
        Money = startCurrency;
    }
}
