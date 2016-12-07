using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/CutScene")]
public class CutScene : ScriptableObject {


	public List<CutSceneEvent> Events;

	/// <summary>
	/// displays cut scenes that run between levels
	/// </summary>
	/// <returns>The cut scene.</returns>
	public IEnumerator RunCutScene()
	{
		LevelManager.Instance.m_CutsceneText.text = "";
		yield return LevelManager.Instance.StartCoroutine (LevelManager.Instance.FullScreenFade (false));
		// checks if skip button is clicked
		for (int i=0; i<Events.Count && !GameManager.Instance.skipClicked; i++) {
			yield return LevelManager.Instance.StartCoroutine (TriggerEvent (i));
		}
		//buttons are not clicked
		GameManager.Instance.skipClicked = false;
		GameManager.Instance.continueClicked = false;
		yield return LevelManager.Instance.StartCoroutine (LevelManager.Instance.FullScreenFade (true));
		LevelManager.Instance.m_BlackOut.gameObject.SetActive (false);
	}

	/// <summary>
	/// Triggers UI events.
	/// </summary>
	/// <returns>The event.</returns>
	/// <param name="index">Index.</param>
	public virtual IEnumerator TriggerEvent( int index)
	{
		yield return new WaitForSeconds (Events[index].timeBeforeDisplaying);
		GameClockManager.Instance.freeze = Events [index].pauseGame;

		if(Events[index].sound != null)
			AudioManager.Instance.PlaySoundEffect(Events [index].sound, Events[index].soundVolume);

		// displays dialog buttons based on dialog type
		foreach (string dialogue in Events[index].dialogue) {
			LevelManager.Instance.RunEvent (Events [index], dialogue);

			if (Events [index].requiresConfirmation) {
				yield return new WaitUntil (() => GameManager.Instance.continueClicked);
				GameManager.Instance.continueClicked = false;

			} else {
				yield return new WaitForSeconds (Events [index].duration);
			}

			if (GameManager.Instance.skipClicked) {
				yield break;
			}
		}

		LevelManager.Instance.HideTextBox ();
		GameClockManager.Instance.freeze = false;
	}
}
