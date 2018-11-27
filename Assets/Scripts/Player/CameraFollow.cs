using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{   
    public bool isActive = true;
    private Vector3 CamTarget;
    public Transform target;
    public float CamSpeed = 1;

    public void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    public void Update()
    {       
        isActive = target != null ? true : false;   // if there is target, camera is active

        if (isActive) {
            CamTarget = new Vector3(target.position.x, target.position.y, -1.5f);
            transform.position = Vector3.Lerp(transform.position, CamTarget, (CamSpeed * Time.deltaTime));
        } else {
            isActive = false;
            //Debug.Log("Player Died");
        }
        
    }
}
