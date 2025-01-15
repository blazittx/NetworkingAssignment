using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeController_Networked : NetworkBehaviour
{
    public float angleVariation = 10f;
    public float baseSpeed = 10f;
    public float speedIncrement = 2f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (IsServer)
        {
            SetInitialVelocity();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        AddVelocityVariationAndIncrementServerRpc();
    }

    [ServerRpc]
    private void AddVelocityVariationAndIncrementServerRpc()
    {
        Vector3 newVelocity = AddVariationAndIncrement(_rb.linearVelocity);
        _rb.linearVelocity = newVelocity;

        SynchronizeVelocityClientRpc(newVelocity);
    }

    [ClientRpc]
    private void SynchronizeVelocityClientRpc(Vector3 velocity)
    {
        if (!IsServer)
        {
            _rb.linearVelocity = velocity;
        }
    }

    private Vector3 AddVariationAndIncrement(Vector3 velocity)
    {
        float randomAngle = Random.Range(-angleVariation, angleVariation);
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        Vector3 modifiedVelocity = rotation * velocity;
        float newSpeed = modifiedVelocity.magnitude + speedIncrement;
        modifiedVelocity = modifiedVelocity.normalized * newSpeed;

        return modifiedVelocity;
    }

    private void SetInitialVelocity()
    {
        Vector3 initialDirection = new Vector3(
            Random.Range(-1f, -0.7f),
            0,
            Random.Range(1f, 0.7f)
        ).normalized;

        _rb.linearVelocity = initialDirection * baseSpeed;
    }
}
