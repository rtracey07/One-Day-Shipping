using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButton : MonoBehaviour {

	public string sceneName;
	public bool specificLevel;
	public int levelIndex;
	public void OnClick()
	{
		StartCoroutine (LoadScene());
	}

	public IEnumerator LoadScene(){
		yield return new WaitForSeconds (1.0f);

		if (specificLevel) {
			GameManager.Instance.currLevelIndex = levelIndex;
		}
		SceneManager.LoadScene (sceneName);
		yield return null;
	}
}
