using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour
{
    [System.Serializable]
    public class TransformSettings
    {
        public Quaternion rotation;
        public Vector3 scale;
    }

    public TransformSettings[] backgroundTransforms;
    public GameObject backgroundPrefab;
    public Camera mainCamera;
    public float spawnOffset = 27.4f; 

    private Queue<GameObject> activeBackgrounds = new Queue<GameObject>();
    private float nextSpawnY = 0f;
    private int transformIndex = 0;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            SpawnBackground(nextSpawnY);
            nextSpawnY += 27.4f;
            transformIndex = (transformIndex + 1) % 4;
        }
    }

    void Update()
    {
        if (mainCamera.transform.position.y + spawnOffset > nextSpawnY)
        {
            SpawnBackground(nextSpawnY);
            nextSpawnY += 27.4f;
            transformIndex = (transformIndex + 1) % 4;
        }

    }

    void SpawnBackground(float yPos)
    {
        Vector3 position = new Vector3(0, yPos, 0);
        TransformSettings settings = backgroundTransforms[transformIndex];
        GameObject bg = Instantiate(backgroundPrefab, position, settings.rotation);
        bg.transform.localScale = settings.scale;
        activeBackgrounds.Enqueue(bg);
    }
}
