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
        public void PathToTransform(Transform target)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, target.position);
        }
    }
}
