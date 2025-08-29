using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{

    private static GameManager instance;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        //print(NetworkManager.Singleton.LocalClientId);
        //InstancePlayerRpc(NetworkManager.Singleton.LocalClientId);

        if (IsServer)
        {
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
        }
        else
        {
            RequestSpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }


    [Rpc(SendTo.Server)]

    public void InstancePlayerRpc(ulong ownerID)
    {
        GameObject player = Instantiate(playerPrefab);
        //player.GetComponent<NetworkObject>().Spawn(true);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(ownerID, true);
    }

    void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnPlayerServerRpc(ulong clientId)
    {
        SpawnPlayer(clientId);
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(playerPrefab, GetSpawnPosition(), Quaternion.identity);
        NetworkObject networkObject = player.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(clientId, true);
    }

    private Vector3 GetSpawnPosition()
    {
        
        return new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
    }

    public static GameManager Instance => instance;
}
