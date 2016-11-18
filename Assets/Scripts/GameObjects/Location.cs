using UnityEngine;
using System.Collections;

public class Location : MonoBehaviour {

	public Animator m_Animator;

	public ParticleSystem activeEffect;
	public ParticleSystem collectedEffect;

	public void SetActive()
	{
		TriggerEvent (true);
	}	

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GameManager.Instance.hasPackage = true;
			TriggerEvent (false);
		}
	}

	public virtual void TriggerEvent(bool on)
	{
		if (on) 
		{
			activeEffect.Play ();
			m_Animator.SetBool ("CollectedBox", false);
			m_Animator.SetBool ("SelectedBox", true);
		} else 
		{
			activeEffect.Stop ();
			m_Animator.SetBool ("CollectedBox", true);
			m_Animator.SetBool ("SelectedBox", false);
		}
	}
}
