using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] public string MAIN_SCENE_NAME;
    [SerializeField] public string UPGRADE_SCENE_NAME;
    [SerializeField] public string PURCHASE_SCENE_NAME;
    [SerializeField] public string BUFF_SCENE_NAME;
    public void PlayButton() {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    public void UpgradeButton() {
        SceneManager.LoadScene(UPGRADE_SCENE_NAME);
    }

    public void BuffButton() {
        SceneManager.LoadScene(BUFF_SCENE_NAME);
    }

    public void PurchaseButton() {
        SceneManager.LoadScene(PURCHASE_SCENE_NAME);
    }


}
