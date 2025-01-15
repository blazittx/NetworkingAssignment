using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerTextDisplay : NetworkBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    private void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshPro>();
        }

        if (IsLocalPlayer)
        {
            string playerText = IsHost ? "P1" : "P2";
            UpdatePlayerTextServerRpc(playerText);
        }
    }

    [ServerRpc]
    private void UpdatePlayerTextServerRpc(string text)
    {
        UpdatePlayerTextClientRpc(text);
    }

    [ClientRpc]
    private void UpdatePlayerTextClientRpc(string text)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = text;
        }
    }
}