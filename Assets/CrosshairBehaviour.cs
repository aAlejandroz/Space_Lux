using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehaviour : MonoBehaviour {

    public Camera maincam;
    public float camZoffset;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cursorPos, offsCursorPos;
        cursorPos = Input.mousePosition;

        offsCursorPos = new Vector3(cursorPos.x, cursorPos.y, camZoffset);

        transform.position = maincam.ViewportToWorldPoint(offsCursorPos);

        transform.position = Input.mousePosition;
	}
}
