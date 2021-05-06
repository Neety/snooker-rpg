using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Animator cameraSwitch;
    private bool mapCam = true;
    void Awake()
    {
        cameraSwitch = GetComponent<Animator>();
    }

    public void SwitchCam()
    {
        if (mapCam)
            cameraSwitch.Play("EntityCam");
        else
            cameraSwitch.Play("MapCam");

        mapCam = !mapCam;
    }

}
