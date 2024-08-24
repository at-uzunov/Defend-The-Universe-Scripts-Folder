using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;
using TMPro;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class EnemyController : MonoBehaviour {

    public GameObject pauseMenu;

    public CannonsArraySO cannonsArraySO;
    public int CannonsArrayElement;


    [SerializeField] public EnemyListSO enemyListSO;
    public bossListSO bossListSO;
    [SerializeField] private int arrayElement;
    [SerializeField] public float countDown = 5f;
    [SerializeField] public float countDownMax = 5f;
    [SerializeField] private int fallingVelocity;
    public GameObject lootGameObject;
    public GameObject buffGameObject;
    public GameObject second_buffGameObject;
    public GameObject third_buffGameObject;
    public GameObject damage_increase_buff_gameObject;
    public GameObject health_buff;

    public float bossTimer = 0;
    public float bossTimerMax = 10;
    private int bossInt;
    public bool has_run = false;
    int bossArrayElement = 0;
    [SerializeField] public int arrayMax = 3;
    [SerializeField] public int arrayStart = 0;
    [SerializeField] private int incrementAmount = 3;
    [SerializeField] private int enemyResetNumberArrayMaxValue = 12;
    public int bossCounter = 1;
    public int current_multiplaier = 1;
    public Camera camera;

    public int highscore;
    public int current_score = 0;
    //int current_health = GameObject.Find("EnemySpawner").GetComponent<EnemyController>().arrayMax;
    public TextMeshProUGUI current_score_text;
    public TextMeshProUGUI highscore_text;


    Scene m_scene;

    GameObject canvas;

    List<GameObject> fallenEnemies = new List<GameObject>();
    int enemyListSize;



    int random;
    int random2;
    int random3;
    int random_for_damage_increase_buff;

    // buffs
    float timer = 0;
    bool buff_enable = false;
    public int min_random_buff_spawn_value = 0;
    public int max_random_buff_spawn_value = 3;

    public float buff_random_tries_each_x_seconds = 10;
    public int determinate_number_to_unlock_buff = 1;

    Image player_healthbar;

    // second buff
    float second_timer;
    public bool second_buff_enable;
    bool third_buff_enable;
    public float second_buff_random_tries_each_x_seconds = 5;
    private void Awake() {
        player_healthbar = GameObject.FindGameObjectWithTag("Player_HealthBar").GetComponent<Image>();
        highscore = PlayerPrefs.GetInt("highscore", 0);
        current_score_text = GameObject.Find("current_score").GetComponent<TextMeshProUGUI>();
        highscore_text = GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>();
        highscore_text.text = highscore.ToString();
        current_score_text.text = current_score.ToString();

        canvas = GameObject.FindGameObjectWithTag("Canvas");
        enemyListSize = enemyListSO.baseEnemyListSO.Count;

        CannonsArrayElement = PlayerPrefs.GetInt("CurrentSkin", 0);
    }

    private void Update() {
        bossTimer += Time.deltaTime;
        bossInt = Mathf.RoundToInt(bossTimer);
        DestroyGameObjectsOffCamera();
        MoveEnemy();
        //MoveEnemy_Spiral(0);
        //if (bossInt < bossTimerMax) {
            countDown -= Time.deltaTime;
            if (countDown < 0f) {
                random_for_damage_increase_buff = Random.Range(0, 5);
                if (random_for_damage_increase_buff == 3) {
                    FourthBuff(damage_increase_buff_gameObject);
                    random_for_damage_increase_buff -= 1;
                }
                countDown = countDownMax;
                if (bossInt < bossTimerMax) {
                    for (int i = arrayStart; i < Mathf.Min(arrayMax, enemyListSO.baseEnemyListSO.Count); i++) {
                        EnemyMove(enemyListSO.baseEnemyListSO[i].enemyHolder, i);
                    }
                }

            }
            timer += Time.deltaTime;
            if ( timer > buff_random_tries_each_x_seconds) {
                if (buff_enable == false) {
                    random = Random.Range(min_random_buff_spawn_value, max_random_buff_spawn_value);
                    if (random == determinate_number_to_unlock_buff) {
                        buff_enable = true;
                        third_buff_enable = true;
                        Debug.Log("DoubleBuff() executed");
                        DoubleBuff(buffGameObject);
                        FifthBuff(third_buffGameObject);
                        random = determinate_number_to_unlock_buff - 1;
                        timer = 0;
                        buff_enable = false;
                        third_buff_enable = false;
                        //Debug.Log("DoubleBuff() closed");
                    }
                    timer = 0;
                }

            }
            second_timer += Time.deltaTime;
            if (second_timer > second_buff_random_tries_each_x_seconds) {
                if (second_buff_enable == false) {
                    random2 = Random.Range(min_random_buff_spawn_value, max_random_buff_spawn_value);
                    Debug.Log("SecondBuff - Random try made. Number is: " + random.ToString());
                    if (random2 == determinate_number_to_unlock_buff) {
                        second_buff_enable = true;
                        SecondBuff(second_buffGameObject);
                        random2 = determinate_number_to_unlock_buff - 1;
                        second_timer = 0;
                        second_buff_enable = false;
                        
                    }
                    if(random == determinate_number_to_unlock_buff + 1) {
                        second_buff_enable = true;
                        ThirdBuff(health_buff);
                        random2 = determinate_number_to_unlock_buff - 1;
                        second_timer = 0;
                        second_buff_enable = false;
                    }
                    if(random == determinate_number_to_unlock_buff + 2) {
                        third_buff_enable = true;
                        FifthBuff(third_buffGameObject);
                        random2 = determinate_number_to_unlock_buff - 2;
                        second_timer = 0;
                        third_buff_enable = false;

                    }
                    second_timer = 0;
                    Debug.Log("SecondBuff - Random try closed.");
                }

            }

        //}
        if (bossInt >= bossTimerMax && !has_run) {
            has_run = true;
            bossCounter++;
            if (bossCounter > 22) {
                current_multiplaier += 1;
                bossCounter = 0;
            }
            arrayMax += incrementAmount;
            arrayStart += incrementAmount;
            if (bossArrayElement >= 0 && bossArrayElement < bossListSO.bossListArraySO.Count) {
                BossSpawned(bossListSO.bossListArraySO[bossArrayElement].enemyHolder, bossArrayElement);
                bossArrayElement += 1;
            }
            else {
                bossArrayElement = 0;
                BossSpawned(bossListSO.bossListArraySO[bossArrayElement].enemyHolder, bossArrayElement);
            }

        }
        if (arrayMax > enemyResetNumberArrayMaxValue) {
            arrayMax = 3;
            arrayStart = 0;
            bossCounter++;
        }
    }
    private void DoubleBuff(GameObject current_buff) {
        //Debug.Log("Double Buff Executed");
        GameObject buff = Instantiate(current_buff, new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), transform.localPosition.y, 0), transform.localRotation);
        buff.transform.SetParent(canvas.transform, false);
        buff.transform.SetSiblingIndex(3);
        buff.AddComponent<CollisionDetection>();
        buff.tag = "Buff";
        //Debug.Log("Spawned BUff");
        fallenEnemies.Add(buff);
    }

    private void FifthBuff( GameObject current_buff ) {
        //Debug.Log("Double Buff Executed");
        GameObject buff = Instantiate(current_buff, new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), UnityEngine.Random.Range(transform.localPosition.y - 100, transform.localPosition.y + 100), 0), transform.localRotation);
        buff.transform.SetParent(canvas.transform, false);
        buff.transform.SetSiblingIndex(3);
        buff.AddComponent<CollisionDetection>();
        buff.tag = "third_buff";
        //Debug.Log("Spawned BUff");
        fallenEnemies.Add(buff);
    }

    private void SecondBuff(GameObject current_buff) {
        //Debug.Log("Second Buff Executed");
        GameObject second_buff = Instantiate(current_buff, new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), transform.localPosition.y, 0), transform.localRotation);
        second_buff.transform.SetParent(canvas.transform, false);
        second_buff.transform.SetSiblingIndex(3);
        second_buff.AddComponent<CollisionDetection>();
        second_buff.tag = "Second_Buff";
        Debug.Log("Second_BUff spawned");
        fallenEnemies.Add(second_buff);
    }

    private void ThirdBuff(GameObject current_buff) {
        //Debug.Log("Second Buff Executed");
        GameObject second_buff = Instantiate(current_buff, new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), transform.localPosition.y, 0), transform.localRotation);
        second_buff.transform.SetParent(canvas.transform, false);
        second_buff.transform.SetSiblingIndex(3);
        second_buff.AddComponent<CollisionDetection>();
        second_buff.tag = "Health_Buff";
        Debug.Log("Second_BUff spawned");
        fallenEnemies.Add(second_buff);
    }

    private void FourthBuff(GameObject current_buff) {
        //Debug.Log("Second Buff Executed");
        GameObject damage_increase_buff = Instantiate(current_buff, new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), transform.localPosition.y, 0), transform.localRotation);
        damage_increase_buff.transform.SetParent(canvas.transform, false);
        damage_increase_buff.transform.SetSiblingIndex(3);
        damage_increase_buff.AddComponent<CollisionDetection>();
        damage_increase_buff.tag = "Damage_increase_buff";
        Debug.Log("Damage_increase_buff");
        fallenEnemies.Add(damage_increase_buff);
    }
    private void EnemyMove(GameObject current_enemy, int enemyNumber) {
        int randomY = UnityEngine.Random.Range(-300, 300);
        GameObject enemy = Instantiate(current_enemy, new Vector3(UnityEngine.Random.Range(this.transform.localPosition.x - 400, this.transform.localPosition.x + 400), this.transform.localPosition.y - randomY, 0), this.transform.localRotation);
        enemy.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = enemyListSO.baseEnemyListSO[enemyNumber].enemySprite;
        enemy.transform.SetParent(canvas.transform, false);
        enemy.transform.SetSiblingIndex(3);
        CollisionDetection col_det = enemy.AddComponent<CollisionDetection>();

       // StartCoroutine(SetAudioClipWithDelay(col_det));
        enemy.tag = "Enemy";

        fallenEnemies.Add(enemy);
    }

   /* private IEnumerator SetAudioClipWithDelay(CollisionDetection col_det) {
        // Wait for the end of the frame to ensure everything is loaded
        yield return new WaitForEndOfFrame();

        // Set the audio clip
        col_det.explosion_audiosource.clip = cannonsArraySO.baseCannonsSO[CannonsArrayElement].bulletsAudio;
        col_det.boss_damaged_audio.clip = cannonsArraySO.baseCannonsSO[CannonsArrayElement].bossBulletsAudio;

    }*/


    private void BossSpawned(GameObject current_boss, int bossNumber) {
        int randomY = UnityEngine.Random.Range(0, 150);
        if (current_boss != null) {
            GameObject enemy = Instantiate(current_boss, new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), transform.localPosition.y - randomY, 0), transform.localRotation);
            enemy.transform.GetComponent<UnityEngine.UI.Image>().sprite = bossListSO.bossListArraySO[bossNumber].enemySprite;
            enemy.transform.SetParent(canvas.transform, false);
            enemy.transform.SetSiblingIndex(3);
            enemy.gameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(transform.localPosition.x - 400, transform.localPosition.x + 400), transform.localPosition.y - randomY, 0);
            CollisionDetection collision_detection = enemy.AddComponent<CollisionDetection>();
            collision_detection.current_multiplaier = current_multiplaier;

            BossController bossController = enemy.GetComponent<BossController>();
            bossController.bossListSO = bossListSO;
            bossController.bossArrayElement = bossNumber;
            //Debug.Log("Boss number passed: " + bossNumber);

            collision_detection.bossArrayEllement = bossNumber;
            // Set the BossController component to active
            bossController.enabled = true;

            enemy.tag = "Boss";
        }
        else {
            Debug.LogError("Current boss GameObject is null.");
        }
    }

    private void DestroyGameObjectsOffCamera() {
        for (int i = fallenEnemies.Count - 1; i >= 0; i--) {
            GameObject enemy = fallenEnemies[i];
            if (enemy != null) {
                Vector3 screenPos = camera.ScreenToWorldPoint(enemy.gameObject.transform.localPosition);
                if (screenPos.y < -20) {
                    /*Debug.Log("Enemy position is: " + enemy.transform.position.y.ToString());
                    Debug.Log("Screen Height is : " + Screen.height.ToString());*/
                    if (player_healthbar.fillAmount > 0f && enemy.tag =="Enemy") {
                        player_healthbar.fillAmount -= 0.2f;
                    }
                    if(player_healthbar.fillAmount <= 0f) {

                        Time.timeScale = 0f;
                        pauseMenu.gameObject.SetActive(true);
                    }
                    Destroy(enemy);
                }
               /*if (enemy.transform.position.y < -Screen.height - 500) {
                    Destroy(enemy);
                    fallenEnemies.RemoveAt(i);
                }*/
            }
            else {
                fallenEnemies.RemoveAt(i);
            }
        }
    }

    private void MoveEnemy() {
        float moveSpeed = fallingVelocity;
        foreach (GameObject projectile in fallenEnemies) {
            if (projectile != null) {
                Vector3 moveDir = Vector3.down; // Adjust this direction as needed
                projectile.transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void MoveEnemy_Spiral(int starting_position) {
        float moveSPeed = fallingVelocity;
        foreach (GameObject projectile in fallenEnemies) {
            if(projectile != null) {
                Vector3 moveDir = Vector3.down;
                Vector3 moveDirx = Vector3.left;
                projectile.transform.position += moveDir * moveSPeed * Time.deltaTime;
                projectile.transform.position += moveDirx * moveSPeed * Time.deltaTime;
                /*if (projectile.transform.position.x == starting_position) {
                    projectile.transform.position += moveDir * moveSPeed * Time.deltaTime;
                    projectile.transform.position += moveDirx * moveSPeed * Time.deltaTime;

                    if(projectile.transform.position.x < starting_position && projectile.transform.position.x > starting_position - 100) {
                        projectile.transform.position += moveDir * moveSPeed * Time.deltaTime;
                        projectile.transform.position += moveDirx * moveSPeed * Time.deltaTime;
                    }


                }*/



            }
        }
    }

}
