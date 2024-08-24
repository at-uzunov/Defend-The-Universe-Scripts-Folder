using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Player : MonoBehaviour {
    [SerializeField] private GameObject player;
    public GameObject pauseMenu;

    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
            float offset = touchPos.y * 0.30f;
            player.transform.position = new Vector3(touchPos.x, touchPos.y-offset, player.transform.position.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Boss" && this.tag == "Player") {
            Debug.Log("One shot!");
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    // " it's coool keyboard, definately eastier to use, not feeling as chained to code. I would describe it - freeing 
    // and it has end functinon




}

