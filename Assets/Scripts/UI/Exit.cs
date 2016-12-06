using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * The MonoBehaviour script for the exit button 
 */
public class Exit : MonoBehaviour {
    
    /**
     * The method called when the user clicks the exit button 
     */
    public void OnClick()
    {
		Application.Quit();
    }

}
