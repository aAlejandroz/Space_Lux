using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {
    
    //public Slider resourceSlider;
    //public float maxResource;

    public Text text;

    public void Start() {
        //resourceSlider.value = 0;
        //maxResource = 500;

        text = GetComponent<Text>();
    }

    public void UpdateResource(int resource) {

        text.text = resource.ToString();

        //resourceSlider.value = resource / maxResource;
       
    }
}
