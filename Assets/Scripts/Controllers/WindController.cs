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

	void OnEnable()
	{
		StartCoroutine (RandomWind ());
	}

	void OnDisable()
	{
		this.StopAllCoroutines ();
	}

	public IEnumerator RandomWind()
	{
		while (true) {
			yield return new WaitForSeconds (Random.Range (1.0f, 3.0f));
			yield return StartCoroutine (TriggerWind ());
		}
	}

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
