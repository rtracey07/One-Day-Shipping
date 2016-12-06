using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * The MonoBehaviour script for the controls button
 */
public class Controls : MonoBehaviour {
    
    /**
     * The method called when the user selects the controls button
     */
    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }
    
    /**
     * Loads the controls scene 
     */
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Controls");
        yield return null;
    }
}
