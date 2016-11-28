using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/CutScene")]
public class CutScene : ScriptableObject {


	public List<CutSceneEvent> Events;

	public IEnumerator RunCutScene()
	{
		foreach (InGameEvent curr in Events) {
			yield return LevelManager.Instance.StartCoroutine (TriggerEvent (0));
		}
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
