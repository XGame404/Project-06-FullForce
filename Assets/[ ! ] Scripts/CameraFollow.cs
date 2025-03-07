using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform TargetedPlayer;

    private bool IsFollowingPlayer;

    [SerializeField] private bool IsInMenu;

    void Start()
    {
        if (!IsInMenu)
            TargetedPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!IsInMenu)
        {
            Follow();
        }
        else 
        {
            Flow();
        }
    }
    void Follow()
    {
        if (!IsInMenu)
            TargetedPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        if (TargetedPlayer.position.y < this.gameObject.transform.position.y)
        {
            IsFollowingPlayer = false;
        }

        if (TargetedPlayer.position.y >= this.gameObject.transform.position.y)
        {
            IsFollowingPlayer = true;
        }

        if (IsFollowingPlayer == true)
        {
            Vector3 temp = this.gameObject.transform.position;
            temp.y = TargetedPlayer.position.y;
            this.gameObject.transform.position = temp;
        }

    }

    void Flow() 
    {
        this.gameObject.transform.Translate(Vector2.up * Time.deltaTime * 2.5f, Space.World);
    }
}
