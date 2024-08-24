using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class BaseEnemySO : ScriptableObject
{
    public GameObject enemyHolder;
    public GameObject enemyLeftHand;
    public GameObject enemyRightHand;
    public Sprite enemySprite;
    public int Health;
    public Sprite bossLeftProjectiles;
    public Sprite bossRightProjectiles;

    public int bulletSpeed;
    public float firerate = 5;

    public int bulletsDamage;

    public Sprite  backgroundImage;
    //public AudioClip backgroundMusic;

    public float ReturnHealth() {
        return Health;
    }

}
