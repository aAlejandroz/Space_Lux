using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTimer : MonoBehaviour {
       
    public Slider slider;   
    private float buildTime;
    private float buildTimeWeight;

    // Start function
    void Start () {        
        slider = GetComponent<Slider>();
        slider.value = 0;        
    }

    // Updates position of slider according to build time  
    public void Update() {
        slider.value += (Time.deltaTime * buildTimeWeight);

        if (slider.value >= 1f) {
            Destroy(gameObject);
        }
    }

    // Function to set initial build time
    public void SetBuildTime(float time) {
        buildTime = time;
        buildTimeWeight = (1 / buildTime); // Ex) if build time = 5, then each second increases bar by 1/5        
    }  
}
