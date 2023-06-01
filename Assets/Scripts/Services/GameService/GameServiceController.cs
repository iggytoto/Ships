using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServiceController : MonoBehaviour
{
    private IAuthService _authService;


    private void Start()
    {
        ConfigureGameService();
    }

    private void ConfigureGameService()
    {
        _authService = gameObject.AddComponent<PlayFabAuthService>();
    }

    public IAuthService GetAuthService()
    {
        return _authService;
    }
}