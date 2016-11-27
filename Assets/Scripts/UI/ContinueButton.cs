using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour {

	public void OnClick()
	{
		GameManager.Instance.Continue ();
	}
}
