using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ContinueGameButton : MonoBehaviour {

	public string sceneName;

	public void OnClick()
	{
		StartCoroutine (LoadScene());
	}

	public IEnumerator LoadScene(){
		yield return new WaitForSeconds (1.0f);
		SceneManager.LoadScene (sceneName);
		yield return null;
	}
}
