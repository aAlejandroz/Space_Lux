using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	public float MovementSpeed;
    public Transform PlayerHands;
    //public Item FireSlot;
	public Camera Camera;

	private Animator anim;
	private Rigidbody2D rb;
	private Vector2 movementInput;
	private Vector2 mouseVec;
	private float mouseAngle;
	private bool isFiring;
    public GameObject currentWeapon;
    public GameObject[] WeaponInventory;

    public PlayerController() {
        WeaponInventory = new GameObject[3];
    }

    public Gun weaponComponent;

    private int weaponIndex = 0;

    private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        switchWeapons(0);
    }
    
    private void switchWeapons(int index) {
        if (currentWeapon != null) {
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(WeaponInventory[index], PlayerHands);
        weaponComponent = currentWeapon.GetComponent<Gun>();

    }

    private void Update() {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        isFiring = Input.GetButton("Fire1");

        if (Input.GetKeyDown("1"))
            switchWeapons(0);

        if (Input.GetKeyDown("2"))
            switchWeapons(1);

        if (Input.GetKeyDown("3"))
            switchWeapons(2);
    }

    private void FixedUpdate() {
		rb.velocity = movementInput * MovementSpeed;

        /*
		if (isFiring && FireSlot != null) {
			FireSlot.Use();
		}
        */
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
