using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : Damageable {

    public int worth = 100;
    public float repairRate;
    public bool canRepair;
    public Slider baseHealth;
    [SerializeField]
    private PlayerPickup playerResource;
    [SerializeField]
    private CircleCollider2D circlecol;

    public Image Fill;  
    public Color MaxHealthColor = Color.green;
    public Color MinHealthColor = Color.red;

    public void Start() {
        playerResource = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickup>();
        //baseHealth.value = MaxHP;
        baseHealth.wholeNumbers = true;
        baseHealth.minValue = 0f;
        baseHealth.maxValue = MaxHP;
        canRepair = false;
    }

    public void Update() {

        if (CurHP >= MaxHP) {
            CurHP = MaxHP;
        }

        canRepair = CurHP < MaxHP ? true : false;
        //baseHealth.value = CurHP / MaxHP;
        UpdateHealthBar(CurHP);
    }

    public void UpdateHealthBar(float val)
    {
        baseHealth.value = val;
        //Fill.color = Color.Lerp(MinHealthColor, MaxHealthColor, (float)val / MaxHP);
    }

    public void Repair() {
        Debug.Log("Repairing");
        canRepair = false;        
        CurHP += 40;
        Wait(repairRate);
    }

    protected override void OnDamaged(float damage) {
        if (!isInvincible) {            
            CurHP -= damage;
            baseHealth.value = (CurHP/ MaxHP);
            StartCoroutine(setInvincibleAndWait());
        }
    }

    protected override void OnDestroyed() {       
        Debug.Log("GAME OVER");        
        Destroy(gameObject); 
    }
    
    public override IEnumerator WaitAndChangeColor() {
        yield break;
    }
    
    public IEnumerator Wait(float repairRate) {
        yield return new WaitForSeconds(repairRate);
        canRepair = true;
    }          

    private void OnTriggerStay2D(Collider2D other) {
        
        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Build Spawn" && playerResource.ResourceCount >= worth) {
            if (canRepair) {
                playerResource.DecrementResource(worth);
                Repair();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && other.tag.Equals("Player") && playerResource.ResourceCount >= worth) {
            if (canRepair) {
                playerResource.DecrementResource(worth);
                Repair();
            }            
        } 
       
    }
}