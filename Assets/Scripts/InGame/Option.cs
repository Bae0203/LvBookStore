using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetOption(bool Active)
    {
        gameObject.SetActive(Active);
    }

    public void SaveGame()
    {
        GameManager.instance.Save();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); // 어플리케이션 종료
        #endif
    }
}
