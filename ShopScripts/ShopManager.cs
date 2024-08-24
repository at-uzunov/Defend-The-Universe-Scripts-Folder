using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private CannonsArraySO cannonsArraySO;
    Sprite[] baseCannonImages;
    public Image[] skinsImages;

    private void Start() {
        int imagesCount = skinsImages.Length;
        int baseCannonsLength = cannonsArraySO.baseCannonsSO.Count;
        baseCannonImages = new Sprite[baseCannonsLength];

        for (int i = 0; i < baseCannonsLength; i++) {
            baseCannonImages[i] = cannonsArraySO.baseCannonsSO[i].cannonImage;
        }

        Debug.Log("=======================");

        // Extract sprites from Image objects
        Sprite[] skinsSprites = new Sprite[skinsImages.Length];
        for (int i = 0; i < skinsImages.Length; i++) {
            skinsSprites[i] = skinsImages[i].sprite;
        }

        // Find intersection
        var intersect = from baseCannon in cannonsArraySO.baseCannonsSO
                        join skinSprite in skinsSprites on baseCannon.cannonImage equals skinSprite
                        select new { Sprite = baseCannon.cannonImage, BulletDamage = baseCannon.bulletsDamage };

        Debug.Log("Intersection:");
        foreach (var item in intersect) {
            Debug.Log($"Sprite: {item.Sprite}, Bullet Damage: {item.BulletDamage}");
        }
    }
}
