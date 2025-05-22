using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController2D>();
            if (controller != null)
            {
                controller.ResetToFiveSecondsAgo();
            }
        }
    }
}
