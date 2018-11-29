using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    public float worth = 20.0f;
    public float speed;
    private bool isMoving = false;
    private AudioSource pickupSound;
    private Transform target;
    [SerializeField]
    private CircleCollider2D circleCollider;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    // Awake function
    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        pickupSound = GetComponent<AudioSource>();
    }

    // Update function
    public void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    // When player enters circle collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Player") && this.tag.Equals("Health"))
        {
            other.gameObject.GetComponent<PlayerHealth>().IncreaseHealth(worth);
            audioManager.PlaySound("Health");
            Destroy(gameObject);
        }
        
    }
    // Function called when player enters circle collider
    public void MoveToPlayer(Transform player)
    {
        target = player;
        isMoving = true;
    }
}
