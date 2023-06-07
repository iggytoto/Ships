using UnityEngine;

public class GameServiceController : MonoBehaviour
{
    private IAuthService _authService;
    private IMatchMakingService _matchMakingService;
    private IServerMatchMakingService _serverMatchMakingService;

    private void Awake()
    {
        ConfigureGameServices();
    }

    private void ConfigureGameServices()
    {
        _authService = gameObject.AddComponent<PlayFabAuthService>();
        Debug.Log("Auth service initialized:" + _authService);
        _matchMakingService = gameObject.AddComponent<PlayFabMatchMakingService>();
        Debug.Log("Match making service initialized:" + _matchMakingService);
#if DEDICATED
        _serverMatchMakingService = gameObject.AddComponent<PlayFabServerMatchMakingService>();
        Debug.Log("Serve match making service initialized:" + _serverMatchMakingService);
#endif
    }

    public IAuthService GetAuthService()
    {
        return _authService;
    }

    public IMatchMakingService GetMatchMakingService()
    {
        return _matchMakingService;
    }

#if DEDICATED
    public IServerMatchMakingService GetServerMatchMakingService()
    {
        return _serverMatchMakingService;
    }
#endif
}