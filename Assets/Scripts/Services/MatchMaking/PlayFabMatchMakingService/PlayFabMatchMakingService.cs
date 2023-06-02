using System;
using System.Collections;
using PlayFab;
using PlayFab.MultiplayerModels;
using UnityEngine;

public class PlayFabMatchMakingService : MonoBehaviour, IMatchMakingService
{
    private const int Timeout = 600;
    private IAuthService _authService;
    private Coroutine _matchMakingTicketProcessing;
    private Action<MatchMakingResult> _currentMatchMakingResultHandler;
    private Action<string> _currentMatchMakingErrorHandler;
    private MatchMakingType _currentMatchMakingTypeRequested;
    private string _currentMatchMakingRequestId;

    private void Start()
    {
        _authService = FindObjectOfType<GameServiceController>().GetAuthService();
    }
    
    public void Register(MatchMakingType type, Action<MatchMakingResult> onSuccess, Action<string> onError)
    {
        if (_matchMakingTicketProcessing != null)
        {
            onError?.Invoke("Failed to start matchmaking: ticket already in process");
            return;
        }

        var authData = _authService.GetAuthData();
        if (authData == null)
        {
            onError?.Invoke("Failed to start matchmaking: not authorized");
            return;
        }

        _currentMatchMakingErrorHandler = onError;
        _currentMatchMakingResultHandler = onSuccess;
        _currentMatchMakingTypeRequested = type;

        PlayFabMultiplayerAPI.CreateMatchmakingTicket(
            new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new EntityKey
                    {
                        Id = authData.Id,
                        Type = "title_player_account"
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new { }
                    }
                },
                QueueName = GetQueueName(type),
                GiveUpAfterSeconds = Timeout
            },
            OnMatchMakingTicketCreated,
            OnMatchMakingTicketError);
    }

    /**
     * Maps matchmaking type to playfab queue name.
     */
    private static string GetQueueName(MatchMakingType type)
    {
        return type switch
        {
            MatchMakingType.Duel => PlayFabQueueNames.DevDuelQueueName,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private void OnMatchMakingTicketError(PlayFabError obj)
    {
        _currentMatchMakingErrorHandler?.Invoke(obj.ErrorMessage);
    }

    /**
     * Matchmaking ticket created handler. Its very important to track request id. After we received confirmation
     * of ticket is created we need to track it, we start coroutine and call handler with new information.
     */
    private void OnMatchMakingTicketCreated(CreateMatchmakingTicketResult obj)
    {
        _currentMatchMakingRequestId = obj.TicketId;
        var result = new MatchMakingResult
        {
            Status = MatchMakingStatus.Searching,
            Type = _currentMatchMakingTypeRequested,
            RequestId = _currentMatchMakingRequestId
        };
        _currentMatchMakingResultHandler?.Invoke(result);
        _matchMakingTicketProcessing = StartCoroutine(PollMatchMakingTicket());
    }

    /**
     * Cancels given ticket if it exists.
     */
    public void CancelRegistration(Action<MatchMakingResult> onSuccess, Action<string> onError)
    {
        if (_currentMatchMakingRequestId == null)
        {
            return;
        }

        PlayFabMultiplayerAPI.CancelMatchmakingTicket(
            new CancelMatchmakingTicketRequest
            {
                TicketId = _currentMatchMakingRequestId,
                QueueName = GetQueueName(_currentMatchMakingTypeRequested)
            },
            success => OnCancelMatchMakingSuccess(onSuccess, success),
            OnMatchMakingTicketError);
    }

    /**
     * Success cancellation handler. If called means playfab successfully canceled ticket, we can now clear state.  
     */
    private void OnCancelMatchMakingSuccess(Action<MatchMakingResult> onSuccess, CancelMatchmakingTicketResult success)
    {
        var result = new MatchMakingResult
        {
            Status = MatchMakingStatus.CancelledByUser,
            Type = _currentMatchMakingTypeRequested,
            RequestId = _currentMatchMakingRequestId
        };
        onSuccess?.Invoke(result);
        ClearMatchMakingData();
    }

    /**
     * Clearing the state of current request. Careful call this method because it wipes request id, if we lost it
     * we need to wait for ticket timeout before it will be removed from playfab queue.
     */
    private void ClearMatchMakingData()
    {
        StopCoroutine(_matchMakingTicketProcessing);
        _matchMakingTicketProcessing = null;
        _currentMatchMakingErrorHandler = null;
        _currentMatchMakingRequestId = null;
        _currentMatchMakingResultHandler = null;
    }

    /**
     * Coroutine that checks status of the ticket each second until cancelled
     */
    private IEnumerator PollMatchMakingTicket()
    {
        while (true)
        {
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = _currentMatchMakingRequestId,
                    QueueName = GetQueueName(_currentMatchMakingTypeRequested)
                },
                OnSuccessPollMatchMakingTicket,
                OnErrorPollMatchMakingTicket
            );
            yield return new WaitForSeconds(1);
        }
    }

    private void OnErrorPollMatchMakingTicket(PlayFabError obj)
    {
        _currentMatchMakingErrorHandler?.Invoke(obj.ErrorMessage);
        ClearMatchMakingData();
    }

    /**
     * Playfab ticket status update handler. If match is found we stop poll coroutine and then checking the match
     * data for a server address and port. If cancelled we wipe out request.
     */
    private void OnSuccessPollMatchMakingTicket(GetMatchmakingTicketResult obj)
    {
        switch (obj.Status)
        {
            case "Matched":
                var matchedResult = new MatchMakingResult
                {
                    Status = MatchMakingStatus.MatchFound,
                    Type = _currentMatchMakingTypeRequested,
                    RequestId = _currentMatchMakingRequestId
                };
                _currentMatchMakingResultHandler?.Invoke(matchedResult);
                StopCoroutine(_matchMakingTicketProcessing);
                RequestMatchData(obj.MatchId, obj.QueueName);
                break;
            case "Cancelled":
                var cancelledResult = new MatchMakingResult
                {
                    Status = MatchMakingStatus.CancelledByService,
                    Type = _currentMatchMakingTypeRequested,
                    RequestId = _currentMatchMakingRequestId
                };
                _currentMatchMakingResultHandler?.Invoke(cancelledResult);
                ClearMatchMakingData();
                break;
        }
    }

    private void RequestMatchData(string matchId, string queueName)
    {
        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = queueName
            },
            OnSuccessGetMatch,
            OnErrorGetMatch);
    }

    private void OnErrorGetMatch(PlayFabError obj)
    {
        _currentMatchMakingErrorHandler?.Invoke(obj.ErrorMessage);
        ClearMatchMakingData();
    }

    private void OnSuccessGetMatch(GetMatchResult obj)
    {
        var result = new MatchMakingResult
        {
            Status = MatchMakingStatus.MatchReady,
            Type = _currentMatchMakingTypeRequested,
            RequestId = _currentMatchMakingRequestId,
            ServerAddress = obj.ServerDetails.IPV4Address,
            ServerPort = obj.ServerDetails.Ports[0].Num.ToString()
        };
        _currentMatchMakingResultHandler.Invoke(result);
    }
}