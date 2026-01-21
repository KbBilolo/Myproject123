using UnityEngine;
using UnityEngine.EventSystems;

public class MobileSwipeLook : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public ThirdPersonCamera cameraController;

    [Header("Sensitivity")]
    public float swipeSensitivity = 0.15f;

    private Vector2 lastPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastPosition;
        lastPosition = eventData.position;

        Vector2 lookInput = new Vector2(delta.x, delta.y) * swipeSensitivity;
        cameraController.SetMobileLook(lookInput);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cameraController.SetMobileLook(Vector2.zero);
    }
}
