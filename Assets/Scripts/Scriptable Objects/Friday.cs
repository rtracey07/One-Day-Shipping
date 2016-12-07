using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Level/Friday")]
public class Friday : Level {

	public GameObject m_Apocalypse;						// apocolypse effects

	public override IEnumerator RunLevel()
	{
		TriggerApocalypse ();
		LevelManager.Instance.StartCoroutine (CheckWinState (1));
		LevelManager.Instance.StartCoroutine (SpawnFlamingPackages ());

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
	/// <summary>
	/// Checks if the level has been won
	/// </summary>
	/// <returns>The window state.</returns>
	/// <param name="eventIndex">Event index.</param>
	public IEnumerator CheckWinState(int eventIndex)
	{
		yield return new WaitUntil (() => LevelManager.Instance.CheckWinState ());
		LevelManager.Instance.StopCoroutine ("SpawnFlamingPackages");
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}

	/// <summary>
	/// makes flaming packages fall from sky
	/// </summary>
	/// <returns>The flaming packages.</returns>
	public IEnumerator SpawnFlamingPackages(){
		GameObject player = GameObject.Find ("Player");
		Rigidbody playerBody = player.GetComponent<Rigidbody> ();
		while (true) {
			yield return new WaitForSeconds(1.0f);
			if (!GameClockManager.Instance.freeze) {
				LevelManager.Instance.SpawnFlamingPackages (player, playerBody);
			}
		}
	}

	/// <summary>
	/// starts the apocolypse
	/// </summary>
	public void TriggerApocalypse(){
		GameObject apocalypse = (GameObject)GameObject.Instantiate (m_Apocalypse, GameManager.Instance.mainCamera.transform);
		apocalypse.transform.localPosition = Vector3.zero;
	}
}
