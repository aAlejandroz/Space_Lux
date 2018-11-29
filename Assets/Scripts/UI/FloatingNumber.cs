using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumber : MonoBehaviour {

    public float moveSpeed;   
    public int cost;
    public Text displayNumber;
    public Color color;

	// Use this for initialization
	void Start () {
        displayNumber.color = color;
	}
	
	// Update is called once per frame
	void Update () {
        string op;

        op = cost >= 0 ? "+" : "";

        displayNumber.text = op + cost;
        transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed) * Time.deltaTime);
	}
}
