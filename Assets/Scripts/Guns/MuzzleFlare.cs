using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlare : MonoBehaviour {

    public float destroyTime = 0.2f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
