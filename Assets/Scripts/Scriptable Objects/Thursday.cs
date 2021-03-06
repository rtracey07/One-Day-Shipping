﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Thursday")]
public class Thursday : Level {

	public override IEnumerator RunLevel()
	{
		LevelManager.Instance.StartCoroutine (CheckWinState (3));

		//reset variables:
		currIndex = 0;
		LevelManager.Instance.UpdatePackageDeliveredCount ();
		GameManager.Instance.destroyed = false;
		LevelManager.Instance.SetPickup ();	

		//Trigger Onboarding event #1: Explain Intro to level:
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (0)); 

		//Package Loop. Will be interrupted by time up co-routine.
		while (GameManager.Instance.stats.packagesDelivered < 2) {
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

		//now deliver the special package on top of the mountain:
		bool delivered = false;
		bool firstTry = true;
		while (!delivered) {
			
			//Wait until package has been picked up:
			yield return new WaitUntil (() => GameManager.Instance.hasPackage);

			//Set dropoff (will always be at Postal Service Office):
			LevelManager.Instance.SetMountainDropoff();

			//Trigger Onboarding event #2: Explain strange dropoff at PSO at first try:
			if (firstTry){
				yield return new WaitForSeconds (2.0f);
				yield return LevelManager.Instance.StartCoroutine (TriggerEvent (1));
				yield return new WaitForSeconds (1.0f);
				yield return LevelManager.Instance.StartCoroutine (TriggerEvent (2));
				firstTry = false;
			}

			//Wait until player no longer is in possession of a package:
			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);
			if (!GameManager.Instance.destroyed) {
				GameManager.Instance.stats.packagesDelivered++;
				delivered = true; //condition to exit loop
				//Reset index for bonus rounds:
				currIndex = 0;
			}

			//reset / update parameters:
			GameManager.Instance.destroyed = false;
			LevelManager.Instance.UpdatePackageDeliveredCount ();

			LevelManager.Instance.SetPickup ();
		}

		//bonus packages loop:
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



}
