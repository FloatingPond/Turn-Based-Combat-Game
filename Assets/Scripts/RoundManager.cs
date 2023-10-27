using Cinemachine;
using CodeMonkey.CameraSystem;
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
        [SerializeField] private Transform playerUnitsTransform, computerUnitsTransform;
        [SerializeField] private List<CinemachineVirtualCamera> cameras;

        [Header("Spawning"), Space(10)]
        [SerializeField] private float spawnHeight = 0;
        [SerializeField] private int maxSpawnAttempts = 100;
        [SerializeField] private List<GameObject> spawnedPlayerUnits;
        [SerializeField] private List<GameObject> spawnedComputerUnits;

        [Header("Tools"), Space(10)]
        [SerializeField] private bool alwaysPlayerTurnFirst = false;
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
            CalculateInitiative();
            InitialiseUnits();
            GetVcams();
        }

        private void GetVcams()
        {
            cameras = FindObjectsOfType<CinemachineVirtualCamera>().ToList();
            if (cameras.Count == 0)
            {
                Debug.LogWarning("No active virtual cameras in scene.");
            }
        }
        public void CalculateInitiativeOrder()
        {
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
                CurrentTurnOwner = TurnOwner.Computer;
            }
            if (alwaysPlayerTurnFirst) CurrentTurnOwner = TurnOwner.Player;
            UIManager.Instance.UpdateCurrentTurnOwnerText();
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
            if (CurrentTurnOwner == TurnOwner.Player)
            {
                spawnedPlayerUnits[0].TryGetComponent(out UnitController unitContoller);
                unitTakingTurn_UnitController = unitContoller;
            }
            else
            {
                spawnedComputerUnits[0].TryGetComponent(out UnitController unitContoller);
                unitTakingTurn_UnitController = unitContoller;
            }    
        }

        public void SpawnPlayerControlledUnit()
        {
            Vector3 spawnPosition = GenerateSpawnPosition(-5, 5, -15, 15);

            if (GetNavMeshPosition(spawnPosition, out Vector3 navMeshPosition, 1))
            {
                spawnedPlayerUnits.Add(Instantiate(level.PlayerUnits[Random.Range(0, level.PlayerUnits.Count)], navMeshPosition, Quaternion.identity, playerUnitsTransform));
            }
        }
        public void SpawnComputerControlledUnit()
        {
            Vector3 spawnPosition = GenerateSpawnPosition(-5, 5, 25, 50);

            if (GetNavMeshPosition(spawnPosition, out Vector3 navMeshPosition, 1))
            {
                spawnedComputerUnits.Add(Instantiate(level.ComputerUnits[Random.Range(0, level.ComputerUnits.Count)], navMeshPosition, Quaternion.identity, computerUnitsTransform));
            }
        }
        public void TransitionCameraToUnit(UnitController unit, CinemachineVirtualCamera cam)
        {
            Debug.Log("Transition camera to " + unit.name);
            SetCameraPrioritiesToZero();
            cam.Priority = 1;
        }

        public void TransitionCameraToMain()
        {
            SetCameraPrioritiesToZero();
            CameraSystem.Instance.InputCam.Priority = 1;
        }
        private void SetCameraPrioritiesToZero()
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                cameras[i].Priority = 0;
            }
        }
        public void NextRound()
        {
            currentRound++;
            if (CurrentTurnOwner == TurnOwner.Player)
            {
                CurrentTurnOwner = TurnOwner.Computer;
            }
            else if (CurrentTurnOwner == TurnOwner.Computer)
            {
                CurrentTurnOwner = TurnOwner.Player;
            }
            UIManager.Instance.UpdateCurrentTurnOwnerText();
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
