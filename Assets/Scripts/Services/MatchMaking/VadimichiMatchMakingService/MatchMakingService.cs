using System;
using UnityEngine;

public class MatchMakingService : MonoBehaviour, IMatchMakingService
{
    private string _apiHostAddress;
    private string _apiHostPort;

    public void Register(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError)
    {
    }

    public void CancelRegistration(Action<MatchMakingResult> onSuccess, Action<string> onError)
    {
    }

    public void Initialize(string host, string port)
    {
        _apiHostAddress = host;
        _apiHostPort = port;
        Debug.Log($"Initialized Vadimichi auth service with host:{host} and port:{port}");
    }
}