using UnityEngine;

namespace PG
{
    public class GunShotRenderer : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        public Vector3 hitPosition;
        #region Singleton
        public static GunShotRenderer Instance { get; private set; }

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
        public void DrawGunshot(Vector3 origin, Vector3 destination)
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
                lineRenderer.SetPosition(1, destination);
            }
        }
        public void ClearGunshot()
        {
            lineRenderer.positionCount = 0;
        }
    }
}
