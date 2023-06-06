using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float speed;
    private MeshRenderer rend;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        rend.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
