using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour {


	public void OnClick(){

		//deactive escape window:
		GameObject escapeUI = GameObject.FindGameObjectWithTag("EscapeUI");
		FindObjectOfType<EscapeMenuManager>().Deactivate (); //deactive escape window
		SceneManager.LoadScene ("FrontEnd");
	}
}
