using System;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private float leftEdge;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        if (gameManager.isPlay)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x < leftEdge)
                Destroy(gameObject);
        }
    }
}
