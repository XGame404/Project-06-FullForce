using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public static PlatformSpawner instance;

    [SerializeField] private GameObject Left_Platform;
    [SerializeField] private GameObject Right_Platform;

    [SerializeField] private float Left_X_Min = -7f, Left_X_Max = -2.5f;
    [SerializeField] private float Right_X_Min = 2.5f, Right_X_Max = 7f;

    [SerializeField] private float SpawnGap_Min = 2.5f;
    [SerializeField] private float SpawnGap_Max = 4.2f;
    private float SpawnGap;
    private float LastSpawnedPosition_Y;

    public int Spawn_Count = 8;
    private int Platform_Spawned;

    [SerializeField] private Transform ParentPlatform;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        SpawnGap = SpawnGap_Min; // Start with minimum gap
        LastSpawnedPosition_Y = transform.position.y;
        SpawnPlatforms();
    }

    public void SpawnPlatforms()
    {
        Vector2 temp = Vector2.zero;

        for (int i = 0; i < Spawn_Count; i++)
        {
            temp.y = LastSpawnedPosition_Y;
            GameObject newPlatform = null;

            if ((Platform_Spawned % 2) == 0)
            {
                temp.x = Random.Range(Right_X_Min, Right_X_Max);
                newPlatform = Instantiate(Right_Platform, temp, Quaternion.identity);
            }
            else
            {
                temp.x = Random.Range(Left_X_Min, Left_X_Max);
                newPlatform = Instantiate(Left_Platform, temp, Quaternion.identity);
            }

            newPlatform.transform.SetParent(null);
            LastSpawnedPosition_Y += SpawnGap;
            Platform_Spawned++;

            // Increase gap after every 15 platforms
            if (Platform_Spawned % 40 == 0) // Changed from 30 to 15
            {
                SpawnGap = Mathf.Clamp(SpawnGap + 0.1f, SpawnGap_Min, SpawnGap_Max);
            }
        }
    }
}