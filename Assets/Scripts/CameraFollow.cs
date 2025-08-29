using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        FindLocalPlayer();
    }

    void Update()
    {
        if (virtualCamera != null && virtualCamera.Follow == null)
        {
            FindLocalPlayer();
        }
    }

    private void FindLocalPlayer()
    {
        if (NetworkManager.Singleton == null) return;

        
        NetworkObject localPlayerNetObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

        if (localPlayerNetObj != null)
        {
            
            GameObject localPlayer = localPlayerNetObj.gameObject;
            virtualCamera.Follow = localPlayer.transform;
            virtualCamera.LookAt = localPlayer.transform;
        }
        else
        {
      
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                NetworkObject netObj = player.GetComponent<NetworkObject>();
                if (netObj != null && netObj.IsOwner)
                {
                    virtualCamera.Follow = player.transform;
                    virtualCamera.LookAt = player.transform;
                }
            }
        }
    }
}

