using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 3f;

    [SerializeField] private GameObject Meteor01;
    [SerializeField] private GameObject Meteor02;
    [SerializeField] private GameObject Meteor03;
    [SerializeField] private GameObject Meteor04;
    void Start()
    {
        int RandomAppearance = Random.Range(0, 4);
        Meteor01.SetActive(RandomAppearance == 0);
        Meteor02.SetActive(RandomAppearance == 1);
        Meteor03.SetActive(RandomAppearance == 2);
        Meteor04.SetActive(RandomAppearance == 3);
    }

    private void Update()
    {
        this.gameObject.transform.Translate(0, -MoveSpeed * Time.deltaTime, 0);
    }
}
