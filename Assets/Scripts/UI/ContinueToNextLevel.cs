using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ContinueToNextLevel : MonoBehaviour {

	void OnEnable()
	{
		this.GetComponent<Button> ().interactable = LevelManager.Instance.CheckWinState ();
	}

	public void OnClick(){
		LevelManager.Instance.StartNextLevel ();
	}
}
