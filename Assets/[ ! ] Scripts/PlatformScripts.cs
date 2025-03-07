using UnityEngine;

public class PlatformScripts : MonoBehaviour
{
    [SerializeField] private GameObject NormalPushItem;
    [SerializeField] private GameObject ExtraPushItem;
    [SerializeField] private Transform ItemSpawnPoint;

    [SerializeField] private GameObject[] EnemySpawnPoints;
    [SerializeField] private GameObject EnemyPrefab;

    void Start()
    {
        GameObject Item = null;

        if (Random.Range(0, 10) > 2)
        {
            Item = Instantiate(NormalPushItem, ItemSpawnPoint.position, Quaternion.identity);
            Item.transform.parent = null;
        }
        else
        {
            Item = Instantiate(ExtraPushItem, ItemSpawnPoint.position, Quaternion.identity);
            Item.transform.parent = null;
        }

        Item.transform.parent = transform;

        if (Random.Range(0, 20) <= 3)
        {
            Instantiate(EnemyPrefab, EnemySpawnPoints[Random.Range(0, EnemySpawnPoints.Length)].transform.position, Quaternion.identity);
            Item.transform.parent = null;
        }

    }

}
