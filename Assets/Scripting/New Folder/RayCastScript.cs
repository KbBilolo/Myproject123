using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RayCastScript : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    public float rayDistance = 10f;

    //public StoryDialogManager dialogManager; // Assign in Inspector
    //private StoryDialogData currentTargetDialog; // The dialog data of the NPC being looked at

    void Update()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, rayDistance))
        {
            Debug.DrawLine(origin, hit.point, Color.red);

            // Check for tag
            if (hit.collider.CompareTag("Professor Reyes"))
            {
                //var npcDialog = hit.collider.GetComponent<NPCDialogTrigger>();

                // if (npcDialog != null)
            }
            else
            {
              //  currentTargetDialog = null;
            }
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * rayDistance, Color.green);
           // currentTargetDialog = null;
        }
    }

    // Call this from a UI button or input event
    public void TryStartDialog()
    {
       // if (currentTargetDialog != null && dialogManager != null)
       // {
       //     dialogManager.StartDialogue(currentTargetDialog);
        //}
    }
}
