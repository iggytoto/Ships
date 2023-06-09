using UnityEngine;

public class GameServiceController : MonoBehaviour
{
    private IAuthService _authService;
    private IMatchMakingService _matchMakingService;

    private void Awake()
    {
        ConfigureGameServices();
    }

    private void ConfigureGameServices()
    {
        _authService = InitAuthService();
        _matchMakingService = InitMatchMakingService();
    }

    private IMatchMakingService InitMatchMakingService()
    {
        var service = gameObject.AddComponent<MatchMakingService>();
        service.Initialize("127.0.0.1", "8080");
        return service;
    }

    private IAuthService InitAuthService()
    {
        var service = gameObject.AddComponent<AuthService>();
        service.Initialize("127.0.0.1", "8080");
        return service;
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