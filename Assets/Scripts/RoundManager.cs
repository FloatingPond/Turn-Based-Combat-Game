using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class RoundManager : MonoBehaviour
    {
        public int currentRound = 1;
        public enum TurnOwner { Player, Computer }
        public TurnOwner CurrentTurnOwner;
        public UnitController unitTakingTurn_UnitController;
        [SerializeField] private LevelData level;
        private List<CinemachineVirtualCamera> cameras;
        [SerializeField] private Transform playerUnitsTransform, computerUnitsTransform;

        [Header("Spawning"), Space(10)]
        [SerializeField] private float spawnHeight = 0;
        [SerializeField] private int maxSpawnAttempts = 100;
        //[SerializeField] private LayerMask ground;
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
            CalculateInitiative();
            InitialiseUnits();
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
           UIManager.Instance.UpdateActionText();
        }
        private void CalculateInitiative()
        {
            int coinFlip = Random.Range(0, 2);
            if (coinFlip == 0)
            {
                CurrentTurnOwner = TurnOwner.Player;
            }
            else if (coinFlip == 1)
            {
                CurrentTurnOwner= TurnOwner.Computer;
            }
        }
        private void InitialiseUnits()
        {
            for (int i = 0; i < level.PlayerUnitAmount; i++)
            {
                SpawnPlayerControlledUnit();
            }
            for (int i = 0; i < level.ComputerUnitAmount; i++)
            {
                SpawnComputerControlledUnit();
            }
        }

        public void SpawnPlayerControlledUnit()
        {
            Vector3 spawnPosition = GenerateSpawnPosition(-5, 5, -15, 15);

            if (GetNavMeshPosition(spawnPosition, out Vector3 navMeshPosition, 1))
            {
                Instantiate(level.PlayerUnits[Random.Range(0, level.PlayerUnits.Count)], navMeshPosition, Quaternion.identity, playerUnitsTransform);
            }
        }
        public void SpawnComputerControlledUnit()
        {
            Vector3 spawnPosition = GenerateSpawnPosition(-5, 5, 25, 50);

            if (GetNavMeshPosition(spawnPosition, out Vector3 navMeshPosition, 1))
            {
                Instantiate(level.ComputerUnits[Random.Range(0, level.ComputerUnits.Count)], navMeshPosition, Quaternion.identity, computerUnitsTransform);
            }
        }
        public void TransitionCameraToUnit(UnitController unit)
        {
            foreach (CinemachineVirtualCamera cam in cameras)
            {
                if (cam.transform.parent == unit.transform)
                {
                    cam.Priority = 1;
                }
                else
                {
                    cam.Priority = 0;
                }
            }
        }

        public void TransitionCameraToMain()
        {
            foreach (CinemachineVirtualCamera cam in cameras)
            {
                if (cam.name == "InputCam")
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
        Vector3 GenerateSpawnPosition(int minX, int maxX, int minZ, int maxZ)
        {
            Vector3 spawnPosition;

            int groundLayer = LayerMask.NameToLayer("Ground");
            int layerMask = ~(1 << groundLayer);

            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                int randomX = Random.Range(minX, maxX);
                int randomZ = Random.Range(minZ, maxZ);

                spawnPosition = new Vector3(RoundToNearestHalf(randomX) + 0.5f, spawnHeight,RoundToNearestHalf(randomZ) + 0.5f);
                Collider[] colliders = Physics.OverlapBox(spawnPosition, transform.localScale / 2, Quaternion.identity, layerMask);
                if (colliders.Length == 0)
                {
                    return spawnPosition;
                }
            }
            Debug.LogWarning("Failed to find a valid spawn position after " + maxSpawnAttempts + " attempts.");
            return Vector3.zero;
        }
        bool GetNavMeshPosition(Vector3 spawnPosition, out Vector3 navMeshPosition, float maxDistanceFromNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, maxDistanceFromNavMesh, NavMesh.AllAreas))
            {
                navMeshPosition = hit.position;
                return true;
            }

            navMeshPosition = Vector3.zero;
            return false;
        }
        public static float RoundToNearestHalf(float a)
        {
            a += 0.5f;
            Mathf.RoundToInt(a);
            a -= 0.5f;
            return a;
        }
    }
}
