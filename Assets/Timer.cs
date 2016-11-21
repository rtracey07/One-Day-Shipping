using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour {

	private float time;
	private Text timerField;
	public bool active;

	// Use this for initialization
	void Start () {
		time = LevelManager.Instance.GetMissionLength () * 60.0f;
		timerField = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) 
		{
			time -= GameClockManager.Instance.time;
			timerField.text = string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
		}
	}
}
