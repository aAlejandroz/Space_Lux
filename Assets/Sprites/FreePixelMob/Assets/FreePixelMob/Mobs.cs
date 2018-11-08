using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class Mobs : MonoBehaviour
{
	static int AnimatorWalk = Animator.StringToHash("Walk");
	static int AnimatorAttack = Animator.StringToHash("Attack");
    [SerializeField]
    private Vector2 targetVec;
    private Rigidbody2D rb2d;
    public GameObject target;
    public float MovementSpeed = 2.5f;

    Animator _animator;
	void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
	}
	void Start()
	{
		StartCoroutine(Animate());
        target = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {       
        targetVec = target.transform.position - transform.position;
        targetVec.x = Mathf.Clamp(targetVec.x, -1.0f, 1.0f);
        targetVec.y = Mathf.Clamp(targetVec.y, -1.0f, 1.0f);
        rb2d.velocity = targetVec * MovementSpeed;

        if (targetVec == Vector2.zero) {
            _animator.StartPlayback();
        }
        else {
            _animator.StopPlayback();
        }
    }
    IEnumerator Animate()
	{
		//yield return new WaitForSeconds(5f);
		while (true)
		{
			_animator.SetBool(AnimatorWalk, true);
			yield return new WaitForSeconds(1f);

			//_animator.transform.localScale = new Vector3(19, 19, 1);
			yield return new WaitForSeconds(1f);

			_animator.SetBool(AnimatorWalk, false);
			yield return new WaitForSeconds(1f);

			_animator.SetTrigger(AnimatorAttack);
			yield return new WaitForSeconds(1f);

			_animator.SetTrigger(AnimatorAttack);
			yield return new WaitForSeconds(1f);

			_animator.SetTrigger(AnimatorAttack);
			yield return new WaitForSeconds(5f);
		}
	}
}
