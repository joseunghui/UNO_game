using BackEnd.Tcp;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchServer
{
    ErrorInfo errorInfo;

    public void AccessMatchServer()
    {
        Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) =>
        {
            Debug.Log($"check >> {errorInfo}");

            // TODO
            Backend.Match.OnMatchMakingRoomCreate = (MatchMakingInteractionEventArgs args) =>
            {
                // TODO
                
            };
        };

    }

    public void DisconnectMatchServer()
    {
        Backend.Match.OnLeaveMatchMakingServer = (LeaveChannelEventArgs args) =>
        {
            // TODO
            Debug.Log("Disconnect!");
        };
    }
}
