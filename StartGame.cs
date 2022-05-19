using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject loaderCanvas;
    public Slider progressBar;
    private int _level;

    private void Awake()
    {
        _level = SceneManager.GetActiveScene().buildIndex + 1;

    }

    public void LoadGame()
    {
        menuCanvas.SetActive(false);
        loaderCanvas.SetActive(true);
        StartCoroutine(StartLoad(_level));
    }

    IEnumerator StartLoad(int level)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(level);
        while (!asyncOperation.isDone)
        {
            progressBar.value = asyncOperation.progress;
            yield return null;
        }
    }
}