using System.Collections;
using UnityEngine;

public abstract class Gun : Item {

	public float FireRate;
	public Projectile ProjectilePrefab; // Has events for on collide
	public Transform SpawnPoint;
    public AudioSource FireSound;
    public bool isPickedUp;
	private bool canFire;
    protected bool canPlaySound;
    private BoxCollider2D boxCollider2D;

    public void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        isPickedUp = false;
    }

    public void Start() {
        if (isPickedUp || transform.parent != null) {            
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public Gun() {
		canFire = true;
	}

    protected abstract void OnFire();

	public override void Use() {
		if (canFire) {
			StartCoroutine(fireAndWait());        
		}
    }

	private IEnumerator fireAndWait() {
		canFire = false;
		OnFire();        
        if (!FireSound.isPlaying) {
            FireSound.Play();                
        }        
        yield return new WaitForSeconds(FireRate);       
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
