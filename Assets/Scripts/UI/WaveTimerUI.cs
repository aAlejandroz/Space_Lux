using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimerUI : MonoBehaviour {

	public Text TextField;
	public float TimeUntilWarning;
    public float remainingTime;
    public bool isCountingDown = false;

	public void StartCountdown(float duration) {      
        remainingTime = TimeUntilWarning;
        StartCoroutine(waitAndDisplayWarning(duration));               
	}
    
	private IEnumerator waitAndDisplayWarning(float duration) {
		yield return new WaitForSeconds(duration - TimeUntilWarning);
        isCountingDown = true;
        //TextField.text = "Enemies spawning in " + TimeUntilWarning + "!";      
		//StartCoroutine(waitAndEraseText(1.5f));
	}

    private void Update() {
        if (isCountingDown) {
            TextField.text = remainingTime.ToString("0");
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f) {
                TextField.text = "";
                StartCoroutine(waitAndDisplayFight());
                isCountingDown = false;
            }
        }
    }

    private IEnumerator waitAndDisplayFight() {
		//yield return new WaitForSeconds(duration);
		TextField.text = "Fight!";
		StartCoroutine(waitAndEraseText(1.5f));
        yield break;
	}

	private IEnumerator waitAndEraseText(float duration) {
		yield return new WaitForSeconds(duration);
		TextField.text = "";
	}
}
