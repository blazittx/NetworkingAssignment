using Unity.Netcode;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    private bool _isServerReady = false;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _isServerReady = true;
            }
            else
            {
                if (_isServerReady)
                {
                    SpawnCubeSequence();
                }
            }
        }
    }

    public void SpawnCubeSequence()
    {
        Invoke(nameof(SpawnCubeAfterDelay), 3.0f);
    }

    public void SpawnCubeAfterDelay()
{
    if (!NetworkManager.Singleton.IsServer) return;

    Vector3 spawnPosition = Vector3.zero;
    Quaternion spawnRotation = Quaternion.identity;

    var cubeInstance = Instantiate(cubePrefab, spawnPosition, spawnRotation);
    cubeInstance.GetComponent<NetworkObject>().Spawn();

    var enemyControllerNetworked = FindObjectOfType<EnemyController_Networked>();
    if (enemyControllerNetworked != null)
    {
        enemyControllerNetworked.StartAI();
    }
    else
    {
        Debug.LogWarning("No EnemyController_Networked found in the scene!");
    }
}

}