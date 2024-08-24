using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Button[] buyButtons;
    [SerializeField] private Button[] selectButtons;
    [SerializeField] private CannonsArraySO cannonsArraySO;
    [SerializeField] private GameObject canvas;
    private bool is_purchased = false;

    private void Awake() {

        int canvas_lenght = canvas.transform.childCount;

        Debug.Log("Number of childs in canvas => " + canvas_lenght);

        selectButtons = new Button[canvas_lenght - 2];
        buyButtons = new Button[canvas_lenght - 3];

        for (int i = 0; i < canvas_lenght - 2; i++) {
            selectButtons[i] = canvas.transform.GetChild(i+1).GetChild(0).GetComponent<Button>();

        }

        for(int i =0;i<canvas_lenght - 3; i++) {
            buyButtons[i] = canvas.transform.GetChild(i + 1).GetChild(1).GetComponent<Button>();
        }
    }
    private void Start() {

        //Coins
        coinsText.text = PlayerPrefs.GetInt("totalCoins", 100).ToString();



        int cannonsArrayLenght = cannonsArraySO.baseCannonsSO.Count;
        Debug.Log(cannonsArrayLenght);
        for(int i = 0; i < selectButtons.Length; i++) {
            Debug.Log("Current i= " + i);
            int currentIndex = i;
            //Select Buttonzasht
            selectButtons[i].onClick.AddListener(( ) => {
                PlayerPrefs.SetInt("CurrentSkin", currentIndex);
                Debug.Log("Current skin is" + currentIndex);
            });
        }
        for(int i = 0; i < buyButtons.Length; i++) {
            //Debug.Log("Current i= " + i);
            int currentIndex = i;


            //Buy Button
            buyButtons[i].onClick.AddListener(( ) => {
                string coins_value = buyButtons[currentIndex].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
                string result = coins_value.Substring(0, coins_value.Length - 5).Trim();
                Debug.Log(result);
                int coins = int.Parse(result);
                if (updateCoins(coins)) {
                    PlayerPrefs.SetString("Skin" + currentIndex, "true");
                    buyButtons[currentIndex].gameObject.SetActive(false);
                }

            });
        }
        for(int i = 0; i < cannonsArrayLenght-1; i++) {
            string result = PlayerPrefs.GetString("Skin" + i,"false").ToString();
            Debug.Log(result);
            if(result == "true") {
                Destroy(buyButtons[i].gameObject);
            }
            
        }
    }

    bool updateCoins(int coins) {
        int CurrentCoins = PlayerPrefs.GetInt("totalCoins", 100);
        if (CurrentCoins - coins > 0) {
            int newCoins = CurrentCoins - coins;
            PlayerPrefs.SetInt("totalCoins", newCoins);
            coinsText.text = newCoins.ToString();
            return true;
        }
        else {
            Debug.Log("Not Enough coins");
            return false;
        }
    }
}
