using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Wednesday")]
public class Wednesday : Level {

	public GameObject stormPrefab;

	public override IEnumerator RunLevel()
	{
		
		TriggerStorm ();

		LevelManager.Instance.StartCoroutine (CheckWinState (1));
		//LevelManager.Instance.StartCoroutine (DetectWind (1));  //add this back in when DetectWind is set up

		//reset variables:
		LevelManager.Instance.UpdatePackageDeliveredCount ();
		GameManager.Instance.destroyed = false;
		LevelManager.Instance.SetPickup ();	

		//Trigger Onboarding event #1: Explain Weather.
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (0)); 

		//Package Loop. Will be interrupted by time up co-routine:
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


	public void TriggerStorm(){
		GameObject storm = (GameObject)GameObject.Instantiate (stormPrefab, GameManager.Instance.mainCamera.transform);
		storm.transform.localPosition = Vector3.zero;
	}

/*
	public IEnumerator DetectWind(int eventIndex)
	{
		yield return new WaitUntil (() => LevelManager.Instance.windDetected ());  //need to implement windDetected in levelmanager
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}

*/

}
