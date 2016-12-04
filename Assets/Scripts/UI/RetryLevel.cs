using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RetryLevel : MonoBehaviour {

	public void OnClick(){
		SceneManager.LoadScene ("InGame");
	}
}
