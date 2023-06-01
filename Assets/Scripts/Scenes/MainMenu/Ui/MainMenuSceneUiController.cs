using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneUiController : MonoBehaviour
{
    [SerializeField] private Button searchDuelButton;
    [SerializeField] private Button cancelSearchDuelButton;
    [SerializeField] private Button logoutButton;

    private IAuthService _authService;

    private void Start()
    {
        Assert.IsNotNull(searchDuelButton);
        Assert.IsNotNull(cancelSearchDuelButton);
        Assert.IsNotNull(logoutButton);

        cancelSearchDuelButton.gameObject.SetActive(false);

        searchDuelButton.onClick.AddListener(OnSearchDuelClicked);
        cancelSearchDuelButton.onClick.AddListener(OnCancelSearchDuelClicked);
        logoutButton.onClick.AddListener(OnLogoutClicked);

        _authService = FindObjectOfType<GameServiceController>().GetAuthService();
    }

    private void OnLogoutClicked()
    {
        _authService.Logout();
        SceneManager.LoadScene(SceneConstants.LoginSceneName);
    }

    private void OnCancelSearchDuelClicked()
    {
        throw new System.NotImplementedException();
    }

    private void OnSearchDuelClicked()
    {
        throw new System.NotImplementedException();
    }
}