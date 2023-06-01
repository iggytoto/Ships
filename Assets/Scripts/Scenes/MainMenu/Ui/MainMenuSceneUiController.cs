using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MainMenuSceneUiController : MonoBehaviour
{
    [SerializeField] private Button searchDuelButton;
    [SerializeField] private Button cancelSearchDuelButton;
    [SerializeField] private Button logoutButton;

    private void Start()
    {
        Assert.IsNotNull(searchDuelButton);
        Assert.IsNotNull(cancelSearchDuelButton);
        Assert.IsNotNull(logoutButton);

        cancelSearchDuelButton.gameObject.SetActive(false);
        
        searchDuelButton.onClick.AddListener(OnSearchDuelClicked);
        cancelSearchDuelButton.onClick.AddListener(OnCancelSearchDuelClicked);
        logoutButton.onClick.AddListener(OnLogoutClicked);
    }

    private void OnLogoutClicked()
    {
        throw new System.NotImplementedException();
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