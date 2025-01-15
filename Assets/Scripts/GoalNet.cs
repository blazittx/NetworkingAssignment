using Unity.Netcode;
using UnityEngine;

public class GoalNet : NetworkBehaviour
{
    public enum Owner
    {
        Player,
        Enemy
    }

    [SerializeField] private Owner goalOwner;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            if (IsServer)
            {
                HandleGoalScored();
                DespawnAndDestroy(other.gameObject);
            }
        }
    }

    private void HandleGoalScored()
    {
        if (goalOwner == Owner.Player)
        {
            gameManager.EnemyScoredServerRpc();
        }
        else if (goalOwner == Owner.Enemy)
        {
            gameManager.PlayerScoredServerRpc();
        }
    }
    
    private void DespawnAndDestroy(GameObject target)
    {
        var networkObject = target.GetComponent<NetworkObject>();
        if (networkObject != null && networkObject.IsSpawned)
        {
            networkObject.Despawn(true);
        }

        Destroy(target);
    }
}