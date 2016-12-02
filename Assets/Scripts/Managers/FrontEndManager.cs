using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrontEndManager : MonoBehaviour {

	public GameObject m_Main;
	public GameObject m_LevelSelect;
	public GameObject m_Scores;
	public GameObject m_Controls;

	private GameObject m_ActiveCanvas;
	private Vector3 mainOutPos;

	void Awake()
	{
		m_ActiveCanvas = m_Main;
		mainOutPos = m_Main.transform.localPosition;

		StartCoroutine (MoveMain (true));
	}

	public void MainTransition()
	{
		m_ActiveCanvas.SetActive(false);
		m_Main.SetActive(true);
		m_ActiveCanvas = m_Main;
	}

	public void LevelSelectTransition()
	{
		m_ActiveCanvas.SetActive(false);
		m_LevelSelect.SetActive(true);
		m_ActiveCanvas = m_LevelSelect;
	}

	public void ScoresTransition()
	{
		m_ActiveCanvas.SetActive(false);
		m_Scores.SetActive(true);
		m_ActiveCanvas = m_Scores;
	}

	public void ControlsTransition()
	{
		m_ActiveCanvas.SetActive(false);
		m_Controls.SetActive(true);
		m_ActiveCanvas = m_Controls;
	}

	private IEnumerator MoveMain(bool moveIn)
	{
		float time = 0.0f;

		while (time <= 1.5f) {
			m_Main.transform.localPosition = Vector3.Lerp (Vector3.zero, mainOutPos, (moveIn ? 1 - time / 1.5f : time / 1.5f));
			time += Time.deltaTime;
			yield return null;
		}

		if (moveIn)
			m_Main.transform.localPosition = Vector3.zero;
		else
			m_Main.transform.localPosition = mainOutPos;
	}
}
