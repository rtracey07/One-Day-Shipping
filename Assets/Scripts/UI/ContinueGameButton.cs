using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * The MonoBehaviour script for the continue game button
 */
public class ContinueGameButton : MonoBehaviour {

	public string sceneName;

    /**
     * The method called when the continue game button is clicked
     */
	public void OnClick()
	{
		StartCoroutine (LoadScene());
	}

    /**
     * Loads the last scene that was played
     */
	public IEnumerator LoadScene(){
		yield return new WaitForSeconds (1.0f);
		SceneManager.LoadScene (sceneName);
		yield return null;
	}
}
