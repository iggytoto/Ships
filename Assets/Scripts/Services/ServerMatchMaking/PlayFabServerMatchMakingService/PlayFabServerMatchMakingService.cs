﻿using System;
using PlayFab;
using UnityEngine;

public class PlayFabServerMatchMakingService : MonoBehaviour, IServerMatchMakingService
{
    public void ApplyAsServer(string host, string port, Action<MatchInstance> onSuccessHandler, Action<string> onError)
    {
    }
}