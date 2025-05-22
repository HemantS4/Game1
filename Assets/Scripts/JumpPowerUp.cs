using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    public enum PowerType { Jump, DoubleJump }
    public PowerType powerType;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController2d player = other.GetComponent<PlayerController2d>();
        if (player)
        {
            if (powerType == PowerType.Jump)
                player.EnableJump();
            else if (powerType == PowerType.DoubleJump)
                player.EnableDoubleJump();

            Destroy(gameObject);
        }
    }
}
