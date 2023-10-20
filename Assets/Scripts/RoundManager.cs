using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PG
{
    public class RoundManager : MonoBehaviour
    {
        public int currentRound = 1;
        public UnitController unitTakingTurn_UnitController;
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
        void Start()
        {
            GetVcams();
            CalculateInitiativeOrder();
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
           List<UnitController> units = new();
           units = FindObjectsOfType<UnitController>().ToList<UnitController>();
           foreach (UnitController unit in units)
           {
                unit.Initiative = Random.Range(1, 20);
           }
           units.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));
           unitTakingTurn_UnitController = units[0];
           UIManager.Instance.AimTargetIK = unitTakingTurn_UnitController.AimTargetIK;
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
