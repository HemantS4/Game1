using UnityEngine;

public class RealityShiftTrigger : MonoBehaviour
{
    public GameObject worldToDeactivate;
    public GameObject worldToActivate;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (worldToDeactivate != null)
                worldToDeactivate.SetActive(false);

            if (worldToActivate != null)
                worldToActivate.SetActive(true);

            Destroy(gameObject); // remove the trigger object
        }
    }
}
