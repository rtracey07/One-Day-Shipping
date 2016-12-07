using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour {

	private GameObject m_ActiveCanvas;

	[Header("Parent of the Escape Menus")]
	public GameObject m_EscapeMenu;

	[Header("Sub Menus for the Escape Menu")]
	public GameObject m_Main;
	public GameObject m_WantToQuit;
	public GameObject m_WantToMainMenu;
	public GameObject m_Settings;

	public bool active;

	void Awake(){
		m_ActiveCanvas = m_Main;
		active = false;
	}
	
	// Set UI window active and freeze game when Escape key is pressed
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			AccessEscapeMenu ();
		}
	}

	/* Set Escape Menu visible */
	public void AccessEscapeMenu(){
		if (SceneManager.GetActiveScene ().name != "FrontEnd" || SceneManager.GetActiveScene ().name != "CutScene") {
			if (!active) {
				m_EscapeMenu.SetActive (true);
				GameClockManager.Instance.freeze = true; //pause the game
				active = true;
			} else {
				m_EscapeMenu.SetActive (false);
				m_ActiveCanvas = m_Main;
				GameClockManager.Instance.freeze = false; //pause the game
				active = false;
			}
		}
	}

	/* Transition to main. */
	public void MainTransition()
	{
		m_ActiveCanvas.SetActive(false);
		m_Main.SetActive(true);
		m_ActiveCanvas = m_Main;
	}

	/* Transition to Are You Sure? */
	public void WantToQuitTransition(){
		m_ActiveCanvas.SetActive (false);
		m_WantToQuit.SetActive (true);
		m_ActiveCanvas = m_WantToQuit;
	}

	/* Transition to Are You Sure? */
	public void WantToMainMenuTransition(){
		m_ActiveCanvas.SetActive (false);
		m_WantToMainMenu.SetActive (true);
		m_ActiveCanvas = m_WantToMainMenu;
	}

	/* Transition to Settings. */
	public void SettingsTransition(){
		m_ActiveCanvas.SetActive (false);
		m_Settings.SetActive (true);
		m_ActiveCanvas = m_Settings;
	}

	/* Deactivate Menu. */
	public void Deactivate(){
		m_EscapeMenu.SetActive (false);
		m_ActiveCanvas.SetActive (false);
		Destroy (this.gameObject);
	}
}
