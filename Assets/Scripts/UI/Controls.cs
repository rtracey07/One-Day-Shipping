using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour {

    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Controls");
        yield return null;
    }
}
