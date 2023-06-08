using System;
using UnityEngine;

public class AuthService : MonoBehaviour, IAuthService
{
    private string _apiHostAddress;
    private string _apiHostPort;

    public ConnectionState ConnectionState => ConnectionState.Disconnected;

    public AuthData GetAuthData()
    {
        return null;
    }

    public void RegisterNewUser(string login, string password, Action<string> onSuccessHandler,
        Action<string> onErrorHandler)
    {
    }

    public void Login(string login, string password, Action<string> onSuccessHandler, Action<string> onErrorHandler)
    {
    }

    public void Logout()
    {
    }

    public void Initialize(string host, string port)
    {
        _apiHostAddress = host;
        _apiHostPort = port;
        Debug.Log($"Initialized Vadimichi auth service with host:{host} and port:{port}");
    }
}