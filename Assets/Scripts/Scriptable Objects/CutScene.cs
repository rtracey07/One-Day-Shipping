using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/CutScene")]
public class CutScene : ScriptableObject {


	public List<CutSceneEvent> Events;

	public IEnumerator RunCutScene()
	{
		LevelManager.Instance.StartCoroutine (LevelManager.Instance.FullScreenFade (false));

		for (int i=0; i<Events.Count; i++) {
			yield return LevelManager.Instance.StartCoroutine (TriggerEvent (i));
		}

		yield return LevelManager.Instance.StartCoroutine (LevelManager.Instance.FullScreenFade (true));
		LevelManager.Instance.m_BlackOut.gameObject.SetActive (false);
	}

	public virtual IEnumerator TriggerEvent( int index)
	{
		yield return new WaitForSeconds (Events[index].timeBeforeDisplaying);
		GameClockManager.Instance.freeze = Events [index].pauseGame;

		foreach (string dialogue in Events[index].dialogue) {
			LevelManager.Instance.RunEvent (Events [index], dialogue);

			if (Events [index].requiresConfirmation) {
				yield return new WaitUntil (() => GameManager.Instance.continueClicked);
				GameManager.Instance.continueClicked = false;
			} else {
				yield return new WaitForSeconds (Events [index].duration);
			}
		}

		LevelManager.Instance.HideTextBox ();
		GameClockManager.Instance.freeze = false;
	}
}
