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


	// Use this for initialization
	void Start () {
		StartCoroutine (TallyScore ());
	}

	IEnumerator TallyScore()
	{
		float time = 0.0f;
		int currScore = GameManager.Instance.stats.packagesDelivered * 100;
		int totalVal = currScore;

		while (time <= tallyLength) {
			delivered.text = ((int)(Mathf.Lerp (0, currScore, time / tallyLength))).ToString ();
			total.text = ((int)(Mathf.Lerp (0, totalVal, time / tallyLength))).ToString ();
			time += GameClockManager.Instance.time;
			yield return null;
		}

		delivered.text = currScore.ToString();
		total.text = currScore.ToString();

		time = 0.0f;

	}
}
