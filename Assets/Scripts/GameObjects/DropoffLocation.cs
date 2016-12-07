using UnityEngine;
using System.Collections;

public class DropoffLocation : Location {

	/// <summary>
	/// Function to activate or deactivate the dropoff location event
	/// </summary>
	/// <param name="on">If set to <c>true</c> on.</param>
	public override void TriggerEvent(bool on)
	{
		if (on) 
		{
			activeEffect.Play ();
		} 
		else 
		{
			activeEffect.Stop ();
		}
	}

	/// <summary>
	/// Function to drop off package.
	/// </summary>
	public override void HandlePackage()
	{
		GameManager.Instance.hasPackage = false;
		AudioManager.Instance.PlaySoundEffect (m_AudioClip, m_Volume);
	}
}
