using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerType { DoubleJump, Dash, RealityShift }
    public PowerType power;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController2D>();
            if (controller == null) return;

            switch (power)
            {
                case PowerType.DoubleJump:
                    controller.UnlockDoubleJump();
                    break;
                case PowerType.Dash:
                    controller.UnlockDash();
                    break;
                case PowerType.RealityShift:
                    controller.UnlockRealityShift();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
