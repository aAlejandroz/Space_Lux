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
	private bool canFire;
    protected bool canPlaySound;
    private BoxCollider2D boxCollider2D;

    public Slider slider;
    private GameObject buildUIInfo;
    public float ReloadTime = 0.75f;
    private float ReloadFin;
    private bool Reloading;

    public float heating;
    public float maxHeat;


    public void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        isPickedUp = false;
        Reloading = false;
        //slider = GetComponent<Slider>();
        //slider.value = 0;
    }

    public void Start() {
        if (isPickedUp || transform.parent != null) {            
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.R))
        {
            canFire = false;
            StartCoroutine(Reload());
        }

        if (Reloading)
        {
            ReloadFin += Time.deltaTime;
            Debug.Log(ReloadFin);
            if (ReloadFin >= ReloadTime)
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
            heating += Time.deltaTime;
            Debug.Log("Current heat level: " + heating);
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
        if (heating >= maxHeat)
        {
            Debug.Log("YAMERO");
            StartCoroutine(Reload());
        }
        else
            canFire = true;
    }

    private IEnumerator Reload()
    {
        //canFire = false;
        Debug.Log("Now Reloading");
        Reloading = true;
        //yield return new WaitForSeconds(ReloadTime);
        yield return new WaitUntil(() => ReloadFin >= ReloadTime);
        ReloadFin = 0f;
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
