using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenLeaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject menuCanvas;
    public Slider progressBar;
    private int _level;


    private void Awake()
    {
        _level = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void OpenLeaderboardCanvas()
    {
        leaderboardCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(StartLoad(_level));
    }

    public void BackMenu()
    {
        menuCanvas.SetActive(true);
        leaderboardCanvas.SetActive(false);
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