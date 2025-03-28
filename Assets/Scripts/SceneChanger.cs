using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public List<GameObject> DISABLE_elements = new();
    public GameObject loading_panel;
    public Slider Slider;

    public void LoadScene(int Scene)
    {
        foreach (GameObject item in DISABLE_elements)
        {
            item.SetActive(false);
        }
        StartCoroutine(AsyncSceneLoad(Scene));
    }

    IEnumerator AsyncSceneLoad(int Scene)
    {
        AsyncOperation loading_operation = SceneManager.LoadSceneAsync(Scene);
        loading_panel.SetActive(true);

        // Ensure scene doesn't activate immediately
        loading_operation.allowSceneActivation = false;
        
        while (loading_operation.progress < 0.9f) // 0.9 means scene is almost ready
        {
            yield return new WaitForSeconds(0.5f);
            float progress = Mathf.Clamp01(loading_operation.progress / 0.9f);
            Slider.value = progress;
            yield return null;
        }

        // ✅ Artificial delay to slow down the loading screen
        yield return new WaitForSeconds(1f); // Change 2f to any number for longer delay

        // Finally, activate the scene
        loading_operation.allowSceneActivation = true;
    }
}
