using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SpawnManager : NetworkBehaviour
{
    //array of spawn points set in the Unity Inspector
    [SerializeField] private Transform[] spawnPoints;

    //called when this object spawns on the network
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
        //Debug.Log($"IsOwner: {IsOwner}, IsHost: {IsHost}, IsLocalPlayer: {IsLocalPlayer}");
    }

    //runs every time a new client joins
    private void OnClientConnected(ulong clientId)
    {
        //PlayerMovement[] players = FindAnyObjectByType<PlayerMovement>();
        //ulong hostId = NetworkManager.Singleton.LocalClientId;

        
        
        //cycle through spawn points based on client ID
        // % (modulo) wraps the index so it never goes out of bounds
        int index = (int)(clientId % (ulong)spawnPoints.Length);
        NetworkObject player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

        //move player to the assigned spawn point
        player.transform.position = spawnPoints[index].position;
    }
}
