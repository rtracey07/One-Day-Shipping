using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Timer class that controls the UI clock.
/// </summary>
public class Timer : MonoBehaviour {


	private float time;
	private float currTime;
	public bool active;

	public Image hourHand;
	private Color hourColor;

	public Image minuteHand;
	private Color minuteColor;

	public Color m_OutOfTimeColor;

	/// <summary>
	/// Start this instance.
	/// Set the time and hand colors.
	/// </summary>
	void Start () {
		time = LevelManager.Instance.GetMissionLength () * 60.0f;
		currTime = time;
		hourColor = hourHand.color;
		minuteColor = minuteHand.color;
	}

	/// <summary>
	/// Update this instance.
	/// Updates the hands to the current time.
	/// Gradually switches color of hands to red as it gets closer to end time.
	/// </summary>
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

			//Time Up.
			if (currTime <= 0.0f)
				GameManager.Instance.timeUp = true;	
		}
	}
}
