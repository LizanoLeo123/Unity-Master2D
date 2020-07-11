using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class InOutScene : MonoBehaviour
{
    public Image fade;
    public string[] myScenes;

    private void Start()
    {
        fade.CrossFadeAlpha(0,0.5f,false);
    }

    public void FadeOut(int index)
    {
        fade.CrossFadeAlpha(1, 0.5f, false);
        StartCoroutine(SceneChange(myScenes[index]));
    }

    IEnumerator SceneChange(string scene)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
}
