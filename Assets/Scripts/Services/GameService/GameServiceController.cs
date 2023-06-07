using UnityEngine;

public class GameServiceController : MonoBehaviour
{
    private IAuthService _authService;
    private IMatchMakingService _matchMakingService;
    private IServerMatchMakingService _serverMatchMakingService;

    private void Start()
    {
        ConfigureGameServices();
    }

    private void ConfigureGameServices()
    {
        _authService = gameObject.AddComponent<PlayFabAuthService>();
        _matchMakingService = gameObject.AddComponent<PlayFabMatchMakingService>();
#if DEDICATED
        _serverMatchMakingService = gameObject.AddComponent<PlayFabServerMatchMakingService>();
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

    public IServerMatchMakingService GetServerMatchMakingService()
    {
        return _serverMatchMakingService;
    }
}