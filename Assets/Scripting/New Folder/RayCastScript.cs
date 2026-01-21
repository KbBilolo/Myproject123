using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastScript : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    public float rayDistance = 10f;
    public LayerMask layerMask;
    

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction =transform.forward;

        if (Physics.Raycast(origin, direction, out hit, rayDistance, layerMask))
        {

            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            Debug.DrawLine(origin, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * rayDistance, Color.green);
        }

    }
}
