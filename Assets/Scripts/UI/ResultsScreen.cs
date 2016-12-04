using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultsScreen : MonoBehaviour {

	public float tallyLength;
	public Text delivered;
	public Text destroyed;
	public Text cars;
	public Text dogs;
	public Text postmen;
	public Text total;
	public GameObject newHighscoreWindow;
	public GameObject optionsWindow;
	public GameObject resultsCalculationWindow;
	public Text newHighScoreNameInput;

	private int totalVal;

	// Use this for initialization
	void Start () {
		newHighscoreWindow.SetActive (false);
		optionsWindow.SetActive (false);
		StartCoroutine (TallyScore ());
	}

	IEnumerator TallyScore()
	{

		yield return new WaitForSeconds (1.0f);

		int currScore = GameManager.Instance.stats.packagesDelivered * 100;
		totalVal = 0;

		yield return StartCoroutine (Tally (currScore, delivered, total));

		currScore = GameManager.Instance.stats.packagesDestroyed * -100;
		yield return StartCoroutine (Tally (currScore, destroyed, total));

		currScore = GameManager.Instance.stats.carsHit * -10;
		yield return StartCoroutine (Tally (currScore, cars, total));

		currScore = GameManager.Instance.stats.dogsHit * -20;
		yield return StartCoroutine (Tally (currScore, dogs, total));

		currScore = GameManager.Instance.stats.postmenHit * -10;
		yield return StartCoroutine (Tally (currScore, postmen, total));

		float time = 0.0f;
		float originalFontSize = total.fontSize;

		yield return new WaitForSeconds(1.5f);

		CheckForHighscore ();

	}



	IEnumerator Tally(int currScore, Text currText, Text currTotalText)
	{
		float time = 0.0f;
		float currTotal = totalVal;
		totalVal += currScore;

		if(currScore < 0)
			currText.color = Color.red;

		while (time <= tallyLength) {
			currText.text = ((int)(Mathf.Lerp (0, currScore, time / tallyLength))).ToString ();
			currTotalText.text = ((int)(Mathf.Lerp (currTotal, totalVal, time / tallyLength))).ToString ();
			time += GameClockManager.Instance.time;
			yield return null;
		}

		currText.text = currScore.ToString();
		currTotalText.text = totalVal.ToString();
	}


	void CheckForHighscore(){
		
		resultsCalculationWindow.GetComponent<Canvas> ().enabled = false;

		int currHighscore = PlayerPrefs.GetInt (LevelManager.Instance.levelData.name + "_Score");

		//store new highscore value if a new highscore has been achieved
		if (totalVal > currHighscore) {
			PlayerPrefs.SetInt (LevelManager.Instance.levelData.name + "_Score", totalVal);
			newHighscoreWindow.SetActive (true);
		}

		optionsWindow.SetActive (true);
	}

	public void OnClick(){
		if (newHighScoreNameInput.text != null)
			PlayerPrefs.SetString (LevelManager.Instance.levelData.name + "_Name", newHighScoreNameInput.text);
		else {
			PlayerPrefs.SetString (LevelManager.Instance.levelData.name + "_Name", "unnamed");
		}
		newHighscoreWindow.SetActive (false);
		optionsWindow.SetActive (true);
	}
}
