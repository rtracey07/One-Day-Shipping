using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Monday")]
public class Monday : Level {

	public override IEnumerator RunLevel()
	{
		bool playedFirstDelivery = false;
		//Setup event for first dog attack.
		LevelManager.Instance.StartCoroutine (OnDogAttack(2));
		LevelManager.Instance.StartCoroutine (CheckWinState (5));
		LevelManager.Instance.StartCoroutine (CheckPackageDestroyed (4));

		//Get first package pickup location & update UI counter.
		LevelManager.Instance.UpdatePackageDeliveredCount ();
		LevelManager.Instance.SetPickup ();

		//Trigger Onboarding event #1: Explain UI.
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (0));

		//Get First package.
		yield return new WaitUntil (() => GameManager.Instance.hasPackage);
		LevelManager.Instance.SetDropoff ();

		//Trigger Onboarding event #2: Explain drop off.
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (1));

		//Deliver package and get updated counter.
		yield return new WaitUntil (() => !GameManager.Instance.hasPackage);
		if (!GameManager.Instance.destroyed) {
			GameManager.Instance.stats.packagesDelivered++;
			LevelManager.Instance.UpdatePackageDeliveredCount ();
			//Trigger Onboarding event #3: Rinse & Repeat delivery.
			yield return LevelManager.Instance.StartCoroutine (TriggerEvent (3));
			playedFirstDelivery = true;
		}

		GameManager.Instance.destroyed = false;

		LevelManager.Instance.SetPickup ();


		//Package Loop. Will be interrupted by time up co-routine.
		while (true) {
			//Package Loop: Drop off.
			yield return new WaitUntil (() => GameManager.Instance.hasPackage);
			LevelManager.Instance.SetDropoff ();

			//Package Loop Pick up.
			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);
			if (!GameManager.Instance.destroyed) {
				GameManager.Instance.stats.packagesDelivered++;
				if (!playedFirstDelivery) {
					//Trigger Onboarding event #3: Rinse & Repeat delivery.
					yield return LevelManager.Instance.StartCoroutine (TriggerEvent (3));
					playedFirstDelivery = true;
				}
			}

			GameManager.Instance.destroyed = false;

			LevelManager.Instance.UpdatePackageDeliveredCount ();
			LevelManager.Instance.SetPickup ();
		}
	}

	/// <summary>
	/// Raises the dog attack event.
	/// </summary>
	/// <param name="eventIndex">Event index.</param>
	public IEnumerator OnDogAttack(int eventIndex)
	{
		yield return new WaitUntil (() => GameManager.Instance.dogAttack);
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}
	/// <summary>
	/// Checks if the level is won
	/// </summary>
	/// <returns>The win state.</returns>
	/// <param name="eventIndex">Event index.</param>
	public IEnumerator CheckWinState(int eventIndex)
	{
		yield return new WaitUntil (() => LevelManager.Instance.CheckWinState ());
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}

	/// <summary>
	/// Checks if the package destroyed.
	/// </summary>
	/// <returns>The package destroyed.</returns>
	/// <param name="eventIndex">Event index.</param>
	public IEnumerator CheckPackageDestroyed(int eventIndex)
	{
		yield return new WaitUntil (() => GameManager.Instance.destroyed);
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}
}
