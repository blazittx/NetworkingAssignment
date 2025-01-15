using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartHostOrClientButton : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    [ContextMenu("StartHost")]
    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Destroy(gameObject);
    }

    [ContextMenu("StartClient")]
    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Destroy(gameObject);
    }
}
