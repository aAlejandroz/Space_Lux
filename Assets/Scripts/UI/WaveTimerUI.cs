using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimerUI : MonoBehaviour {

	public Text TextField;
	public float TimeUntilWarning;

	public void StartCountdown(float duration) {
        //StartCoroutine(Wait());
        StartCoroutine(waitAndDisplayWarning(duration));        
        StartCoroutine(waitAndDisplayFight(duration));
	}
    
	private IEnumerator waitAndDisplayWarning(float duration) {
		yield return new WaitForSeconds(duration - TimeUntilWarning);
		TextField.text = "Enemies spawning in " + TimeUntilWarning + "!";
		StartCoroutine(waitAndEraseText(1.5f));
	}
    
    /*
    private IEnumerator Wait() { 

    }
    */

    private IEnumerator waitAndDisplayFight(float duration) {
		yield return new WaitForSeconds(duration);
		TextField.text = "Fight!";
		StartCoroutine(waitAndEraseText(1.5f));
	}

	private IEnumerator waitAndEraseText(float duration) {
		yield return new WaitForSeconds(duration);
		TextField.text = "";
	}

    /*
    private void Update() {
        wait()
    }
    */
}
