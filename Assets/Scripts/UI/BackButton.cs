using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("FrontEnd");
        yield return null;
    }
}
