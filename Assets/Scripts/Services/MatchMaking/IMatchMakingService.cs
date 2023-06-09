using System;

public interface IMatchMakingService
{
#if DEPRECATED
    public void ApplyAsServer(string host, string port, Action<MatchInstance> onSuccessHandler,
        Action<string> onError);
#endif
    public void Register(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError);
    public void CancelRegistration(Action<MatchMakingResult> onSuccess, Action<string> onError);
}