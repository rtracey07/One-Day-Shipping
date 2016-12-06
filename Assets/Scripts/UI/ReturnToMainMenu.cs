using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * The MonoBehaviour script for returning to the main menu
 */
public class ReturnToMainMenu : MonoBehaviour {


    /**
     * The function called when the user clicks
     */
	public void OnClick() {        
		GameManager.Instance.Reset();
        //deactive escape window
        FindObjectOfType<EscapeMenuManager>().Deactivate (); 
		SceneManager.LoadScene ("FrontEnd");
	}
}
