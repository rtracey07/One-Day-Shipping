using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour {

	public ParticleSystem m_WindParticles;

	public float m_WindSpeed;
	private Rigidbody m_Player;

	// Use this for initialization
	void Start () {
		m_Player = FindObjectOfType<PlayerController> ().GetComponent<Rigidbody> ();
		m_WindParticles.Stop ();
	}

	// start the wind when enabled
	void OnEnable()
	{
		StartCoroutine (RandomWind ());
	}

	// stop when disabled
	void OnDisable()
	{
		this.StopAllCoroutines ();
	}

	/// <summary>
	/// Sets the wind at random
	/// </summary>
	/// <returns>The wind.</returns>
	public IEnumerator RandomWind()
	{
		while (true) {
			yield return new WaitForSeconds (Random.Range (1.0f, 3.0f));
			yield return StartCoroutine (TriggerWind ());
		}
	}

	/// <summary>
	/// Triggers the wind.
	/// </summary>
	/// <returns>The wind.</returns>
	public IEnumerator TriggerWind()
	{
		m_WindParticles.transform.RotateAround (m_Player.position, Vector3.up, Random.Range (0.0f, 359.0f));
		m_WindParticles.Play ();

		do {
			m_Player.AddForce(
				new Vector3(m_WindParticles.transform.forward.x, 0, m_WindParticles.transform.forward.z) * m_WindSpeed * GameClockManager.Instance.time
			);

			yield return null;
		} while (m_WindParticles.isPlaying);
	}
}
