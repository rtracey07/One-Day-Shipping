using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


/**
 * The MonoBehaviour script for the level retry
 */
public class RetryLevel : MonoBehaviour {
    
    /**
     * The method called when the user clicks the retry level button
     */
	public void OnClick(){

		if (LevelManager.Instance.CheckWinState ())
			GameManager.Instance.currLevelIndex--;

		LevelManager.Instance.StartNextLevel ();
	}
}
