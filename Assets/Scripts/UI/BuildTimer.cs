using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTimer : MonoBehaviour {
        
    public Slider slider;
    [SerializeField]
    private float buildTime;
    [SerializeField]
    private float buildTimeWeight;

    void Start () {        
        slider = GetComponent<Slider>();
        slider.value = 0;        
    }   

    public void SetBuildTime(float time) {
        buildTime = time;
        buildTimeWeight = (1 / buildTime); // Ex) if build time = 5, then each second increases bar by 1/5        
    }  

    // Updates position of slider according to build time
    // when build time = 0, destroy slider
    public void Update() {
        slider.value += (Time.deltaTime * buildTimeWeight);        
                
        if (slider.value >= 1f) {
            Destroy(gameObject);
        }         
    }
}
