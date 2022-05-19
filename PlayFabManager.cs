using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayFabManager : MonoBehaviour
{
    [Header("UI")] public TextMeshProUGUI messageText;
    public TextMeshProUGUI registerMessageText;
    public TextMeshProUGUI resetPasswordMessageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public TextMeshProUGUI userName;
    public TextMeshProUGUI userID;
    private int _level;
    public Slider progressBar;
    public GameObject menuCanvas;
    public GameObject loadingCanvas;
    public GameObject resetPasswordCanvas;
    public GameObject registerCanvas;
    public GameObject playCanvas;
    public AudioSource audioSource;
    string name = null;
    string id = null;
    [Header("Windows")] public GameObject nameCanvas;

    [Header("Display Name Window")] public GameObject nameError;
    public TMP_InputField nameInput;

    private void Awake()
    {
        PlayerPrefs.GetString("id");
        PlayerPrefs.GetString("name");
        if (PlayerPrefs.HasKey("id") && PlayerPrefs.HasKey("name"))
        {
            playCanvas.SetActive(true);
            menuCanvas.SetActive(false);
            loadingCanvas.SetActive(false);
        }

        _level = SceneManager.GetActiveScene().buildIndex + 1;
        messageText.text = "";
        GetPlayerProfile();

    }

    private void Start()
    {
        audioSource.Play();
    }

    public void RegisterButton()
    {
        if (registerPasswordInput.text.Length < 6)
        {
            registerMessageText.text = "Password Too Short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = registerEmailInput.text,
            Password = registerPasswordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        registerMessageText.text = "Registered Successful!";
    }


    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged In!";
        menuCanvas.SetActive(false);


        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            id = result.InfoResultPayload.PlayerProfile.PlayerId;
            PlayerPrefs.SetString("name", name);
            PlayerPrefs.SetString("id", id);
            userName.text = result.InfoResultPayload.PlayerProfile.DisplayName;
            userID.text = "ID: " + result.InfoResultPayload.PlayerProfile.PlayerId;
        }

        if (name == null)
        {
            nameCanvas.SetActive(true);
        }
        else
        {
            playCanvas.SetActive(true);
        }
    }


    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Tutorial",
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        playCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        nameCanvas.SetActive(false);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("UpdatedDisplayName");
    }

    public void GetPlayerProfile()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
            {
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true,
                }
            },
            result => userName.text = result.PlayerProfile.DisplayName,
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        messageText.color = Color.red;
        print(error.GenerateErrorReport());
    }

    private void OnRegisterError(PlayFabError error)
    {
        registerMessageText.text = error.ErrorMessage;
        registerMessageText.color = Color.red;
        print(error.GenerateErrorReport());
    }
    private void OnResetPasswordError(PlayFabError error)
    {
        resetPasswordMessageText.text = error.ErrorMessage;
        resetPasswordMessageText.color = Color.red;
        print(error.GenerateErrorReport());
    }

    public void PlayGame()
    {
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

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "70278"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnResetPasswordError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        resetPasswordMessageText.text = "Password Reset Mail Sent!";
    }


    public void ChangeToRegisterCanvas()
    {
        registerCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        loadingCanvas.SetActive(false);
        resetPasswordCanvas.SetActive(false);
    }

    public void ChangeToResetPasswordCanvas()
    {
        resetPasswordCanvas.SetActive(true);
        registerCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        loadingCanvas.SetActive(false);
    }

    public void BackToMenuCanvas()
    {
        menuCanvas.SetActive(true);
        resetPasswordCanvas.SetActive(false);
        registerCanvas.SetActive(false);
        loadingCanvas.SetActive(false);
        playCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        playCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        emailInput.text = "";
        passwordInput.text = "";
        PlayerPrefs.DeleteAll();
    }
}