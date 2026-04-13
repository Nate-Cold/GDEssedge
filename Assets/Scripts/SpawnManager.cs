using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        int index = (int)(clientId % (ulong)spawnPoints.Length);
        NetworkObject player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        player.transform.position = spawnPoints[index].position;
    }
}
