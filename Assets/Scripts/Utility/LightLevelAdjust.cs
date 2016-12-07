using UnityEngine;
using System.Collections;

/**
 * The MonoBehaviour script for adjusting the level lighting
 */
public class LightLevelAdjust : MonoBehaviour {

	private Light m_MainLight;

	[Range(-1,1)]
	public float adjustment;

    /**
     * Sets the light intensity
     */
	void Start () {
		m_MainLight = GameObject.Find ("Sun").GetComponent<Light> ();
		m_MainLight.intensity += adjustment;
	}

}
