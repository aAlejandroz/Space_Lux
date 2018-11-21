using UnityEngine;

public class Shotgun : Gun{

    public float Force;
    public float Spread;
    private Quaternion rotation;

    protected override void OnFire()
    {
        rotation = SpawnPoint.rotation;

        var projectile = Instantiate(ProjectilePrefab, SpawnPoint.position, rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * Force);

        rotation *= Quaternion.Euler(1.0f, 1.0f, Spread);
        var projectile2 = Instantiate(ProjectilePrefab, SpawnPoint.position, rotation);
        projectile2.GetComponent<Rigidbody2D>().AddForce(projectile2.transform.right * Force);

        rotation *= Quaternion.Euler(1.0f, 1.0f, Spread);
        var projectile3 = Instantiate(ProjectilePrefab, SpawnPoint.position, rotation);
        projectile3.GetComponent<Rigidbody2D>().AddForce(projectile3.transform.right * Force);        

        if (transform.parent.tag == "PlayerAxis") {  // isFriendlyBullet defined by where bullet comes from
            projectile.isPlayerBullet = true;
            projectile2.isPlayerBullet = true;
            projectile3.isPlayerBullet = true;
        }
        else if (transform.parent.tag == "Buildable") {
            projectile.isTurretBullet = true;
            projectile2.isTurretBullet = true;
            projectile3.isTurretBullet = true;
        }
        else {

        }

        /*
        var projectile3 = Instantiate(ProjectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        projectile3.GetComponent<Rigidbody2D>().AddForce(projectile3.transform.right * Force);

        var projectile4 = Instantiate(ProjectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        projectile4.GetComponent<Rigidbody2D>().AddForce(projectile4.transform.right * Force);
        */
    }
}
