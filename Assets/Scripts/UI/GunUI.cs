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

    public void UpdateGunDisplay(Buildable gun) {
        image.sprite = gun.GetComponent<SpriteRenderer>().sprite;       
    }
}
