using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneUiController : MonoBehaviour
{
    [SerializeField] private Button searchDuelButton;
    [SerializeField] private Button cancelSearchDuelButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private TMP_Text messageText;

    private IAuthService _authService;
    private IMatchMakingService _matchMakingService;

    private void Start()
    {
        Assert.IsNotNull(searchDuelButton);
        Assert.IsNotNull(cancelSearchDuelButton);
        Assert.IsNotNull(logoutButton);
        Assert.IsNotNull(messageText);

        cancelSearchDuelButton.gameObject.SetActive(false);

        searchDuelButton.onClick.AddListener(OnSearchDuelClicked);
        cancelSearchDuelButton.onClick.AddListener(OnCancelSearchDuelClicked);
        logoutButton.onClick.AddListener(OnLogoutClicked);

        var gameService = FindObjectOfType<GameServiceController>();
        _authService = gameService.GetAuthService();
        _matchMakingService = gameService.GetMatchMakingService();
    }

    private void OnLogoutClicked()
    {
        _authService.Logout();
        SceneManager.LoadScene(SceneConstants.LoginSceneName);
    }

    private void OnCancelSearchDuelClicked()
    {
        _matchMakingService.CancelRegistration(MatchMakingType.Duel, OnCancelMatchMakingResultSuccess, OnError);
        searchDuelButton.gameObject.SetActive(true);
        cancelSearchDuelButton.gameObject.SetActive(false);
    }

    private void OnSearchDuelClicked()
    {
        _matchMakingService.Register(MatchMakingType.Duel, OnMatchMakingResultSuccess, OnError);
        searchDuelButton.gameObject.SetActive(false);
        cancelSearchDuelButton.gameObject.SetActive(true);
    }

    private void OnCancelMatchMakingResultSuccess(MatchMakingResult obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnError(string obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnMatchMakingResultSuccess(MatchMakingResult obj)
    {
        throw new System.NotImplementedException();
    }
}