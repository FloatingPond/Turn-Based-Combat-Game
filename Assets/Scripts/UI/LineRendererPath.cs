using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class LineRendererPath : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        [Button]
        public void PathToTransform(Vector3 origin, Vector3 target)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, target);
        }
    }
}
