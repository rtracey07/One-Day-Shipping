﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButton : MonoBehaviour {

	public string sceneName;

	public void OnClick()
	{
		StartCoroutine (LoadScene());
	}

	public IEnumerator LoadScene(){
		yield return new WaitForSeconds (1.0f);
		GameManager.Instance.currLevelIndex = 0;
		SceneManager.LoadScene (sceneName);
		yield return null;
	}
}
