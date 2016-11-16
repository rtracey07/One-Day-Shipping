using UnityEngine;
using System.Collections;

public class PickupLocation : MonoBehaviour {

	private bool selectedLocation;
	public Animator m_Animator;

	public ParticleSystem activeEffect;
	public ParticleSystem collectedEffect;


	// Use this for initialization
	void Start () {
		selectedLocation = false;
	}

	public void SetActive()
	{
		selectedLocation = true;
		activeEffect.Play ();
		m_Animator.SetBool ("CollectedBox", false);
		m_Animator.SetBool ("SelectedBox", true);
	}	

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GameManager.Instance.hasPackage = true;
			activeEffect.Stop ();
			m_Animator.SetBool ("CollectedBox", true);
			m_Animator.SetBool ("SelectedBox", false);
		}
	}
}
