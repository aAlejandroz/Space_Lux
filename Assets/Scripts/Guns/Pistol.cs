using UnityEngine;

public class Pistol : Gun {

	public float Force;

	protected override void OnFire() {
		var projectile = Instantiate(ProjectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * Force);

        if (transform.parent.tag == "PlayerAxis") {  // isFriendlyBullet defined by where bullet comes from
            projectile.isPlayerBullet = true;
        }
        else if (transform.parent.tag == "Buildable") {
            projectile.isTurretBullet = true;
        }
        else {
            projectile.isPlayerBullet = false;
            projectile.isTurretBullet = false;
        }

        //projectile.transform.Translate(Vector2.right * Force * Time.deltaTime);
    }
}
