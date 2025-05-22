using UnityEngine;

public class RealityShiftManager : MonoBehaviour
{
    public GameObject currentBackground;
    public GameObject alternateBackground;

    public GameObject[] currentPlatforms;
    public GameObject[] alternatePlatforms;

    public CameraFollowAndShake cameraFollowAndShake;


    private bool isAlternate = false;

    public void SwapReality()
    {
        isAlternate = !isAlternate;

        currentBackground.SetActive(!isAlternate);
        alternateBackground.SetActive(isAlternate);

        foreach (GameObject obj in currentPlatforms)
            obj.SetActive(!isAlternate);

        foreach (GameObject obj in alternatePlatforms)
            obj.SetActive(isAlternate);

        
    }

}
