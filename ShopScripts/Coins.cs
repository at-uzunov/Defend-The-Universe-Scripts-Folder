using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI damageText;


    int current_damage;
    int cannonsArrayElement;
    private void Start() {
        cannonsArrayElement = PlayerPrefs.GetInt("CurrentSkin", 0);
        if (this.gameObject.name != "Coins" && gameObject.GetComponent<Up>().cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage != null) {
            current_damage = gameObject.GetComponent<Up>().cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage;
        }
        else {
            coinsText.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
        }
    }
    private void Update() {
    }
}
