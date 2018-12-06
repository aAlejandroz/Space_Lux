using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunNameUI : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    public string gunName;   

    public void Start() {

    }

    public void Update() {
        textDisplay.text = gunName;
    }

    public void UpdateName(string name) {
        gunName = name;
    }
}
