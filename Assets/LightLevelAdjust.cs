using UnityEngine;
using System.Collections;

public class LightLevelAdjust : MonoBehaviour {

	private Light m_MainLight;

	[Range(-1,1)]
	public float adjustment;

	void Start () {
		m_MainLight = GameObject.Find ("Sun").GetComponent<Light> ();
		m_MainLight.intensity += adjustment;
	}


}
