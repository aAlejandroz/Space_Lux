using UnityEngine;

public class Flamethrower : Gun
{
    public float Force;

    protected override void OnFire()
    {
        var projectile = Instantiate(ProjectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * Force);

        if (transform.parent.tag == "Player") {  // isFriendlyBullet defined by where bullet comes from
            projectile.isPlayerBullet = true;
        }
        else if (transform.parent.tag == "Buildable") {
            projectile.isTurretBullet = true;
        } else {

        }

    }
}
