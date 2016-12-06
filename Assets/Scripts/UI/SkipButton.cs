using UnityEngine;
using System.Collections;

/**
 * The MonoBehaviour script for the skip button 
 */
public class SkipButton : MonoBehaviour {

    //  Audio for button click
	public AudioClip buttonClick;

    /**
     * The method that's called when the button is clicked
     */
    public void OnClick()
	{
		GameManager.Instance.Skip ();
		AudioManager.Instance.PlaySoundEffect (buttonClick, 0.1f);
	}
}
