using UnityEngine;
using System.Collections;

public class DropoffLocation : Location {

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

	public override void HandlePackage()
	{
		GameManager.Instance.hasPackage = false;
		AudioManager.Instance.PlaySoundEffect (m_AudioClip, m_Volume);
	}
}
