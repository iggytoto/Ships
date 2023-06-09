using System;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.EcsSandBox
{
 public class Frontend : MonoBehaviour
    {
        const ushort k_NetworkPort = 7979;

        public InputField Address;
        public Button ClientServerButton;

        public void Start()
        {
            ClientServerButton.gameObject.SetActive(ClientServerBootstrap.RequestedPlayType == ClientServerBootstrap.PlayType.ClientAndServer);
        }

        public void StartClientServer(string sceneName)
        {
            if (ClientServerBootstrap.RequestedPlayType != ClientServerBootstrap.PlayType.ClientAndServer)
            {
                Debug.LogError($"Creating client/server worlds is not allowed if playmode is set to {ClientServerBootstrap.RequestedPlayType}");
                return;
            }

            var server = ClientServerBootstrap.CreateServerWorld("ServerWorld");
            var client = ClientServerBootstrap.CreateClientWorld("ClientWorld");

            SceneManager.LoadScene(GetLoadingTempSceneName());

            //Destroy the local simulation world to avoid the game scene to be loaded into it
            //This prevent rendering (rendering from multiple world with presentation is not greatly supported)
            //and other issues.
            DestroyLocalSimulationWorld();
            if (World.DefaultGameObjectInjectionWorld == null)
                World.DefaultGameObjectInjectionWorld = server;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            var addressPort = ParseAddressPort(Address.text);

            NetworkEndpoint ep = NetworkEndpoint.AnyIpv4.WithPort(addressPort.port);
            {
                using var drvQuery = server.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ep);
            }

            ep = NetworkEndpoint.LoopbackIpv4.WithPort(addressPort.port);
            {
                using var drvQuery = client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(client.EntityManager, ep);
            }
        }

        private static string GetLoadingTempSceneName()
        {
            return "SandboxTemp";
        }

        public void StartClientServer()
        {
            StartClientServer(GetLoadingSceneName());
        }

        private static string GetLoadingSceneName()
        {
            return "Sandbox";
        }

        public void ConnectToServer()
        {
            var client = ClientServerBootstrap.CreateClientWorld("ClientWorld");
            SceneManager.LoadScene(GetLoadingTempSceneName());
            DestroyLocalSimulationWorld();
            
            if (World.DefaultGameObjectInjectionWorld == null)
                World.DefaultGameObjectInjectionWorld = client;
            SceneManager.LoadSceneAsync(GetLoadingSceneName(), LoadSceneMode.Additive);

            var addressPort = ParseAddressPort(Address.text);
            var ep = NetworkEndpoint.Parse(addressPort.address, addressPort.port);
            {
                using var drvQuery = client.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(client.EntityManager, ep);
            }
        }
        

        protected void DestroyLocalSimulationWorld()
        {
            foreach (var world in World.All)
            {
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                    break;
                }
            }
        }

        // Tries to parse a port, returns true if successful, otherwise false
        // The port will be set to whatever is parsed, otherwise the default port of k_NetworkPort
        private (string address, UInt16 port) ParseAddressPort(string s)
        {
            var parts = s.Split(":");

            var port = default(ushort);
            var address = parts[0];
            
            if (!UInt16.TryParse(parts[1], out port))
            {
                throw new ArgumentException(nameof(s));
                //Debug.LogWarning($"Unable to parse port, using default port {k_NetworkPort}");
            }

            return (address, port);
        }
    }
}