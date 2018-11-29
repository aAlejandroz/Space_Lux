using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour {

    // Update image according to current building prefab
    private Image image;

    private void Start() {
        image = GetComponent<Image>();
    }

    public void UpdateGunDisplay(Sprite item) {
        image.sprite = item;
        //image.sprite = gun.GetComponent<SpriteRenderer>().sprite;
        //if (image.sprite.GetType().Equals(typeof(SpriteRenderer)))
    }
}
