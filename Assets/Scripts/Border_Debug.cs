using Unity.VisualScripting;
using UnityEngine;

public class Border_Debug : MonoBehaviour
{   public GameObject startPoint;
    public GameObject endPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
            Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
    }
}
