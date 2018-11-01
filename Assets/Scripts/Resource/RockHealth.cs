using System.Collections;
using UnityEngine;

public class RockHealth : MonoBehaviour {
    public GameObject drop;

    private void OnDestroy() //called, when enemy will be destroyed
    {
        Instantiate(drop, transform.position, drop.transform.rotation); //your dropped sword      
    }


}
