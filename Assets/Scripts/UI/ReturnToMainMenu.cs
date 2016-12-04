using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour {


	public void OnClick(){

		//deactive escape window:
		GameObject escapeUI = GameObject.FindGameObjectWithTag("EscapeUI");
		if (escapeUI != null) {
			escapeUI.GetComponent<EscapeMenuManager> ().Deactivate (); 
		}

		SceneManager.LoadScene ("FrontEnd");
	}
}
