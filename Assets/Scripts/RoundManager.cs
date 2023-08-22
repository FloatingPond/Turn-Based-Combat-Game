using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PG
{
    public class RoundManager : MonoBehaviour
    {
        public int currentRound = 1;
        public UnitManager unitTakingTurn;
        private List<CinemachineVirtualCamera> cameras;
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
                Debug.LogWarning("No virtual cameras in scene.");
            }
        }
        public void CalculateInitiativeOrder()
        {
           List<UnitManager> units = new List<UnitManager> ();
           units = FindObjectsOfType<UnitManager>().ToList<UnitManager>();
           foreach (UnitManager unit in units)
           {
                unit.initiative = Random.Range(1, 20);
           }
           unitTakingTurn = units.First();
           TransitionCameraToUnit(unitTakingTurn);
        }
        private void TransitionCameraToUnit(UnitManager unit)
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
