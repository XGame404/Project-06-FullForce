using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D target)
    {

        if (target.tag == "Background" || target.tag == "Platform" ||
            target.tag == "NormalPush" || target.tag == "ExtraPush" ||
            target.tag == "Bird")
        {

            Destroy(target.gameObject);
        }

    }
}
