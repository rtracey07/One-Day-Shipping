using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/CutScene")]
public class CutScene : ScriptableObject {


	public List<CutSceneEvent> Events;
	private Slider volumeControl;

	public IEnumerator RunCutScene()
	{
		yield return LevelManager.Instance.StartCoroutine (LevelManager.Instance.FullScreenFade (false));

		for (int i=0; i<Events.Count && !GameManager.Instance.skipClicked; i++) {
			yield return LevelManager.Instance.StartCoroutine (TriggerEvent (i));
		}

		GameManager.Instance.skipClicked = false;
		GameManager.Instance.continueClicked = false;
		yield return LevelManager.Instance.StartCoroutine (LevelManager.Instance.FullScreenFade (true));
		LevelManager.Instance.m_BlackOut.gameObject.SetActive (false);
		GameObject audio = GameObject.FindGameObjectWithTag ("AudioSlider");
		if (audio != null) {
			volumeControl = audio.GetComponent<Slider> ();
		}
	}

	public virtual IEnumerator TriggerEvent( int index)
	{
		yield return new WaitForSeconds (Events[index].timeBeforeDisplaying);
		GameClockManager.Instance.freeze = Events [index].pauseGame;

		GameObject audio = GameObject.FindGameObjectWithTag ("AudioSlider");
		if (audio != null) {
			volumeControl = audio.GetComponent<Slider> ();
			Debug.Log ("value: " + (volumeControl.value * Events [index].soundVolume));

		} else {
			Debug.Log ("audio slider not found");
		}

		if (Events [index].sound != null && audio != null)
			AudioManager.Instance.PlaySoundEffect(Events [index].sound, volumeControl.value*Events[index].soundVolume);
		else if(Events[index].sound != null)
			AudioManager.Instance.PlaySoundEffect(Events [index].sound, Events[index].soundVolume);


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
