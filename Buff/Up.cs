using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Up : MonoBehaviour {
    public CannonsArraySO cannonsArraySO;
    [SerializeField] private int cannonsArrayElement;
    [SerializeField] private Image img_healthbar;
    [SerializeField] private float fillamount;
    [SerializeField] private TextMeshProUGUI upgrade_Cost;
    [SerializeField] private TextMeshProUGUI coins_text;
    [SerializeField] private TextMeshProUGUI current_damage;
    int maxDamage;
    public int bulletDamage;

    [SerializeField] private TextMeshProUGUI attack_speed_cost;

    public TextMeshProUGUI current_attack_speed_text;
    public Image img_attack_speed;
    public float firerate;
    public float max_attack_speed = 7;
    public float attack_speed_increase=1;

    private void Start() {
        cannonsArrayElement = PlayerPrefs.GetInt("CurrentSkin", 0);
        firerate = cannonsArraySO.baseCannonsSO[cannonsArrayElement].firerate;
        attack_speed_increase = cannonsArraySO.baseCannonsSO[cannonsArrayElement].attack_speed_increase;
        maxDamage = cannonsArraySO.baseCannonsSO[cannonsArrayElement].MaxDamage;
        bulletDamage = cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage;
        current_damage.text = bulletDamage.ToString();
        gameObject.GetComponent<Image>().sprite = cannonsArraySO.baseCannonsSO[cannonsArrayElement].cannonImage;
        upgrade_Cost.text = cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost.ToString();
        coins_text.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
        if (img_healthbar != null) {
            fillamount = bulletDamage - maxDamage;
            float real_number = 1 - (fillamount / -100);
            img_healthbar.fillAmount = real_number;
        }
        if (img_attack_speed != null) {
            img_attack_speed.fillAmount = CalculateAttackSpeedFillAmount();
            current_attack_speed_text.text = firerate.ToString();
        }

    }

    private float CalculateAttackSpeedFillAmount() {
        float result = attack_speed_increase / max_attack_speed;
        Debug.Log("Attack speed calc: " + result.ToString());
        Debug.Log("ACTUAL attack speed is : " + attack_speed_increase);
        Debug.Log("MAX ATTACK SPEED IS: " + max_attack_speed);
        return  attack_speed_increase / max_attack_speed;
    }

    public void Increase_Cannon_Damage() {
        bulletDamage = cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage;
        fillamount = bulletDamage - maxDamage;
        float real_number = 1 - (fillamount / -100);
        int get_coins = PlayerPrefs.GetInt("totalCoins", 0);
        int current_multiplaier = cannonsArraySO.baseCannonsSO[cannonsArrayElement].damage_coins_multiplaier;
        if (img_healthbar != null) { // && bulletDamage > cannonsArraySO.baseCannonsSO[cannonsArrayElement].MinDamage) {
            bulletDamage = cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage + 10;
            img_healthbar.fillAmount = real_number;
            cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage = bulletDamage;
            get_coins -= cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost * current_multiplaier;
            PlayerPrefs.SetInt("totalCoins", get_coins);
            coins_text.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
            current_damage.text = bulletDamage.ToString();
        } 
        else {
            Debug.Log("Max Damage reached");
        }
    }

    public void Decrease_Cannon_Damage() {
        bulletDamage = cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage;
        fillamount = bulletDamage - maxDamage;
        float real_number = 1 - (fillamount / -100);
        int get_coins = PlayerPrefs.GetInt("totalCoins", 0);
        int current_multiplaier = cannonsArraySO.baseCannonsSO[cannonsArrayElement].damage_coins_multiplaier;
        if (img_healthbar != null) { // && bulletDamage > cannonsArraySO.baseCannonsSO[cannonsArrayElement].MinDamage) {
            bulletDamage = cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage - 10;
            img_healthbar.fillAmount = real_number;
            cannonsArraySO.baseCannonsSO[cannonsArrayElement].bulletsDamage = bulletDamage;
            get_coins += cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost * current_multiplaier;
            PlayerPrefs.SetInt("totalCoins", get_coins);
            coins_text.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
            current_damage.text = bulletDamage.ToString();
        }
        else {
            Debug.Log("Minimum Damage reached");
        }
    }

    public void Increase_attack_speed() {
        firerate = cannonsArraySO.baseCannonsSO[cannonsArrayElement].firerate;
        int get_coins = PlayerPrefs.GetInt("totalCoins", 0);
        attack_speed_increase= cannonsArraySO.baseCannonsSO[cannonsArrayElement].attack_speed_increase;
        if (img_attack_speed != null ) {
            attack_speed_increase += 1;
            cannonsArraySO.baseCannonsSO[cannonsArrayElement].attack_speed_increase = attack_speed_increase;
            cannonsArraySO.baseCannonsSO[cannonsArrayElement].firerate = firerate + CalculateAttackSpeedFillAmount();
            img_attack_speed.fillAmount = CalculateAttackSpeedFillAmount();
            get_coins -= cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost;
            PlayerPrefs.SetInt("totalCoins", get_coins); 
            attack_speed_cost.text = cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost.ToString();
            coins_text.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
            current_attack_speed_text.text = attack_speed_increase.ToString();
        }
        else {
            Debug.Log("Max Damage reached");
        }
    }


    public void Decrease_attack_speed() {
        firerate = cannonsArraySO.baseCannonsSO[cannonsArrayElement].firerate;
        int get_coins = PlayerPrefs.GetInt("totalCoins", 0);
        attack_speed_increase = cannonsArraySO.baseCannonsSO[cannonsArrayElement].attack_speed_increase;
        if (img_attack_speed != null) {
            attack_speed_increase -= 1;
            cannonsArraySO.baseCannonsSO[cannonsArrayElement].attack_speed_increase = attack_speed_increase;
            cannonsArraySO.baseCannonsSO[cannonsArrayElement].firerate = firerate - CalculateAttackSpeedFillAmount();
            img_attack_speed.fillAmount = CalculateAttackSpeedFillAmount();
            /*get_coins -= cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost;
            PlayerPrefs.SetInt("totalCoins", get_coins);*/
            attack_speed_cost.text = cannonsArraySO.baseCannonsSO[cannonsArrayElement].upgrade_cost.ToString();
            coins_text.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
            current_attack_speed_text.text = attack_speed_increase.ToString();
        }
        else {
            Debug.Log("Lowest_damage Damage reached");
        }
    }
}
