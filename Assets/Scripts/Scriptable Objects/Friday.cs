using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Friday")]
public class Friday : Level {

	public override IEnumerator RunLevel()
	{
		LevelManager.Instance.StartCoroutine (CheckWinState (1));
		//LevelManager.Instance.StartCoroutine (CheckFlamingPackages (1));

		LevelManager.Instance.UpdatePackageDeliveredCount ();	
		GameManager.Instance.destroyed = false;
		LevelManager.Instance.SetPickup ();	

		//Trigger Onboarding event #1: Explain Flaming Packages from the sky.
		yield return new WaitForSeconds(1.5f);
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

	//flaming package detection not implemented yet.
	/*
	public IEnumerator CheckFlamingPackages(int eventIndex)
	{
		
	}
	*/

}
