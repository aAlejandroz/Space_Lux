using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Damageable {

    public AudioClip HurtSound;
	public HeartUI HeartUI;
    private AudioSource audioSource;

	public PlayerHealth() {
		isInvincible = false;
	}

    private void Awake() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    protected override void OnDamaged(float damage) {
        if (!isInvincible) {
            audioSource.PlayOneShot(HurtSound);
            CurHP -= damage;                      
			HeartUI.UpdateHearts();
            GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
            StartCoroutine(WaitAndChangeColor());
            StartCoroutine(setInvincibleAndWait());
		}
	}

    public void IncreaseHealth(float incre)
    {
        if (CurHP == MaxHP)
        {
            Debug.Log("Waste of HP Token");
        }
        else
        {
            CurHP += incre;
            if (checkMax())
            {
                CurHP = MaxHP;
            }
            HeartUI.UpdateHearts();
        }

    }

    protected override void OnDestroyed() {
        // Gameover scene
        Debug.Log("GAMEOVER");
        SceneManager.LoadScene("GameOver");
        Cursor.visible = true;
        Destroy(gameObject);

	}

    public override IEnumerator WaitAndChangeColor() {
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public bool checkMax()
    {
        if (CurHP > MaxHP)
        {
            return true;
        }
        else
            return false;
    }
}

