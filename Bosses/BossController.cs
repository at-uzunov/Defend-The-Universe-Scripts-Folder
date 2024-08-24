/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteAlways]
public class BossController : MonoBehaviour {

    public bossListSO bossListSO;
    public int bossArrayElement;

    [SerializeField] private float timerCount;
    [SerializeField] private float timerMax;

    [SerializeField] private float destroyObjectsOffscreen = 5f;
    [SerializeField] private float destroyObjectsOffscreenMax = 5f;

    [SerializeField] private int projectileVelocity=10;


    List<GameObject> firedBullets = new List<GameObject>();

    [SerializeField] private string bulletTag="Boss";

    [SerializeField] private GameObject leftCannon;
    [SerializeField] private GameObject rightCannon;
    public int bulletDamage;
    public int bullet_speed_multiplyier = 100;

    //Vector3 stageDimensions;

    bool left = false;
    bool right = true;

    bool touched_down = false;
    bool touched_up = true;

    float get_time;

    float start_position;

    Scene m_scene;

    private void Start() {
        start_position = gameObject.transform.localPosition.y;
        Debug.Log("Array Element: " + bossArrayElement);
        if (bossListSO.bossListArraySO[bossArrayElement] == null) {
            bossArrayElement = 0;
        }

        //stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).normalized;

        leftCannon = Instantiate(bossListSO.bossListArraySO[bossArrayElement].enemyLeftHand);
        rightCannon = Instantiate(bossListSO.bossListArraySO[bossArrayElement].enemyRightHand);
        bulletDamage = bossListSO.bossListArraySO[bossArrayElement].bulletsDamage;
        projectileVelocity = bossListSO.bossListArraySO[bossArrayElement].bulletSpeed * bullet_speed_multiplyier;
        timerMax = bossListSO.bossListArraySO[bossArrayElement].firerate;

        transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = bossListSO.bossListArraySO[bossArrayElement].bossLeftProjectiles;
        transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().sprite = bossListSO.bossListArraySO[bossArrayElement].bossRightProjectiles;

        transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().preserveAspect = true;
        transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().preserveAspect = true;

        leftCannon.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        rightCannon.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);

        //Debug.Log(stageDimensions);

