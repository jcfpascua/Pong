using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PaddleMovement : MonoBehaviour
{
    public float speed = 10f;
    public bool isPlayerOne = true;

    // Movement limits along X axis
    public float minX = -4f;
    public float maxX = 4f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // paddle won't be pushed
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void FixedUpdate()
    {
        float moveInput = 0f;

        // Player Two controls
        if (!isPlayerOne)
        {
            if (Input.GetKey(KeyCode.W)) moveInput = 1f;
            if (Input.GetKey(KeyCode.S)) moveInput = -1f;
        }

        // Player One controls
        if (isPlayerOne)
        {
            if (Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;
        }

        // Calculate target position along X axis only
        float targetX = Mathf.Clamp(rb.position.x + moveInput * speed * Time.fixedDeltaTime, minX, maxX);
        Vector3 targetPos = new Vector3(targetX, rb.position.y, rb.position.z);

        rb.MovePosition(targetPos);
    }
}
