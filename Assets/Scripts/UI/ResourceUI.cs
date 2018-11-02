using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {
    
    public Text text;

    public void Start() {
        text = GetComponent<Text>();
    }

    public void UpdateResource(int resource) {
        text.text = resource.ToString();
    }
}
