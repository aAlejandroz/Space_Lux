using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour {

    [SerializeField]
    private float vertical, horizontal;
    public float MovementSpeed;
    public float AttackSpeed;
    public float AttackDamage;
    private bool isTargetInRange;
    public Animator anim;
    public GameObject target;
    private Vector2 targetVec;
    private Rigidbody2D rb2d;

    private void Awake() {        
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {       

        targetVec = target.transform.position - transform.position;
        targetVec.x = Mathf.Clamp(targetVec.x, -1.0f, 1.0f);
        targetVec.y = Mathf.Clamp(targetVec.y, -1.0f, 1.0f);
        rb2d.velocity = targetVec * MovementSpeed;

        horizontal = targetVec.x;
        vertical = targetVec.y;
        
        SetAnimations();
    }

    private void setAnimInput(float x, float y) {
        anim.SetFloat("xInput", x);
        anim.SetFloat("yInput", y);
    }

    protected void SetAnimations() {
        if (horizontal >= 0.5f && (vertical <= 0.5f && vertical >= -0.5f)) { // Right
            Debug.Log("Right");
            setAnimInput(1, 0);
        }
        else if (horizontal >= 0.5f && vertical >= 0.5f) { // Up right
            setAnimInput(1, 1);
        }
        else if (vertical >= 0.5f && (horizontal <= 0.5f && horizontal >= -0.5f)) { // Up
            setAnimInput(0, 1);
        }
        else if (vertical >= 0.5f && (horizontal <= -0.5f)) { // Up left
            setAnimInput(-1, 1);
        }
        else if (horizontal <= -0.5f && (vertical <= 0.5f && vertical >= -0.5f)) { // Left
            setAnimInput(-1, 0);
        }
        else if (vertical <= -0.5f && horizontal <= -0.5f) { // Down left
            setAnimInput(-1, -1);
        }
        else if (vertical <= -0.5f && (horizontal <= 0.5f && horizontal >= -0.5f)) { // Down
            setAnimInput(0, -1);
        }
        else if (vertical <= -0.5f && horizontal >= 0.5f) { // Down right
            setAnimInput(1, -1);
        }
    }
}

