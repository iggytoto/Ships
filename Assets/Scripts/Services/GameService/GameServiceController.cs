using UnityEngine;

public class GameServiceController : MonoBehaviour
{
    private IAuthService _authService;
    private IMatchMakingService _matchMakingService;


    private void Start()
    {
        ConfigureGameServices();
    }

    private void ConfigureGameServices()
    {
        _authService = gameObject.AddComponent<PlayFabAuthService>();
        _matchMakingService = gameObject.AddComponent<PlayFabMatchMakingService>();
    }

    public IAuthService GetAuthService()
    {
        return _authService;
    }

    public IMatchMakingService GetMatchMakingService()
    {
        return _matchMakingService;
    }
}