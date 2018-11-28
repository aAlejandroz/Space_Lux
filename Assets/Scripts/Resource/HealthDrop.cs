using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour {

    public int worth = 25;   
    public PlayerPickup playerResource;
    public GameObject Stimpak;
    [SerializeField]
    private CircleCollider2D circlecol;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(Input.GetKeyDown(KeyCode.E) && other.tag.Equals("Player") && other.gameObject.GetComponent<PlayerPickup>().ResourceCount >= worth)
        {            
            other.GetComponent<PlayerPickup>().DecrementResource(worth);
            playerResource.DisplayNumber(-worth, Color.red);
            Instantiate(Stimpak, transform.position + new Vector3(0.0f, 1.0f, 0), Stimpak.transform.rotation);
        }
    }
}
