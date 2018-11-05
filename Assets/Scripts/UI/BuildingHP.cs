using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHP : MonoBehaviour {

    public Text health;

    public void Start() {
        health = GetComponent<Text>();
    }               

    public void UpdateHP(float currentHP) {
        health.text = currentHP.ToString("0");

        if (currentHP <= 0f) {
            Destroy(gameObject);
        }
    }
}
