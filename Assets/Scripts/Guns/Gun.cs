using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Gun : Item {

	public float FireRate;
	public Projectile ProjectilePrefab; // Has events for on collide
	public Transform SpawnPoint;
    public AudioSource FireSound;
    public bool isPickedUp;
	[SerializeField] private bool canFire;
    protected bool canPlaySound;
    private BoxCollider2D boxCollider2D;

    public Slider slider;
    private GameObject buildUIInfo;
    public float ReloadTime = 0.75f;
    private float curReloadingTime;
    private bool Reloading;

    public float heatingRate;
    public float heating;
    public float maxHeat;
    public float heatingSliderWeight;


    public void Awake() {        
        boxCollider2D = GetComponent<BoxCollider2D>();
        isPickedUp = false;
        Reloading = false;        
    }

    public void Start() {
        if (isPickedUp || transform.parent != null) {         
            if (transform.parent.tag == "PlayerAxis") {
                isPickedUp = true;
                GetComponent<SpriteRenderer>().enabled = false;
                slider = GameObject.FindGameObjectWithTag("ReloadSlider").GetComponent<Slider>();
            }
        }
        else {
            GetComponent<SpriteRenderer>().enabled = true;            
        }

        heating = 0.0f;
    }

    private void Update()
    {        

        if (slider != null) {
            slider.value = heating;
        }

        if (heating >= 0.0f && !Reloading) {
            heating -= (Time.deltaTime * (heating) / 10);            
        }               

        if (Input.GetKey(KeyCode.R) && isPickedUp && transform.parent.tag == "PlayerAxis")
        {
            canFire = false;
            StartCoroutine(Reload());
        }

        if (Reloading)
        {
            heating -= (Time.deltaTime * heatingSliderWeight);
            curReloadingTime += Time.deltaTime;                        
            if (curReloadingTime >= ReloadTime)
                Reloading = false;
        }
    }

    public Gun() {
		canFire = true;
	}

    protected abstract void OnFire();

	public override void Use() {        
		if (canFire) {
            StartCoroutine(fireAndWait());
            heating += (Time.deltaTime * heatingRate);
        }
    }

    private IEnumerator fireAndWait()
    {
        canFire = false;
        OnFire();
        if (!FireSound.isPlaying)
        {
            FireSound.Play();
        }
        yield return new WaitForSeconds(FireRate);
        if (heating >= maxHeat || Reloading)
        {
            Debug.Log("YAMERO");
            StartCoroutine(Reload());            
        }
        else
            canFire = true;
    }

    private IEnumerator Reload()
    {
        heatingSliderWeight = (1 / (ReloadTime) * slider.value);        
        
        Debug.Log("Now Reloading");
        Reloading = true;
        //yield return new WaitForSeconds(ReloadTime);
        yield return new WaitUntil(() => curReloadingTime >= ReloadTime);
        curReloadingTime = 0f;
        Debug.Log("Done Reloading");
        heating = 0.0f;
        canFire = true;
    }

    // When player touches gun
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player") && other.IsTouching(boxCollider2D)) {     // Check if other is player
            // pickupSound.Play();            
            string weaponName = gameObject.name;
            string pathName = "Guns/" + weaponName; // ex: Guns/Flamethrower            

            GameObject weapon = Resources.Load<GameObject>(pathName);   // loads weapon prefab from pathname
            weapon.GetComponent<Gun>().isPickedUp = true;

            other.gameObject.GetComponent<PlayerController>().WeaponInventory.Add(weapon);
            Debug.Log("New Weapon added to inventory");
            Destroy(gameObject);
        }
    }
}
