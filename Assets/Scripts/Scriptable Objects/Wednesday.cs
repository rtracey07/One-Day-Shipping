using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Wednesday")]
public class Wednesday : Level {

	public override IEnumerator RunLevel()
	{
		//LevelManager.Instance.StartCoroutine (TriggerStorm ());
		LevelManager.Instance.StartCoroutine (CheckWinState (1));

		//Package Loop. Will be interrupted by time up co-routine.
		LevelManager.Instance.UpdatePackageDeliveredCount ();
		GameManager.Instance.destroyed = false;
		LevelManager.Instance.SetPickup ();	

		//Trigger Onboarding event #1: Explain Entering Suburbs.
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (0)); 

		while (true) {
			//Package Loop: Drop off.
			yield return new WaitUntil (() => GameManager.Instance.hasPackage);
			LevelManager.Instance.SetDropoff ();

			//Package Loop Pick up.
			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);
			if (!GameManager.Instance.destroyed) {
				GameManager.Instance.stats.packagesDelivered++;
			}

			GameManager.Instance.destroyed = false;

			LevelManager.Instance.UpdatePackageDeliveredCount ();
			LevelManager.Instance.SetPickup ();
		}
	}

	public IEnumerator CheckWinState(int eventIndex)
	{
		yield return new WaitUntil (() => LevelManager.Instance.CheckWinState ());
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}

	//storm logic goes here:

	/*
	public IEnumerator TriggerStorm(){
		yield return;
	}
	*/

}
