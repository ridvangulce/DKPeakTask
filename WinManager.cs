using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public AudioSource audioSource;
    private int _level;

    private void Start()
    {
        audioSource.Play();
        _level = SceneManager.GetActiveScene().buildIndex;
    }

    public void Restart()
    {
        SceneManager.LoadScene(_level - 1);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(_level - 2);
    }
}