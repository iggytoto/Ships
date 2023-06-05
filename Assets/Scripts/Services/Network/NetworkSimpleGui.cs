using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkSimpleGui : MonoBehaviour
{
    public static string hostPort = "127.0.0.1:7777";
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        hostPort = GUILayout.TextField(hostPort);
        

        if (GUILayout.Button("Host (Server + Client)"))
        {
            SetupHostPort(hostPort);
            NetworkManager.Singleton.StartHost();
        }

        if (GUILayout.Button("Client"))
        {
            SetupHostPort(hostPort);
            NetworkManager.Singleton.StartClient();
        }

        if (GUILayout.Button("Server (Server)"))
        {
            SetupHostPort(hostPort);
            NetworkManager.Singleton.StartServer();
        }
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
                        NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }


    static void SetupHostPort(string hostPort)
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        var hostPortArgs = hostPort.Split(":");
        transport.ConnectionData.Address = hostPortArgs[0];
        transport.ConnectionData.Port = ushort.Parse(hostPortArgs[1]);
    }
}
