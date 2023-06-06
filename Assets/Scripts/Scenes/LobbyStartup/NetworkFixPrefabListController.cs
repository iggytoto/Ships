using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

//can be deleted after 1.4.1 https://forum.unity.com/threads/network-prefabs-not-initializing-correctly.1430224/
public class NetworkFixPrefabListController : MonoBehaviour
{
    [SerializeField] private NetworkPrefabsList _networkPrefabsList;
 
    private void Start()
    {
        RegisterNetworkPrefabs();
    }
 
    private void RegisterNetworkPrefabs()
    {
        var prefabs = _networkPrefabsList.PrefabList.Select(x => x.Prefab);
        foreach (var prefab in prefabs)
        {
            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }
}