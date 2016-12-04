using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultsScreen : MonoBehaviour {

	[Header("Results Windows")]
	public GameObject m_HighScoreUI;
	public GameObject m_OptionsUI;
	public GameObject m_ResultsUI;

	[Header("Results Fields")]
	public float tallyLength;
	public Text delivered;
	public Text destroyed;
	public Text cars;
	public Text dogs;
	public Text postmen;
	public Text total;

	[Header("New High Score Name Field")]
	public Text NameInput;

	private int totalVal;

	// Use this for initialization
	void Start () {
		m_HighScoreUI.SetActive (false);
		m_OptionsUI.SetActive (false);

		if (LevelManager.Instance.CheckWinState ())
			PlayerPrefs.SetInt (LevelManager.Instance.levelData.name + "_Win", 1);

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
		
		m_ResultsUI.GetComponent<Canvas> ().enabled = false;

		int currHighscore = PlayerPrefs.GetInt (LevelManager.Instance.levelData.name + "_Score");

		//store new highscore value if a new highscore has been achieved
		if (totalVal > currHighscore) {
			PlayerPrefs.SetInt (LevelManager.Instance.levelData.name + "_Score", totalVal);
			m_HighScoreUI.SetActive (true);
		} else {
			m_OptionsUI.SetActive (true);
		}
	}

	public void OnClick(){
		if (NameInput.text != null)
			PlayerPrefs.SetString (LevelManager.Instance.levelData.name + "_Name", 
				NameInput.text.Length > 7 ?  NameInput.text.Substring(0, 7) : NameInput.text);
		else {
			PlayerPrefs.SetString (LevelManager.Instance.levelData.name + "_Name", "unnamed");
		}
		m_HighScoreUI.SetActive (false);
		m_OptionsUI.SetActive (true);
	}
}
