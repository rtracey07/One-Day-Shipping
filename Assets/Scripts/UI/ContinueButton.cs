using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour {

	public AudioClip buttonClick;

	public void OnClick()
	{
		GameManager.Instance.Continue ();
		AudioManager.Instance.PlaySoundEffect (buttonClick, 0.1f);
	}
}
