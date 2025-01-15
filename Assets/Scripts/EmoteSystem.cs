using Unity.Netcode;
using UnityEngine;

public class EmoteSystem : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer emoteRenderer;
    [SerializeField] private Sprite[] emoteSprites;

    private void Start()
    {
        if (emoteRenderer == null)
        {
            emoteRenderer = GetComponent<SpriteRenderer>();
        }

        emoteRenderer.enabled = false;
    }

    private void Update()
    {
        if (!IsLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) TriggerEmoteServerRpc(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TriggerEmoteServerRpc(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TriggerEmoteServerRpc(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TriggerEmoteServerRpc(3);
    }

    [ServerRpc]
    private void TriggerEmoteServerRpc(int emoteIndex)
    {
        if (emoteIndex < 0 || emoteIndex >= emoteSprites.Length) return;

        TriggerEmoteClientRpc(emoteIndex);
    }

    [ClientRpc]
    private void TriggerEmoteClientRpc(int emoteIndex)
    {
        if (emoteRenderer == null || emoteIndex < 0 || emoteIndex >= emoteSprites.Length) return;

        emoteRenderer.sprite = emoteSprites[emoteIndex];
        emoteRenderer.enabled = true;

        CancelInvoke(nameof(HideEmote));
        Invoke(nameof(HideEmote), 2f);
    }

    private void HideEmote()
    {
        if (emoteRenderer != null)
        {
            emoteRenderer.enabled = false;
        }
    }
}