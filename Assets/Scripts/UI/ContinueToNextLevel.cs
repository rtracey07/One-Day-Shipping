using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * The MonoBehaviour script for the continue to next level button
 */
public class ContinueToNextLevel : MonoBehaviour {

    /**
     * Enable or disable the continue button
     */
    void OnEnable()
    {
        this.GetComponent<Button>().interactable = LevelManager.Instance.CheckWinState();
    }

    /**
     * The method called when the user clicks the continue button
     */
	public void OnClick(){
		LevelManager.Instance.StartNextLevel ();
	}
}
