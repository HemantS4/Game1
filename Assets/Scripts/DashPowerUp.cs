using UnityEngine;

public class DashPowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController2d player = other.GetComponent<PlayerController2d>();
        if (player)
        {
            player.EnableDash();
            Destroy(gameObject);
        }
    }
}
