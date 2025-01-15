using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private CubeSpawner cubeSpawner;

    private void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        if (IsServer)
        {
            scoreManager.ResetScoresServerRpc();
        }
        
        cubeSpawner = FindObjectOfType<CubeSpawner>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerScoredServerRpc()
    {
        if (IsServer)
        {
            scoreManager.AddPlayerScoreServerRpc(1);
            Debug.Log("Player scored a goal!");
        }
        
        cubeSpawner.SpawnCubeSequence();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnemyScoredServerRpc()
    {
        if (IsServer)
        {
            scoreManager.AddEnemyScoreServerRpc(1);
            Debug.Log("Enemy scored a goal!");
        }
        
        cubeSpawner.SpawnCubeSequence();
    }
}