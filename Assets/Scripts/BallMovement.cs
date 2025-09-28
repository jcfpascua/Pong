using UnityEngine;
using TMPro;

public class BallMovement : MonoBehaviour
{
    public float initialSpeed = 8f;
    public float speedIncrease = 0.5f;
    public float maxXOffset = 1f;

    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    public int maxScore = 5;

    public TextMeshProUGUI playerOneText; // assign "Player 1 Score"
    public TextMeshProUGUI playerTwoText; // assign "Player 2 Score"
    public TextMeshProUGUI winText;       // assign "WinText"

    private Rigidbody rb;
    private float startY;
    private bool gameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        startY = rb.position.y;
        LaunchBall();
        UpdateScoreUI();
        if (winText != null) winText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
            playerOneScore = 0;
            playerTwoScore = 0;
            UpdateScoreUI();
            gameOver = false;
            if (winText != null) winText.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerOne" || collision.gameObject.name == "PlayerTwo")
        {
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = collision.collider.bounds.center;
            float offsetX = Mathf.Clamp(hitPoint.x - paddleCenter.x, -maxXOffset, maxXOffset);
            float newSpeed = rb.linearVelocity.magnitude + speedIncrease;
            float zDir = Mathf.Sign(rb.linearVelocity.z);
            rb.linearVelocity = new Vector3(offsetX, 0f, zDir).normalized * newSpeed;
            return;
        }

        if (collision.gameObject.name == "SideWall1" || collision.gameObject.name == "SideWall2")
        {
            float targetX = 0f;
            float directionX = Mathf.Sign(targetX - rb.position.x);
            rb.linearVelocity = new Vector3(directionX * Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;

        if (other.gameObject.name == "BackPlayerOne")
        {
            playerTwoScore++;
            UpdateScoreUI();
            CheckWin();
            if (!gameOver) ResetGame();
        }
        else if (other.gameObject.name == "BackPlayerTwo")
        {
            playerOneScore++;
            UpdateScoreUI();
            CheckWin();
            if (!gameOver) ResetGame();
        }
    }

    void LaunchBall()
    {
        rb.isKinematic = false;
        rb.position = new Vector3(0f, startY, 0f);
        rb.linearVelocity = new Vector3(0f, 0f, Random.value < 0.5f ? -initialSpeed : initialSpeed);
        rb.angularVelocity = Vector3.zero;
    }

    public void ResetGame()
    {
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = new Vector3(0f, startY, 0f);
        rb.rotation = Quaternion.identity;
        Invoke(nameof(LaunchBall), 0.5f);
    }

    void UpdateScoreUI()
    {
        if (playerOneText != null) playerOneText.text = playerOneScore.ToString();
        if (playerTwoText != null) playerTwoText.text = playerTwoScore.ToString();
    }

    void CheckWin()
    {
        if (playerOneScore >= maxScore)
        {
            gameOver = true;
            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                winText.text = "Player One Wins!";
            }
            rb.isKinematic = true;
        }
        else if (playerTwoScore >= maxScore)
        {
            gameOver = true;
            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                winText.text = "Player Two Wins!";
            }
            rb.isKinematic = true;
        }
    }
}
