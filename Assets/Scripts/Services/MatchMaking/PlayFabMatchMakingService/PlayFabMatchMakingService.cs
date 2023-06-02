using System;
using UnityEngine;

public class PlayFabMatchMakingService : MonoBehaviour, IMatchMakingService
{
    public void Register(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError)
    {
        throw new NotImplementedException();
    }

    public void CancelRegistration(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError)
    {
        throw new NotImplementedException();
    }
}
