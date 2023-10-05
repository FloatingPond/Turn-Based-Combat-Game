using UnityEngine;

namespace PG
{
    public class RotateTowardsPlayer : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.transform, Vector3.up);
            transform.rotation *= Quaternion.Euler(0, 180, 0); // Invert the y-axis
        }
    }
}
