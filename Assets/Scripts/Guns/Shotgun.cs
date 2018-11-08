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

        if (transform.parent.tag == "Player" || transform.parent.tag == "Buildable") {  // isFriendlyBullet defined by where bullet comes from
            projectile.isFriendlyBullet = true;
            projectile2.isFriendlyBullet = true;
            projectile3.isFriendlyBullet = true;
        }
        else {
            projectile.isFriendlyBullet = false;
            projectile2.isFriendlyBullet = false;
            projectile3.isFriendlyBullet = false;
        }

        /*
        var projectile3 = Instantiate(ProjectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        projectile3.GetComponent<Rigidbody2D>().AddForce(projectile3.transform.right * Force);

        var projectile4 = Instantiate(ProjectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        projectile4.GetComponent<Rigidbody2D>().AddForce(projectile4.transform.right * Force);
        */
    }
}
