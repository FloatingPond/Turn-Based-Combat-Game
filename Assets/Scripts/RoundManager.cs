using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PG
{
    public class RoundManager : MonoBehaviour
    {
        public int currentRound = 1;
        public UnitController unitTakingTurn;
        private List<CinemachineVirtualCamera> cameras;
        #region Singleton
        public static RoundManager Instance { get; private set; }

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
        // Start is called before the first frame update
        void Start()
        {
            GetVcams();
            CalculateInitiativeOrder();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void GetVcams()
        {
            cameras = new List<CinemachineVirtualCamera>();
            cameras = FindObjectsOfType<CinemachineVirtualCamera>().ToList();
            if (cameras.Count == 0)
            {
                Debug.LogWarning("No active virtual cameras in scene.");
            }
        }
        public void CalculateInitiativeOrder()
        {
           List<UnitController> units = new List<UnitController> ();
           units = FindObjectsOfType<UnitController>().ToList<UnitController>();
           foreach (UnitController unit in units)
           {
                unit.initiative = Random.Range(1, 20);
           }
           units.Sort((a, b) => b.initiative.CompareTo(a.initiative));
           unitTakingTurn = units[0];
           //TransitionCameraToUnit(unitTakingTurn);
        }
        private void TransitionCameraToUnit(UnitController unit)
        {
            foreach (CinemachineVirtualCamera cam in cameras)
            {
                if (cam.Follow == unit.transform)
                {
                    cam.Priority = 1;
                }
                else
                {
                    cam.Priority = 0;
                }
            }
        }

        public void NextRound()
        {
            currentRound++;
            //UPDATE UI WITH ROUND NUMBER
        }
    }
}
