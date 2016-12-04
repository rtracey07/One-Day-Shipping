using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour {

	public void OnClick(){
		GameObject escapeUI = GameObject.FindGameObjectWithTag("EscapeUI");
		escapeUI.GetComponent<EscapeMenuManager> ().Deactivate (); //deactive escape window
		SceneManager.LoadScene ("FrontEnd");
	}
}
