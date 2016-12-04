using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {

	public Button m_Monday;
	public Button m_Tuesday;
	public Button m_Wednesday;
	public Button m_Thursday;
	public Button m_Friday;

	// Use this for initialization
	void OnEnable () {
		m_Monday.interactable = true;
		m_Tuesday.interactable = PlayerPrefs.GetInt ("Monday_Win") == 1;
		m_Wednesday.interactable = PlayerPrefs.GetInt ("Tuesday_Win") == 1;
		m_Thursday.interactable = PlayerPrefs.GetInt ("Wednesday_Win") == 1;
		m_Friday.interactable = PlayerPrefs.GetInt ("Thursday_Win") == 1;
	}
}
