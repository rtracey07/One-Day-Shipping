using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour {


	public void OnClick(){

		//deactive escape window:
		GameManager.Instance.Reset();
		FindObjectOfType<EscapeMenuManager>().Deactivate (); //deactive escape window
		SceneManager.LoadScene ("FrontEnd");
		escapeUI.SetActive (false);
	}
}
