using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounterUI : MonoBehaviour {

    private int killCount = 0;
    public Text killCountText;

    public void Awake() {
        killCountText.text = "0";
    }

    public void IncKillCount() {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
