using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class CollisionDetection : MonoBehaviour {

    public AudioSource enemy_hit_audio;
    public AudioSource actual_explosionaudio;
    public AudioSource double_buffsaudio;
    public AudioSource double_damage_audio;
    public AudioSource health_up_audio;
    public AudioSource boss_damaged_audio;
    public AudioSource boss_completed_audio;
    Image healthBar;
    float bulletDamage;
    public bossListSO bossListSO;
    EnemyListSO enemyListSO;

    public int bossArrayEllement;

    public int enemyHealthElement;

    public int current_multiplaier = 1;
    Scene m_scene;
    public void Start() {
        TextMeshProUGUI player_damage = GameObject.FindGameObjectWithTag("Damage_text").GetComponent<TextMeshProUGUI>();
        PlayerCannons plyrCannons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCannons>();
        player_damage.text = plyrCannons.bulletDamage.ToString();
        if (this.tag == "Enemy") {
            healthBar = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }
        else if(this.tag == "Boss") {
            healthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        }
        bulletDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCannons>().ReturnBulletDamage();

        bossListSO = GameObject.Find("EnemyController").GetComponent<EnemyController>().bossListSO;

        enemyListSO = GameObject.Find("EnemyController").GetComponent<EnemyController>().enemyListSO;

        EnemyController enemyController = GameObject.Find("EnemyController").GetComponent<EnemyController>();
        enemyHealthElement = enemyController.arrayStart;

        enemy_hit_audio = GameObject.Find("Damage_objects_Normal").GetComponent<AudioSource>();
        actual_explosionaudio = GameObject.Find("Actualdestroyedsound").GetComponent<AudioSource>();
        double_buffsaudio = GameObject.Find("BuffsAudioSource").GetComponent<AudioSource>();
        double_damage_audio = GameObject.Find("Double_Damage_Audio_Source").GetComponent<AudioSource>();
        health_up_audio = GameObject.Find("Sounds_Health_AudioSource").GetComponent<AudioSource>();
        boss_damaged_audio = GameObject.Find("Boss_Damaged_Audio").GetComponent<AudioSource>();
        boss_completed_audio = GameObject.Find("Boss_Completed_Audio").GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Bullets" && this.tag == "Enemy") {
            float damage_amount = collision.gameObject.GetComponent<BulletsInfo>().Bullet_Damage;
            //Debug.Log("Bullet hit minion for : " + damage_amount.ToString() + " damage");
            healthBar.fillAmount -= damage_amount / enemyListSO.baseEnemyListSO[enemyHealthElement].Health;
            Destroy(collision.gameObject);
            if (healthBar.fillAmount <= .1f) {
                actual_explosionaudio.Play();
                Destroy(gameObject);
            }
            else {
                enemy_hit_audio.Play();
            }
        }
        if (collision.gameObject.tag == "Bullets" && this.tag == "Boss") {
            float damage_amount = collision.gameObject.GetComponent<BulletsInfo>().Bullet_Damage;
            damage_amount = damage_amount / current_multiplaier;
            //Debug.Log("Bullet hit boss for : " + damage_amount.ToString() + " damage");
            healthBar.fillAmount -= (damage_amount / bossListSO.bossListArraySO[bossArrayEllement].Health);
            Destroy(collision.gameObject);
            if (healthBar.fillAmount <= .1f) {
                Destroy(gameObject);
                boss_completed_audio.Play();
                int current_coins = PlayerPrefs.GetInt("totalCoins", 0);
                current_coins += 2;
                PlayerPrefs.SetInt("totalCoins", current_coins);
                TextMeshProUGUI coins_text = GameObject.Find("Coins").GetComponent<TextMeshProUGUI>();
                coins_text.text = current_coins.ToString();

                this.GetComponent<BossController>().DestroyProjectiles();
                EnemyController enemycontroller = GameObject.Find("EnemyController").GetComponent<EnemyController>();
                enemycontroller.countDown = enemycontroller.countDownMax;
                                 
                enemycontroller.bossTimer = 0f;
                enemycontroller.has_run = false;
                enemycontroller.current_score += 1;
                enemycontroller.current_score_text.text = enemycontroller.current_score.ToString();

                /*AudioClip music = enemycontroller.bossListSO.bossListArraySO[bossArrayEllement].backgroundMusic;
                AudioSource audiosource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
                audiosource.clip = music;
                audiosource.Play();*/

                Image background = GameObject.Find("background (1)").GetComponent<Image>();
                background.sprite = enemycontroller.bossListSO.bossListArraySO[bossArrayEllement].backgroundImage;
                if (enemycontroller.current_score > enemycontroller.highscore) {
                    PlayerPrefs.SetInt("highscore", enemycontroller.current_score);
                    enemycontroller.highscore_text.text = enemycontroller.current_score.ToString();
                }
                TextMeshProUGUI set_text = GameObject.Find("current_score").GetComponent<TextMeshProUGUI>();
                set_text.text = enemycontroller.current_score.ToString();
            }
            else {
                boss_damaged_audio.Play();
            }
        }

        if (collision.gameObject.tag == "Bullets" && this.tag == "Player") {
            //Debug.Log("One shot!");
        }
        if (collision.gameObject.tag == "Bullets" && this.tag == "Buff") {
            double_buffsaudio.Play();
            Destroy(gameObject);
            Destroy(collision.gameObject);
            PlayerCannons plyrCannons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCannons>();
            plyrCannons._double = true;
            //Debug.Log("_duble - true");

        }

        if (collision.gameObject.tag == "Bullets" && this.tag == "Second_Buff") {
            double_damage_audio.Play();
            PlayerCannons plyrCannons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCannons>();
            plyrCannons.second_double = true;
            //Debug.Log("second_duble - true");
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Bullets" && this.tag == "third_buff") {
            double_damage_audio.Play();
            PlayerCannons plyrCannons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCannons>();
            plyrCannons.third = true;
            //Debug.Log("third - true");
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Bullets" && this.tag== "Health_Buff") {
            Image player_healthbar = GameObject.FindGameObjectWithTag("Player_HealthBar").GetComponent<Image>();
            player_healthbar.fillAmount += .2f;
            Destroy(gameObject);
            Destroy(collision.gameObject);
            health_up_audio.Play();
        }
        if (collision.gameObject.tag == "Bullets" && this.tag == "Damage_increase_buff") {
            TextMeshProUGUI player_damage = GameObject.FindGameObjectWithTag("Damage_text").GetComponent<TextMeshProUGUI>();
            PlayerCannons plyrCannons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCannons>();
            plyrCannons.bulletDamage += 50;
            player_damage.text = plyrCannons.bulletDamage.ToString();
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

    }
}
