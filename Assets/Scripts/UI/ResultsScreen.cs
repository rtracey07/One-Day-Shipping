using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * The MonoBehaviour script for
 */
public class ResultsScreen : MonoBehaviour {

    //  The UIs
	[Header("Results Windows")]
	public GameObject m_HighScoreUI;
	public GameObject m_OptionsUI;
	public GameObject m_ResultsUI;
    
    //  The Results feilds
	[Header("Results Fields")]
	public float tallyLength;
	public Text delivered;
	public Text destroyed;
	public Text cars;
	public Text dogs;
	public Text postmen;
	public Text total;

    //  The name input feild
	[Header("New High Score Name Field")]
	public Text NameInput;

    //  The total value
	private int totalVal;

    /**
     * Use this for initialization 
     */
    void Start () {
		m_HighScoreUI.SetActive (false);
		m_OptionsUI.SetActive (false);

		if (LevelManager.Instance.CheckWinState ())
			PlayerPrefs.SetInt (LevelManager.Instance.levelData.name + "_Win", 1);

		StartCoroutine (TallyScore ());
	}

    /**
     * Tallys all the users scores
     */
	IEnumerator TallyScore()
	{
        //  Pause
		yield return new WaitForSeconds (1.0f);

        //  Tally up the scores
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

        //  Pause to show user score
		yield return new WaitForSeconds(1.5f);

        //  Check if user has set a high score
		CheckForHighscore ();
	}

    /**
     * Tally a single score
     */
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

    /**
     * Checks if the users score is a HighScore
     */
	void CheckForHighscore(){
		
		m_ResultsUI.SetActive (false);

		int currHighscore = PlayerPrefs.GetInt (LevelManager.Instance.levelData.name + "_Score");

		//  Store new highscore value if a new highscore has been achieved
		if (totalVal > currHighscore) {
			PlayerPrefs.SetInt (LevelManager.Instance.levelData.name + "_Score", totalVal);
			m_HighScoreUI.SetActive (true);
		} else {
			m_OptionsUI.SetActive (true);
		}
	}

    /**
     * The method called when the user submits their name for a HighScore
     *
     */
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
