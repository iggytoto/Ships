using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerIdleSceneController : MonoBehaviour
{
    private IAuthService _authService;
    private IServerMatchMakingService _serverMatchMakingService;
    [SerializeField] public string username = "server";
    [SerializeField] public string password = "password";
    [SerializeField] public string host = "127.0.0.1";
    [SerializeField] public string port = "7777";
    [SerializeField] public float updateInterval = 5;
    private float _updateTime;
    private MatchInstance _matchInstance;

#if DEDICATED

    private void Start()
    {
        var gs = FindObjectOfType<GameServiceController>();
        _authService = gs.GetAuthService();
        _serverMatchMakingService = gs.GetServerMatchMakingService();

        if (!NetworkManager.Singleton.IsServer)
        {
            StartNetCodeServer();
        }
    }

    private static void StartNetCodeServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void Update()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        _updateTime -= Time.deltaTime;
        if (!(_updateTime <= 0)) return;
        if (_authService.ConnectionState == ConnectionState.Disconnected || _authService.GetAuthData() == null)
        {
            ProcessLogin();
        }
        else if (_authService.ConnectionState == ConnectionState.Connecting)
        {
        }
        else
        {
            ProcessEvent();
        }

        _updateTime = updateInterval;
    }

    private void ProcessEvent()
    {
        if (_matchInstance == null)
        {
            _serverMatchMakingService.ApplyAsServer(
                host,
                port,
                mi => _matchInstance = mi,
                Debug.LogError);
        }
        else
        {
            switch (_matchInstance.type)
            {
                case MatchType.Duel:
                    SceneManager.LoadScene(SceneConstants.DuelBattleSceneName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void ProcessLogin()
    {
        Debug.Log($"Trying to login with credentials: {username}:{password}");
        _authService.Login(username, password, _ => ProcessEvent(), Debug.LogError);
    }

#endif
}