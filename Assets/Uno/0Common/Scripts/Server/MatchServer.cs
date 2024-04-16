using BackEnd.Tcp;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchServer
{
    ErrorInfo errorInfo;

    static string pvcModeInDate = "2023-12-05T06:26:44.987Z";
    static string pvpModeInDate = "2023-12-05T02:43:08.723Z";

    public void AccessMatchServer()
    {
        Debug.Log("AccessMatchServer()");

        Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) =>
        {
            Debug.Log($"check >> {errorInfo}");

            // TODO
            Backend.Match.OnMatchMakingRoomCreate = (MatchMakingInteractionEventArgs args) =>
            {
                // TODO
                Backend.Match.RequestMatchMaking(MatchType.Random, MatchModeType.OneOnOne, pvpModeInDate);

                Debug.Log($"Match Request");
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
