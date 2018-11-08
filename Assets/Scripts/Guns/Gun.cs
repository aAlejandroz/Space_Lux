using System.Collections;
using UnityEngine;

public abstract class Gun : Item {

	public float FireRate;
	public Projectile ProjectilePrefab; // Has events for on collide
	public Transform SpawnPoint;
    public AudioSource FireSound;
	private bool canFire;
    protected bool canPlaySound;

    public void Start() {
        if (transform.parent.tag == "Player" || transform.parent.tag == "Buildable") {  // isFriendlyBullet defined by where bullet comes from
            ProjectilePrefab.isFriendlyBullet = true;
        } else {
            ProjectilePrefab.isFriendlyBullet = false;
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
}
