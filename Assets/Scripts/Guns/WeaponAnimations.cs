using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimations : MonoBehaviour {

    public Animator anim;
    public SpriteRenderer _spriteRenderer;
    public Sprite gunUp;
    public Sprite gunDiagUp;
    public Sprite gunDown;
    public Sprite gunDiagDown;
    public Sprite gunSide;

    public void Start() {
        anim = GetComponentInParent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    public void Update() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SouthWalk_Gun")) {
            Debug.Log("South walk");
            _spriteRenderer.sprite = gunDown;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("DiagDown_Left")) {
            _spriteRenderer.sprite = gunDiagDown;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

}
