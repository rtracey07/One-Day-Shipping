using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RetryLevel : MonoBehaviour {

	public void OnClick(){

		if (LevelManager.Instance.CheckWinState ())
			GameManager.Instance.currLevelIndex--;

		LevelManager.Instance.StartNextLevel ();
	}
}
