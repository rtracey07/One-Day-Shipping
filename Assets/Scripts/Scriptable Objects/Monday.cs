using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Data/Level/Monday")]
public class Monday : Level {

	public override IEnumerator RunLevel()
	{
		LevelManager.Instance.SetPickup ();

		yield return new WaitForSeconds (2);

		GameClockManager.Instance.freeze = events [0].pauseGame;

		foreach (string dialogue in events[0].dialogue) {
			LevelManager.Instance.RunEvent (events [0], dialogue);

			if (events [0].requiresConfirmation) {
				yield return new WaitUntil (() => GameManager.Instance.continueClicked);
				GameManager.Instance.continueClicked = false;
			} else {
				yield return new WaitForSeconds (events [0].duration);
			}
		}

		LevelManager.Instance.HideTextBox ();
		GameClockManager.Instance.freeze = false;

		for (int i = 0; i < 5; i++) {
			yield return new WaitUntil (() => GameManager.Instance.hasPackage);

			LevelManager.Instance.SetDropoff ();

			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);

			LevelManager.Instance.SetPickup ();
		}
	}
}
