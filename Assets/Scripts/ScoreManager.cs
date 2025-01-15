using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    private NetworkVariable<int> _playerScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> _enemyScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;

    private void OnEnable()
    {
        _playerScore.OnValueChanged += OnPlayerScoreChanged;
        _enemyScore.OnValueChanged += OnEnemyScoreChanged;
    }

    private void OnDisable()
    {
        _playerScore.OnValueChanged -= OnPlayerScoreChanged;
        _enemyScore.OnValueChanged -= OnEnemyScoreChanged;
    }

    private void OnPlayerScoreChanged(int oldValue, int newValue)
    {
        playerScoreText.text = $"{newValue}";
    }

    private void OnEnemyScoreChanged(int oldValue, int newValue)
    {
        enemyScoreText.text = $"{newValue}";
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerScoreServerRpc(int points)
    {
        _playerScore.Value += points;
        Debug.Log($"Player's new score: {_playerScore.Value}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddEnemyScoreServerRpc(int points)
    {
        _enemyScore.Value += points;
        Debug.Log($"Enemy's new score: {_enemyScore.Value}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetScoresServerRpc()
    {
        _playerScore.Value = 0;
        _enemyScore.Value = 0;
        Debug.Log("Scores have been reset.");
    }
}