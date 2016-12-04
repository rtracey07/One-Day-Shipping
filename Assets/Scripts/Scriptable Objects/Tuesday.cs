using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Tuesday")]
public class Tuesday : Level {

	// Changes since Monday:
	// Deliver packages to the suburbs
	// postmen now shoots packages

	// textboxes:
	// "you just did a great job in the city. now let's go to suburbs"
	// "Did that guy just throw a package at you? wow you really pissed them off. Stay away"
	// "This is the suburbs. Don't get lost"
	// "Really, Noodleman? Do you have to walk on my lawn?!" (strategically place location)
	// ""

	public override IEnumerator RunLevel()
	{
		LevelManager.Instance.StartCoroutine (CheckWinState (3));
		LevelManager.Instance.StartCoroutine (CheckPostmanHit (1));
		LevelManager.Instance.StartCoroutine (CheckEnteringSuburbs (2));

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

	public IEnumerator CheckPostmanHit(int eventIndex)
	{
		yield return new WaitUntil (() => GameManager.Instance.postmanAttack);
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}

	public IEnumerator CheckEnteringSuburbs(int eventIndex)
	{
		PlayerController player = FindObjectOfType<PlayerController> ();
		if (player == null) {
			yield break;
		}
		//wait until we are in the suburb location disclosed by (87, y, 18)
		yield return new WaitUntil (() => player.transform.position.x > 87.0f && player.transform.position.z > 18.0f);

		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}

}

