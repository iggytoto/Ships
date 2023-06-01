using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSceneUiController : MonoBehaviour
{
    [SerializeField] private TMP_InputField loginInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text messageText;

    private IAuthService _authService;

    private void Start()
    {
        Assert.IsNotNull(loginInput);
        Assert.IsNotNull(passwordInput);
        Assert.IsNotNull(registerButton);
        Assert.IsNotNull(loginButton);
        Assert.IsNotNull(messageText);
        Assert.IsNotNull(exitButton);

        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        _authService = FindObjectOfType<GameServiceController>().GetAuthService();
    }

    private static void OnExitButtonClicked()
    {
        Application.Quit(0);
    }

    private void OnRegisterButtonClicked()
    {
        _authService.RegisterNewUser(loginInput.text, passwordInput.text, OnMessage, OnMessage);
    }

    private void OnLoginButtonClicked()
    {
        _authService.Login(loginInput.text, passwordInput.text, OnMessage, OnMessage);
        SceneManager.LoadScene(SceneConstants.MainMenuSceneName);
    }

    private void OnMessage(string message)
    {
        messageText.text = message;
    }
}