        Debug.Log(Screen.width);
        Debug.Log(start_position);





    }

    private void Update() {
        get_time += Time.deltaTime;
        if(left == false && right == true) {
            //MoveBossLeft();

            //Debug.Log("start_position " + start_position);
            Left_WiggleMove_BigBrain_Showtime(5, start_position - 300, start_position + 300);
            
        }

        if (transform.position.x < 0 && left == false && right == true) {
            left = true;
            right = false;
        }

        if (left == true && right == false) {
            //MoveBossRight();
            //Left_WiggleMove_BigBrain_Showtime(5, start_position - 300, start_position + 300);
            Right_WiggleMove_BigBrain_Showtime(5, start_position - 300, start_position + 300);

        }

        if (transform.position.x > Screen.width && left == true && right == false) {
            left = false;
            right = true;

        }
        timerCount -= Time.deltaTime;
        if (timerCount < 0f) {
            StartCoroutine(BossBullets());
        }
        destroyObjectsOffscreen -= Time.deltaTime;
        if (destroyObjectsOffscreen < 0f) {
            StartCoroutine(DestroyObectsOffScreen());
        }
        MoveProjectiles();
    }
    private IEnumerator DestroyObectsOffScreen() {
        if (firedBullets != null) {
            DestroyGameObjectsOffCamera();
            destroyObjectsOffscreen = destroyObjectsOffscreenMax;
        }
        yield return null;
    }

    private IEnumerator BossBullets() {
        SpawnObject(leftCannon, -150, bossListSO.bossListArraySO[bossArrayElement].bossLeftProjectiles);
        SpawnObject(rightCannon, 150, bossListSO.bossListArraySO[bossArrayElement].bossRightProjectiles);
        timerCount = timerMax;
        yield return null;
    }
    private void SpawnObject(GameObject obj, int x_offset, Sprite attack_projectile) {
        GameObject projectile = Instantiate(obj, new Vector3(transform.localPosition.x + x_offset, transform.localPosition.y + -175, 0), transform.localRotation);
        setupProjectile(projectile, attack_projectile);

        projectile.tag = bulletTag;
        firedBullets.Add(projectile);

    }

    public void MoveBossLeft() {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - get_time, gameObject.transform.localPosition.y, 0);
            //Debug.Log("MoveBossLeft is beeing executed");

    }

    public void Left_WiggleMove_BigBrain_Showtime(float y_amplitude, float down_treshhold, float up_treshhold) {
        //.Log(gameObject.transform.localPosition);
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 5, gameObject.transform.localPosition.y - y_amplitude, 0);
            // gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - y_amplitude, gameObject.transform.localPosition.y - y_amplitude, 0);
        }
        if (gameObject.transform.localPosition.y <= down_treshhold && gameObject.transform.localPosition.y < up_treshhold) {
            touched_down = true;
            touched_up = false;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 5, gameObject.transform.localPosition.y + y_amplitude, 0);
        }
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == true && touched_up == false) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 5, gameObject.transform.localPosition.y + y_amplitude, 0);
            //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - y_amplitude, gameObject.transform.localPosition.y + y_amplitude, 0);
        }
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y >= up_treshhold && touched_down == true && touched_up == false) {
            touched_down = false;
            touched_up = true;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 5, gameObject.transform.localPosition.y - y_amplitude, 0);

        }
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 5, gameObject.transform.localPosition.y - y_amplitude, 0);

        }
    }


    public void Right_WiggleMove_BigBrain_Showtime(float y_amplitude, float down_treshhold, float up_treshhold) {
        //Debug.Log(gameObject.transform.localPosition);
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 5, gameObject.transform.localPosition.y - y_amplitude, 0);
            // gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - y_amplitude, gameObject.transform.localPosition.y - y_amplitude, 0);
        }
        if (gameObject.transform.localPosition.y <= down_treshhold && gameObject.transform.localPosition.y < up_treshhold) {
            touched_down = true;
            touched_up = false;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 5, gameObject.transform.localPosition.y + y_amplitude, 0);
        }
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == true && touched_up == false) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 5, gameObject.transform.localPosition.y + y_amplitude, 0);
            //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - y_amplitude, gameObject.transform.localPosition.y + y_amplitude, 0);
        }
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y >= up_treshhold && touched_down == true && touched_up == false) {
            touched_down = false;
            touched_up = true;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 5, gameObject.transform.localPosition.y - y_amplitude, 0);

        }
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 5, gameObject.transform.localPosition.y - y_amplitude, 0);

        }
    }

    public void MoveBossRight() {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + get_time, gameObject.transform.localPosition.y, 0);

    }
    private void MoveProjectiles() {
        float moveSpeed = projectileVelocity;
        foreach (GameObject projectile in firedBullets) {
            if (projectile != null) {
                Vector3 moveDir = Vector3.down; //+ new Vector3(Time.deltaTime*40,0,0); // Adjust this direction as needed
                projectile.transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void DestroyProjectiles() {
        foreach (GameObject projectile in firedBullets) {
            Destroy(projectile.gameObject);
        }
    }

    private void DestroyGameObjectsOffCamera() {
        foreach (GameObject projectile in firedBullets) {
            if (projectile != null) {
                if (projectile.transform.localPosition.y > Screen.height) {
                    Destroy(projectile);
                }
            }
            else {
                firedBullets = firedBullets.Where(x => x != null).ToList();
            }
        }
        Debug.Log(firedBullets.Count);
    }


    private void setupProjectile(GameObject projectile, Sprite bullet_sprite) {
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        projectile.transform.SetSiblingIndex(3);
        projectile.transform.GetChild(0).gameObject.SetActive(false);
        projectile.transform.GetChild(1).gameObject.SetActive(true);

        projectile.transform.GetChild(1).GetComponent<Image>().sprite = bullet_sprite;
        projectile.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;

    }

}*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class BossController : MonoBehaviour {
    public bossListSO bossListSO;
    public int bossArrayElement;

    [SerializeField] private float timerCount;
    [SerializeField] private float timerMax;

    [SerializeField] private float destroyObjectsOffscreen = 5f;
    [SerializeField] private float destroyObjectsOffscreenMax = 5f;

    [SerializeField] private int projectileVelocity = 3;

    List<GameObject> firedBullets = new List<GameObject>();

    [SerializeField] private string bulletTag = "Boss";

    [SerializeField] private GameObject leftCannon;
    [SerializeField] private GameObject rightCannon;
    public int bulletDamage;
    public int bullet_speed_multiplyier = 10;

    bool left = false;
    bool right = true;

    bool touched_down = false;
    bool touched_up = true;

    float get_time;

    float start_position;

    RectTransform canvasRectTransform;

    private void Start() {
        start_position = gameObject.transform.localPosition.y;
        Debug.Log("Array Element: " + bossArrayElement);

        // Find the Canvas RectTransform
        canvasRectTransform = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();

        if (bossListSO.bossListArraySO[bossArrayElement] == null) {
            bossArrayElement = 0;
        }

        leftCannon = Instantiate(bossListSO.bossListArraySO[bossArrayElement].enemyLeftHand);
        rightCannon = Instantiate(bossListSO.bossListArraySO[bossArrayElement].enemyRightHand);
        bulletDamage = bossListSO.bossListArraySO[bossArrayElement].bulletsDamage;
        projectileVelocity = bossListSO.bossListArraySO[bossArrayElement].bulletSpeed;
        timerMax = bossListSO.bossListArraySO[bossArrayElement].firerate;

        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = bossListSO.bossListArraySO[bossArrayElement].bossLeftProjectiles;
        transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = bossListSO.bossListArraySO[bossArrayElement].bossRightProjectiles;

        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().preserveAspect = true;
        transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().preserveAspect = true;

        leftCannon.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        rightCannon.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);

        //Debug.Log(Screen.width);
        //Debug.Log(start_position);
    }

    private void Update() {
        get_time += Time.deltaTime;

        if (left == false && right == true) {
            Left_WiggleMove_BigBrain_Showtime(1, start_position - 300, start_position + 300);
        }

        // Calculate screen boundaries in world space
        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);
        float leftBoundary = canvasCorners[0].x;
        float rightBoundary = canvasCorners[2].x;

        if (transform.position.x < leftBoundary && left == false && right == true) {
            left = true;
            right = false;
        }

        if (left == true && right == false) {
            Right_WiggleMove_BigBrain_Showtime(1, start_position - 300, start_position + 300);
        }

        if (transform.position.x > rightBoundary && left == true && right == false) {
            left = false;
            right = true;
        }

        timerCount -= Time.deltaTime;
        if (timerCount < 0f) {
            StartCoroutine(BossBullets());
        }

        /*destroyObjectsOffscreen -= Time.deltaTime;
        if (destroyObjectsOffscreen < 0f) {
            StartCoroutine(DestroyObectsOffScreen());
        }*/
        DestroyGameObjectsOffCamera();
        MoveProjectiles();
    }

    private IEnumerator DestroyObectsOffScreen() {
        if (firedBullets != null) {
            DestroyGameObjectsOffCamera();
            destroyObjectsOffscreen = destroyObjectsOffscreenMax;
        }
        yield return null;
    }

    private IEnumerator BossBullets() {
        SpawnObject(leftCannon, -150, bossListSO.bossListArraySO[bossArrayElement].bossLeftProjectiles);
        SpawnObject(rightCannon, 150, bossListSO.bossListArraySO[bossArrayElement].bossRightProjectiles);
        timerCount = timerMax;
        yield return null;
    }

    private void SpawnObject(GameObject obj, int x_offset, Sprite attack_projectile) {
        GameObject projectile = Instantiate(obj, new Vector3(transform.localPosition.x + x_offset, transform.localPosition.y - 175, 0), transform.localRotation);
        setupProjectile(projectile, attack_projectile);

        projectile.tag = bulletTag;
        firedBullets.Add(projectile);
    }

    public void MoveBossLeft() {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - get_time, gameObject.transform.localPosition.y, 0);
    }

    public void Left_WiggleMove_BigBrain_Showtime(float y_amplitude, float down_treshhold, float up_treshhold) {
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 150*Time.deltaTime, gameObject.transform.localPosition.y - 150 * Time.deltaTime, 0);
        }

        if (gameObject.transform.localPosition.y <= down_treshhold && gameObject.transform.localPosition.y < up_treshhold) {
            touched_down = true;
            touched_up = false;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 150 * Time.deltaTime, gameObject.transform.localPosition.y + 150 * Time.deltaTime, 0);
        }

        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == true && touched_up == false) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 150 * Time.deltaTime, gameObject.transform.localPosition.y + 150 * Time.deltaTime, 0);
        }

        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y >= up_treshhold && touched_down == true && touched_up == false) {
            touched_down = false;
            touched_up = true;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 150 * Time.deltaTime, gameObject.transform.localPosition.y - 150 * Time.deltaTime, 0);
        }

        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 150 * Time.deltaTime, gameObject.transform.localPosition.y - 150 * Time.deltaTime, 0);
        }
    }

    public void Right_WiggleMove_BigBrain_Showtime(float y_amplitude, float down_treshhold, float up_treshhold) {
        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 150 * Time.deltaTime, gameObject.transform.localPosition.y - 150 * Time.deltaTime, 0);
        }

        if (gameObject.transform.localPosition.y <= down_treshhold && touched_down ==false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 150 * Time.deltaTime, gameObject.transform.localPosition.y + 150 * Time.deltaTime, 0);
            touched_down = true;
            touched_up = false;
        }

        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == true && touched_up == false) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 150 * Time.deltaTime, gameObject.transform.localPosition.y + 150 * Time.deltaTime, 0);
        }

        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y > up_treshhold && touched_down == true && touched_up == false) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 150 * Time.deltaTime, gameObject.transform.localPosition.y - 150 * Time.deltaTime, 0);
            touched_down = false;
            touched_up = true;
            
        }

        if (gameObject.transform.localPosition.y > down_treshhold && gameObject.transform.localPosition.y < up_treshhold && touched_down == false && touched_up == true) {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 150 * Time.deltaTime, gameObject.transform.localPosition.y - 150 * Time.deltaTime, 0);
        }
    }

    public void MoveBossRight() {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + get_time, gameObject.transform.localPosition.y, 0);
    }

    private void MoveProjectiles() {
        float moveSpeed = projectileVelocity;
        foreach (GameObject projectile in firedBullets) {
            if (projectile != null) {
                Vector3 moveDir = Vector3.down;
                projectile.transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void DestroyProjectiles() {
        foreach (GameObject projectile in firedBullets) {
            if (projectile != null) {
                Destroy(projectile.gameObject);
            }
        }
    }

    private void DestroyGameObjectsOffCamera() {
        foreach (GameObject projectile in firedBullets) {
            if (projectile != null) {
                if (projectile.transform.localPosition.y < -1000) {
                    Destroy(projectile);
                }
            }
            else {
                firedBullets = firedBullets.Where(x => x != null).ToList();
            }
        }
    }

    private void setupProjectile(GameObject projectile, Sprite bullet_sprite) {
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        projectile.transform.SetSiblingIndex(3);
        projectile.transform.GetChild(0).gameObject.SetActive(false);
        projectile.transform.GetChild(1).gameObject.SetActive(true);

        projectile.transform.GetChild(1).GetComponent<Image>().sprite = bullet_sprite;
        projectile.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
    }
}
