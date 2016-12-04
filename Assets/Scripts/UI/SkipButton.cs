using UnityEngine;
using System.Collections;

public class SkipButton : MonoBehaviour {

	public AudioClip buttonClick;

	public void OnClick()
	{
		GameManager.Instance.Skip ();
		AudioManager.Instance.PlaySoundEffect (buttonClick, 0.1f);
	}
}
