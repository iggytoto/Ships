using System;

public interface IMatchMakingService
{
    public void Register(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError);
    public void CancelRegistration(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError);
}