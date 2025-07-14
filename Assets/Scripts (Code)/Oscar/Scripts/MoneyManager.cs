using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [Header("Configuración")]
    public int startingMoney = 100;
    public TMP_Text moneyText;           

    private int currentMoney;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        currentMoney = startingMoney;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = $"${currentMoney}";
    }

 
    public bool Spend(int amount)
    {
        if (amount > currentMoney) return false;
        currentMoney -= amount;
        UpdateUI();
        return true;
    }

    public void Earn(int amount)
    {
        currentMoney += amount;
        UpdateUI();
    }
}