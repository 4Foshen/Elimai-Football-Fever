using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts")]
    public GameManager gameManager;
    [Header("Parametrs")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float rotateSpeed;

    private Rigidbody2D player;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * -1f * Time.deltaTime);
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Jump();
            }
        }
    }
    public void Jump()
    {
        player.velocity = Vector2.up * jumpPower;
        gameManager.audioSource.PlayOneShot(gameManager.jumpSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
            FindObjectOfType<GameManager>().GameOver();
        else if (collision.gameObject.tag == "Score")
            FindObjectOfType<GameManager>().IncreaseScore();
    }

    private void OnEnable()
    {
        player.bodyType = RigidbodyType2D.Dynamic;
        Jump();
    }

    private void OnDisable()
    {
        player.bodyType = RigidbodyType2D.Static;
    }
    
}