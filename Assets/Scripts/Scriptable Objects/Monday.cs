using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Data/Level/Monday")]
public class Monday : Level {

	public override IEnumerator RunLevel()
	{
		LevelManager.Instance.StartCoroutine (OnDogAttack(2));

		LevelManager.Instance.SetPickup ();

		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (0));

		yield return new WaitUntil (() => GameManager.Instance.hasPackage);
		LevelManager.Instance.SetDropoff ();

		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (1));

		for (int i = 0; i < 5; i++) {
			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);

			LevelManager.Instance.SetPickup ();

			yield return new WaitUntil (() => GameManager.Instance.hasPackage);

			LevelManager.Instance.SetDropoff ();
		}
	}

	public IEnumerator OnDogAttack(int eventIndex)
	{
		yield return new WaitUntil (() => GameManager.Instance.dogAttack);
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}
}
