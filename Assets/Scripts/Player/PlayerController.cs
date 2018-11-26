using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Two modes, shooting & building
    // Press "q to switch between mode" (onkeydown)
    public enum Mode { SHOOTING_MODE, BUILDING_MODE };

    public Mode mode;
    private int weaponIndex = 0;    
    private float mouseAngle;
    public float MovementSpeed;
    private bool isFiring;

    public Transform PlayerHands; 
	public Camera Camera;
    
	private Animator anim;
	private Rigidbody2D rb;    
	private Vector2 movementInput;
	private Vector2 mouseVec;	
    public GameObject currentWeapon;
    public List<GameObject> WeaponInventory;
    public Gun weaponComponent;
    public GunUI gunDisplay;

    public PlayerController() {
        WeaponInventory = new List<GameObject>();   // initializes weaponInventory for player
    }

    private void Awake() {
        gameObject.GetComponent<BuildManager>().enabled = false;
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        currentWeapon = WeaponInventory[weaponIndex];
        mode = Mode.SHOOTING_MODE;
        switchWeapons(0);        
    }

    // Destroys current weapon & switches weapon according to weapon index
    private void switchWeapons(int index) {        
        if (currentWeapon != null) {
            Destroy(currentWeapon);
        }

        float offset;
        if (WeaponInventory[index].name == "Flamethrower") {    // Adds offset to put flamethrower closer to body   
            Debug.Log("Flamethrower");
            offset = -1.1f;
        } else {
            offset = 0f;
        }

        currentWeapon = Instantiate(WeaponInventory[index], PlayerHands, false);
        currentWeapon.transform.localPosition = new Vector3(0.1f + offset, 0.4f, 0);
        weaponComponent = currentWeapon.GetComponent<Gun>();        
    }

    private void Update() {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Check to see if player pressed "q". If true, switch to building mode. 
        if (Input.GetKeyDown(KeyCode.Q)) {
            SwitchMode();
        }

        if (mode == Mode.SHOOTING_MODE) {
            isFiring = Input.GetButton("Fire1");
            if (Input.GetAxis("Mouse ScrollWheel") != 0f) {     // Player chooses gun with the scroll wheel 
                weaponIndex++;

                if (weaponIndex >= WeaponInventory.Count) {
                    weaponIndex = 0;
                }

                switchWeapons(weaponIndex);
            }

            gunDisplay.UpdateGunDisplay(currentWeapon.GetComponent<SpriteRenderer>().sprite);   // Updates display in UI
        }

        if (mode == Mode.BUILDING_MODE) {
            isFiring = false;
        }
        
    }

    // If mode is shooting mode, switch to building.
    // If mode is building mode, switch to shooting;    
    private void SwitchMode() {           
        if (mode == Mode.SHOOTING_MODE) {
            mode = Mode.BUILDING_MODE;
            gameObject.GetComponent<BuildManager>().enabled = true;
        } else {
            mode = Mode.SHOOTING_MODE;
            gameObject.GetComponent<BuildManager>().enabled = false;
        }  
    }

    private void FixedUpdate() {
		rb.velocity = movementInput * MovementSpeed;

        if (isFiring) {            
            weaponComponent.Use();
        }

        UpdateAnimations();
	}

	private void UpdateAnimations() {
		mouseVec = Input.mousePosition - Camera.WorldToScreenPoint(transform.position);
		mouseAngle = Mathf.Atan2(mouseVec.y, mouseVec.x) * Mathf.Rad2Deg;

		if (movementInput == Vector2.zero) {
			anim.StartPlayback();
		} else {
			anim.StopPlayback();
		}

		if (mouseAngle <= 33.0f && mouseAngle >= -33.0f) { // Right
			setAnimInput(1, 0);
		} else if (mouseAngle <= 66.0f && mouseAngle > 33.0f) { // Up right
			setAnimInput(1, 1);
		} else if (mouseAngle <= 123.0f && mouseAngle > 66.0f) { // Up
			setAnimInput(0, 1);
		} else if (mouseAngle <= 156.0f && mouseAngle > 123.0f) { // Up left
			setAnimInput(-1, 1);
		} else if (mouseAngle >= Mathf.PI || mouseAngle < -156.0f) { // Left
			setAnimInput(-1, 0);
		} else if (mouseAngle >= -156.0f && mouseAngle < -123.0f) { // Down left
			setAnimInput(-1, -1);
		} else if (mouseAngle >= -123.0f && mouseAngle < -66.0f) { // Down
			setAnimInput(0, -1);
		} else if (mouseAngle >= -66.0f && mouseAngle < -33.0f) { // Down right
			setAnimInput(1, -1);
		}
	}

    private void setAnimInput(float x, float y)
    {
        anim.SetFloat("xInput", x);
        anim.SetFloat("yInput", y);
    }
}
