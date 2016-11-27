using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

	private float time;
	private float currTime;
	public bool active;

	public Image hourHand;
	private Color hourColor;

	public Image minuteHand;
	private Color minuteColor;

	public Color m_OutOfTimeColor;

	void Start () {
		time = LevelManager.Instance.GetMissionLength () * 60.0f;
		currTime = time;
		hourColor = hourHand.color;
		minuteColor = minuteHand.color;
	}

	void Update () {
		if (active) 
		{
			currTime -= GameClockManager.Instance.time;

			//Move hands.
			hourHand.rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp (-150, 90, currTime/time));
			minuteHand.rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp (-2880, 0, currTime/time));

			//Change to Red.
			if (currTime / time < 0.5f) {
				hourHand.color = Color.Lerp (m_OutOfTimeColor, hourColor, 2 * (currTime / time));
				minuteHand.color = Color.Lerp (m_OutOfTimeColor, minuteColor, 2 * (currTime / time));
			}
		}
	}
}
