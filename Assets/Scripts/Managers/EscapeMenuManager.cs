﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour {

	private GameObject m_ActiveCanvas;

	//reference to other UI windows:
	public GameObject m_Main;
	public GameObject m_WantToQuit;
	public GameObject m_WantToMainMenu;
	public GameObject m_Settings;

	public bool active;

	void Awake(){
		m_ActiveCanvas = m_Main;
		m_ActiveCanvas.SetActive (false);
		active = false;
	}
	
	// Set UI window active and freeze game when Escape key is pressed
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			AccessEscapeMenu ();
		}
	}

	public void AccessEscapeMenu(){
		if (SceneManager.GetActiveScene ().name != "FrontEnd") {
			if (!active) {
				m_ActiveCanvas.SetActive (true);
				GameClockManager.Instance.freeze = true; //pause the game
				active = true;
			} else {
				m_ActiveCanvas.SetActive (false);
				GameClockManager.Instance.freeze = false; //pause the game
				active = false;
			}
		}
	}

	public void MainTransition()
	{
		m_ActiveCanvas.SetActive(false);
		m_Main.SetActive(true);
		m_ActiveCanvas = m_Main;
	}

	public void WantToQuitTransition(){
		m_ActiveCanvas.SetActive (false);
		m_WantToQuit.SetActive (true);
		m_ActiveCanvas = m_WantToQuit;
	}

	public void WantToMainMenuTransition(){
		m_ActiveCanvas.SetActive (false);
		m_WantToMainMenu.SetActive (true);
		m_ActiveCanvas = m_WantToMainMenu;
	}

	public void SettingsTransition(){
		m_ActiveCanvas.SetActive (false);
		m_Settings.SetActive (true);
		m_ActiveCanvas = m_Settings;
	}

	public void Deactivate(){
		m_ActiveCanvas.SetActive (false);
		active = false;
	}

}
