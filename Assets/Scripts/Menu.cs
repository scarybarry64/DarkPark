using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void EjectGame()
    {
        //if (!Application.isEditor)
        //{
        //    Application.Quit();
        //}
        //else
        //{
        //    EditorApplication.isPlaying = false;
        //}

        Application.Quit();
    }
}
