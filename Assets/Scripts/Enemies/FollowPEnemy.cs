using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// add drop later

[RequireComponent (typeof(Rigidbody2D))]
public class FollowPEnemy : MonoBehaviour
{
    enum State
    {
        FOLLOWING = 0,
        ATTACKING = 1
    }

    public float Speed;
    public float AttackRadius;
    public Gun Gun;

    // Create GameObject List for Player targets 
    private Animator anim;
    [SerializeField] private GameObject target;
    //public List<GameObject> targetList;
    private Rigidbody2D rb2d;
    private State curState;

    // caches
    private Vector2 distance;
    private RaycastHit2D[] attackRadiusColl;
    //private bool foundPlayer;
    [SerializeField]private bool foundTarget;
    private Vector2 targetVec;
    private float targetAngle;
    private Vector2 angleVec;
    private float angle;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Base");  // Default to going after base
        rb2d = GetComponent<Rigidbody2D>();
        curState = State.FOLLOWING;
        foundTarget = false;
    }

    private void FixedUpdate()
    {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Base");
        }
        
        switch (curState)
        {
            case State.FOLLOWING:
                OnFollowing();
                break;
            case State.ATTACKING:
                OnAttacking();
                break;
        }

        attackRadiusColl = Physics2D.CircleCastAll(transform.position, AttackRadius, transform.rotation.eulerAngles);
        for (int i = 0; i < attackRadiusColl.Length && !foundTarget; i++)
        {            
            var collider = attackRadiusColl[i].collider;            

            if (collider.tag == "Player" || (collider.tag == "Buildable" && collider.GetType() == typeof(BoxCollider2D)) || collider.tag == "Base")          
            {                
                curState = State.ATTACKING;
                rb2d.velocity = Vector2.zero;                
                foundTarget = true;
                target = collider.gameObject;
            }            
        }

        if (!foundTarget) {
            curState = State.FOLLOWING;
        }
       
        foundTarget = false;
        SetAnimations();        
    }

    private void OnFollowing()
    {
        distance = target.transform.position - transform.position;
        distance.x = Mathf.Clamp(distance.x, -1.0f, 1.0f);
        distance.y = Mathf.Clamp(distance.y, -1.0f, 1.0f);
        rb2d.velocity = distance * Speed;
    }

    private void OnAttacking()
    {
        rb2d.velocity = Vector2.zero;

        Vector3 offset = target.tag == "Buildable" ? new Vector3(0, 1, 0) : Vector3.zero;   // Calculate offset of 1 unit down for turrets

        targetVec = target.transform.position - (Gun.transform.position + offset);  // add offset
        targetAngle = Mathf.Atan2(targetVec.y, targetVec.x) * Mathf.Rad2Deg;
        Gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
        Gun.Use();
    }

    private void setAnimInput(float x, float y)
    {
        anim.SetFloat("xInput", x);
        anim.SetFloat("yInput", y);
    }

    private void SetAnimations()
    {
        angleVec = target.transform.position - transform.position;
        angle = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg;

        if (distance != Vector2.zero)
        { // Is player moving?
            anim.StopPlayback();
        }
        else
        {
            anim.StartPlayback();
        }

        if (angle <= 33.0f && angle >= -33.0f)
        { // Right
            setAnimInput(1, 0);
        }
        else if (angle <= 66.0f && angle > 33.0f)
        { // Up right
            setAnimInput(1, 1);
        }
        else if (angle <= 123.0f && angle > 66.0f)
        { // Up
            setAnimInput(0, 1);
        }
        else if (angle <= 156.0f && angle > 123.0f)
        { // Up left
            setAnimInput(-1, 1);
        }
        else if (angle >= Mathf.PI || angle < -156.0f)
        { // Left
            setAnimInput(-1, 0);
        }
        else if (angle >= -156.0f && angle < -123.0f)
        { // Down left
            setAnimInput(-1, -1);
        }
        else if (angle >= -123.0f && angle < -66.0f)
        { // Down
            setAnimInput(0, -1);
        }
        else if (angle >= -66.0f && angle < -33.0f)
        { // Down right
            setAnimInput(1, -1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}