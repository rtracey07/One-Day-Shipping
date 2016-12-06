using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


/**
 * The MonoBehaviour script for the Back button
 */
public class BackButton : MonoBehaviour {

    /**
     * The function that is called when the user clicks the back button
     */
    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }

    /**
     * Loads the main Front end menu
     */
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("FrontEnd");
        yield return null;
    }
}
