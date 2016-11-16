using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PickupLocation : MonoBehaviour {

	private bool selectedLocation;
	private Animator m_Animator;

	public ParticleSystem activeEffect;
	public ParticleSystem collectedEffect;


	// Use this for initialization
	void Start () {
		selectedLocation = false;
		m_Animator = GetComponent<Animator> ();
	}

	public void SetActive()
	{
		selectedLocation = true;
		activeEffect.Play ();
		m_Animator.SetBool ("SelectedBox", true);
	}		
}
