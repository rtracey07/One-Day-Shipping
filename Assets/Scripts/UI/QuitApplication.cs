using UnityEngine;
using System.Collections;

/**
 * The MonoBehaviour script for quitting the application
 */
public class QuitApplication : MonoBehaviour {

    /**
     * Quits the application
     */
	public void OnClick(){
		Application.Quit ();
	}
}
