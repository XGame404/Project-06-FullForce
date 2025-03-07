using UnityEngine;

public class ItemChanging : MonoBehaviour
{
    public int RandomAppearance;

    [SerializeField] private GameObject Item01;
    [SerializeField] private GameObject Item02;
    [SerializeField] private GameObject Item03;
    [SerializeField] private GameObject Item04;

    [SerializeField] private bool IsExtraPush;
    [SerializeField] private bool IsCoinIcon;

    [SerializeField] private float PatrolTime = 0.5f;
    private float CurrentPatrolTime;
    private float ItemIDByTime = -1f;

    void Start()
    {
        CurrentPatrolTime = PatrolTime;
        RandomAppearance = Random.Range(0, 4);
        Item01.SetActive(RandomAppearance == 0);
        Item02.SetActive(RandomAppearance == 1);
        Item03.SetActive(RandomAppearance == 2);
        Item04.SetActive(RandomAppearance == 3);
    }

    private void Update()
    {
        CurrentPatrolTime -= Time.deltaTime;

        if (IsExtraPush)
        {
            if (CurrentPatrolTime > 0)
            {
                transform.Translate(new Vector2(-2 * Time.deltaTime, 0), Space.World);
            }
            else if (CurrentPatrolTime > -PatrolTime)
            {
                transform.Translate(new Vector2(2 * Time.deltaTime, 0), Space.World);
            }
            else
            {
                CurrentPatrolTime = PatrolTime;
            }
        }

        if (IsCoinIcon)
        {
            ItemIDByTime += Time.deltaTime;

            if (ItemIDByTime > 3) 
            {
                ItemIDByTime = -1f;
            }

            Item01.SetActive(ItemIDByTime <= 0);
            Item02.SetActive(ItemIDByTime <= 1 && ItemIDByTime > 0);
            Item03.SetActive(ItemIDByTime <= 2 && ItemIDByTime > 1);
            Item04.SetActive(ItemIDByTime <= 3 && ItemIDByTime > 2);
        }
    }
}