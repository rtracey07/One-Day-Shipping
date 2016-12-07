using UnityEngine;
using System.Collections;

public class Location : MonoBehaviour {

	//public references:
    public GameObject minimapMarker;
	public Animator m_Animator;
	public AudioClip m_AudioClip;
	public float m_Volume;
	public ParticleSystem activeEffect;
	public ParticleSystem collectedEffect;

	/// <summary>
	/// Awake this instance and set minimapMarker to inactive
	/// </summary>
    void Awake()
    {
        minimapMarker.SetActive(false);
    }

	/// <summary>
	/// Triggers the event (Set Location to active).
	/// </summary>
	public void SetActive()
	{
		TriggerEvent (true);
    }	

	/// <summary>
	/// Trigger detection when player meets the Location.
	/// Turn off this Location.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && LevelManager.Instance.currentDestination == this) {
			HandlePackage ();
			TriggerEvent (false);
		}
	}

	/// <summary>
	/// Function to trigger the Location based on bool input.
	/// </summary>
	/// <param name="on">If set to <c>true</c> on.</param>
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

	/// <summary>
	/// Sets the mini map marker active depending on bool input
	/// </summary>
	/// <param name="active">If set to <c>true</c> active.</param>
    public virtual void SetMiniMapMarkerActive(bool active)
    {
        minimapMarker.SetActive(active);
    }

	/// <summary>
	/// Handles the package.
	/// Tells GameManager that you have a package.
	/// </summary>
	public virtual void HandlePackage()
	{
		GameManager.Instance.hasPackage = true;
		AudioManager.Instance.PlaySoundEffect (m_AudioClip, m_Volume);
	}
}
