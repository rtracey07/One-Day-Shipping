using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButton : MonoBehaviour {

	public void OnClick()
	{
		StartCoroutine (LoadScene());
	}

	public IEnumerator LoadScene(){
		yield return new WaitForSeconds (1.0f);
		SceneManager.LoadScene ("CutScene");
		yield return null;
	}
}
