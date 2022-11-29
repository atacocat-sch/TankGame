using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public int gameLevelBuildIndex;

    [Space]
    public Transform menuContainer;

    [Space]
    public GameObject LoadingIcon;

    bool loadingGameScene = false;

    private void Start()
    {
        LoadingIcon.SetActive(false);
    }

    public void LoadGame()
    {
        if (loadingGameScene) return;
        loadingGameScene = true;

        SceneManager.LoadSceneAsync(gameLevelBuildIndex);
        LoadingIcon.SetActive(true);
    }

    public void Quit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenMenu (GameObject menu)
    {
        for (int i = 0; i < menuContainer.childCount; i++)
        {
            menuContainer.GetChild(i).gameObject.SetActive(menuContainer.GetChild(i).gameObject == menu);
        }
    }
}
