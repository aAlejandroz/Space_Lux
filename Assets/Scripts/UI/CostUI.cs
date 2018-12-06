using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostUI : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    public string turretName;
    public int cost;    

    public void Start() {
        
    }

    public void Update() {
        textDisplay.text = turretName + "\nCost: " + cost;
    }

    public void UpdateCosts(int cost, string turretName) {
        this.cost = cost;
        this.turretName = turretName;
    }
}
