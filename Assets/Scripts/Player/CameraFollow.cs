using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Vector3 CamTarget;
    public Transform target;
    public float CamSpeed = 1;

    public void Update()
    {
        CamTarget = new Vector3(target.position.x, target.position.y, -1.5f);
        transform.position = Vector3.Lerp(transform.position, CamTarget, (CamSpeed * Time.deltaTime));
    }
}
