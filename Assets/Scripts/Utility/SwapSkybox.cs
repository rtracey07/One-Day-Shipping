using UnityEngine;
using System.Collections;

public class SwapSkybox : MonoBehaviour {

	public Material m_Skybox;
	private Material m_OriginalSkybox;

	//Cache Current Skybox.
	void Awake()
	{
		m_OriginalSkybox = RenderSettings.skybox;
	}

	//Swap New Skybox in.
	void OnEnable() {
		RenderSettings.skybox = m_Skybox;
	}

	//Put Old Skybox back.
	void OnDisable()
	{
		RenderSettings.skybox = m_OriginalSkybox;
	}

}
