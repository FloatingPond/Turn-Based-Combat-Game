using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace PG
{
    public class GunShotRenderer : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        public InputManager inputManager;
        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            inputManager = FindObjectOfType<InputManager>();
        }
        public void DrawGunshot(Vector3 origin)
        {
            Vector3 destination = inputManager.hoverWorldPosition;
            Vector3 direction = (destination - origin).normalized;
            float distance = Vector3.Distance(origin, destination);
            RaycastHit hit;

            if (Physics.Raycast(origin, direction, out hit, distance))
            {
                transform.position = origin;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, origin);
                lineRenderer.SetPosition(1, destination);
            }
        }
    }
}
