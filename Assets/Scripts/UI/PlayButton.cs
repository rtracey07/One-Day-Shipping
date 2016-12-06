using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * The MonoBehaviour script for the play button
 */
public class PlayButton : MonoBehaviour {

	public string sceneName;
	public bool specificLevel;
	public int levelIndex;
    
    /**
     * The method called when the user clicks the play button
     */
	public void OnClick()
	{
		StartCoroutine (LoadScene());
	}

    /**
     * Loads the first scene
     */
	public IEnumerator LoadScene(){
		yield return new WaitForSeconds (1.0f);

		if (specificLevel) {
			GameManager.Instance.currLevelIndex = levelIndex;
		}
		SceneManager.LoadScene (sceneName);
		yield return null;
	}
}
