using UnityEngine;

public enum CameraMode
{
    ThirdPerson,
    Minigame,
    Dialogue
}

public class CameraManager : MonoBehaviour
{
    public Camera thirdPersonCam;
    public Camera minigameCam;
    public Camera dialogCam;

    Camera currentCam;

    void Start()
    {
        SwitchCamera(CameraMode.ThirdPerson);
    }

    public void SwitchCamera(CameraMode mode)
    {
        // Disable all
        thirdPersonCam.gameObject.SetActive(false);
        minigameCam.gameObject.SetActive(false);
        dialogCam.gameObject.SetActive(false);

        switch (mode)
        {
            case CameraMode.ThirdPerson:
                Enable(thirdPersonCam);
                break;

            case CameraMode.Minigame:
                Enable(minigameCam);
                break;

            case CameraMode.Dialogue:
                Enable(dialogCam);
                break;
        }
    }

    void Enable(Camera cam)
    {
        cam.gameObject.SetActive(true);
        currentCam = cam;
    }

    public void changeThird()
    {
        SwitchCamera(CameraMode.ThirdPerson);
    }
    public void ChangeMinigame()
    {
        SwitchCamera(CameraMode.Minigame);

    }
    public void ChangeDialogue()
    {
        SwitchCamera(CameraMode.Dialogue);
    }
}
