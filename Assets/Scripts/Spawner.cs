using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float prepareTime;
    [SerializeField] private float spawnRate;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minVariance = 0.5f;

    private Vector3 previousPosition;

    public GameObject flagsPrefab;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), prepareTime, spawnRate);
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }
    private void Spawn()
    {
        GameObject flags = Instantiate(flagsPrefab, transform.position, Quaternion.identity);

        if (previousPosition.y > 0f)
        {
            flags.transform.position += Vector3.up * Random.Range(minHeight, -minVariance);
        }
        else
        {
            flags.transform.position += Vector3.up * Random.Range(minVariance, maxHeight);
        }

        previousPosition = flags.transform.position;
    }
}
