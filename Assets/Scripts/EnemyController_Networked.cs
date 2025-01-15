using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController_Networked : NetworkBehaviour
{
    public GameObject cube;
    public float moveSpeed = 8f;
    public float interceptThreshold = 0.5f;
    public float reactionDelay = 0.2f;
    public float randomness = 1.5f;

    private Rigidbody _rb;
    private bool _canMove = false;
    private Vector3 _predictedTargetPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void StartAI()
    {
        if (IsServer)
        {
            cube = FindAnyObjectByType<CubeController_Networked>()?.gameObject;

            if (cube != null)
            {
                _canMove = true;
                InvokeRepeating(nameof(UpdatePredictedTarget), 0f, reactionDelay);
            }
            else
            {
                Debug.LogWarning("CubeController_Networked not found!");
            }
        }
    }

    private void UpdatePredictedTarget()
    {
        if (!_canMove || cube == null) return;

        _predictedTargetPosition = cube.transform.position;
        _predictedTargetPosition.z += Random.Range(-randomness, randomness);
    }

    private void FixedUpdate()
    {
        if (!IsServer || !_canMove || cube == null) return;

        float directionZ = 0;

        if (Mathf.Abs(_predictedTargetPosition.z - transform.position.z) > interceptThreshold)
        {
            directionZ = Mathf.Sign(_predictedTargetPosition.z - transform.position.z);
        }

        Vector3 movement = new Vector3(0, 0, directionZ) * moveSpeed;
        _rb.linearVelocity = new Vector3(0, 0, movement.z);
    }
}
