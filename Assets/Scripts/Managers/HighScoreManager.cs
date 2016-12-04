using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

	//reference to the highscore name textboxes
	public Text name_Monday;
	public Text name_Tuesday;
	public Text name_Wednesday;
	public Text name_Thursday;
	public Text name_Friday;

	//reference to the highscore score textboxes
	public Text score_Monday;
	public Text score_Tuesday;
	public Text score_Wednesday;
	public Text score_Thursday;
	public Text score_Friday;

	//singleton pattern:
	private static HighScoreManager _Instance;
	public static HighScoreManager Instance {
		get { 
			return _Instance;
		}
	}

	void Awake(){
		if (_Instance == null)
			_Instance = this;
		else {
			DestroyImmediate (this);
		}

		//set scores:
		score_Monday.text = PlayerPrefs.GetInt ("Monday_Score").ToString();
		score_Tuesday.text = PlayerPrefs.GetInt ("Tuesday_Score").ToString();
		score_Wednesday.text = PlayerPrefs.GetInt ("Wednesday_Score").ToString();
		score_Thursday.text = PlayerPrefs.GetInt ("Thursday_Score").ToString();
		score_Friday.text = PlayerPrefs.GetInt ("Friday_Score").ToString();

		//set names:
		name_Monday.text = PlayerPrefs.GetString("Monday_Name").ToString();
		name_Tuesday.text = PlayerPrefs.GetString("Tuesday_Name").ToString();
		name_Wednesday.text = PlayerPrefs.GetString("Wednesday_Name").ToString();
		name_Thursday.text = PlayerPrefs.GetString("Thursday_Name").ToString();
		name_Friday.text = PlayerPrefs.GetString("Friday_Name").ToString();
	}

}
