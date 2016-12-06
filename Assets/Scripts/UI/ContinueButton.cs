using UnityEngine;
using System.Collections;

/**
 * The MonoBehaviour script for the continue button
 */
public class ContinueButton : MonoBehaviour {

	public AudioClip buttonClick;

    /**
     * The function that is called when the user clicks the continue button
     */
	public void OnClick()
	{
		GameManager.Instance.Continue ();
		AudioManager.Instance.PlaySoundEffect (buttonClick, 0.1f);
	}
}
