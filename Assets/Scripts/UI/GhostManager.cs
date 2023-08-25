using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class GhostManager : MonoBehaviour
    {
        public SkinnedMeshRenderer skinnedMesh;
        public List<MeshRenderer> renderers;
        // Start is called before the first frame update
        void Start()
        {
            HideGhost();
        }
        [Button]
        public void ShowGhost()
        {
            skinnedMesh.enabled = true;
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
        }
        [Button]
        public void HideGhost()
        {
            skinnedMesh.enabled = false;
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }
}
