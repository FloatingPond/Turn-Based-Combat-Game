using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG
{
    public class WeaponTrajectoryIndicator : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        public Vector3 hitPosition;
        #region Singleton
        public static WeaponTrajectoryIndicator Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        public void DrawProjectedGunshot(Vector3 origin, Vector3 destination)
        {
            Vector3 direction = (destination - origin).normalized;
            float distance = Vector3.Distance(origin, destination);
            RaycastHit hit;

            if (Physics.Raycast(origin, direction, out hit, distance))
            {
                hitPosition = hit.point;
                transform.position = origin;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, origin);
                lineRenderer.SetPosition(1, hitPosition);
            }
        }
        public void ClearRendererPositions()
        {
            lineRenderer.positionCount = 0;
        }

        public void DrawGrenadeArc(List<Vector3> positions)
        {
            lineRenderer.positionCount = positions.Count;
            for (int i = 0; i < positions.Count; i++)
            {
                lineRenderer.SetPosition(i, positions[i]);
            }
        }
    }
}
