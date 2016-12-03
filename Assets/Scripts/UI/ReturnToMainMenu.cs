using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour {

	public void OnClick(){
		SceneManager.LoadScene ("FrontEnd");
	}
}
