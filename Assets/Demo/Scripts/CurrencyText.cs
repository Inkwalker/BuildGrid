using UnityEngine;
using UnityEngine.UI;

public class CurrencyText : MonoBehaviour
{
    [SerializeField] private string currenctSign = "¢";

    private Text currencyText;

    private void Awake()
    {
        currencyText = GetComponent<Text>();
    }

    private void Update()
    {
        currencyText.text = Bank.Money.ToString() + currenctSign;
    }
}
