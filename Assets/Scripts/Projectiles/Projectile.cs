using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour {
    public bool isFriendlyBullet;
}
