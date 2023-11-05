using Cinemachine;
using UnityEngine;

namespace CodeMonkey.CameraSystem
{

    public class CameraSystem : MonoBehaviour {

        public CinemachineVirtualCamera InputCam;
        [SerializeField] private bool useEdgeScrolling = false;
        [SerializeField] private bool useDragPan = false;
        [SerializeField] private float cameraBaseMoveSpeed = 50f;
        [SerializeField] private float currentCameraBaseMoveSpeed = 50f;
        [SerializeField] private float cameraBaseRotateSpeed = 100f;
        [SerializeField] private float currentCameraRotateSpeed = 100f;
        [SerializeField] private float zoomSpeed = 10f;
        private enum zoomType { FOV, MoveForward, LowerY }
        [SerializeField]
        private zoomType zoom;
        [SerializeField] private float fieldOfViewMax = 50;
        [SerializeField] private float fieldOfViewMin = 10;
        [SerializeField] private float followOffsetMin = 5f;
        [SerializeField] private float followOffsetMax = 50f;
        [SerializeField] private float followOffsetMinY = 10f;
        [SerializeField] private float followOffsetMaxY = 50f;

        private bool dragPanMoveActive;
        private Vector2 lastMousePosition;
        private float targetFieldOfView = 50;
        private Vector3 followOffset;

        #region Singleton
        public static CameraSystem Instance { get; private set; }

        private void Awake() 
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
            #endregion
            followOffset = InputCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }

        private void Update() 
        {
            HandleCameraMovement();
            if (useEdgeScrolling) {
                HandleCameraMovementEdgeScrolling();
            }

            if (useDragPan) {
                HandleCameraMovementDragPan();
            }

            HandleCameraRotation();

            switch (zoom)
            {
                case zoomType.FOV:
                    HandleCameraZoom_FieldOfView();
                    break;
                case zoomType.MoveForward:
                    HandleCameraZoom_MoveForward();
                    break;
                case zoomType.LowerY:
                    HandleCameraZoom_LowerY();
                    break;
                default:
                    break;
            }
            
        }

        private void HandleCameraMovement() 
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            currentCameraBaseMoveSpeed = (followOffset.y / cameraBaseMoveSpeed) * 100;

            transform.position += moveDir * currentCameraBaseMoveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementEdgeScrolling() 
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            int edgeScrollSize = 20;

            if (Input.mousePosition.x < edgeScrollSize) {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.y < edgeScrollSize) {
                inputDir.z = -1f;
            }
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) {
                inputDir.x = +1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) {
                inputDir.z = +1f;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            transform.position += moveDir * cameraBaseMoveSpeed * Time.deltaTime;
        }

        private void HandleCameraMovementDragPan() 
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetMouseButtonDown(1)) {
                dragPanMoveActive = true;
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1)) {
                dragPanMoveActive = false;
            }

            if (dragPanMoveActive) {
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

                float dragPanSpeed = 1f;
                inputDir.x = mouseMovementDelta.x * dragPanSpeed;
                inputDir.z = mouseMovementDelta.y * dragPanSpeed;

                lastMousePosition = Input.mousePosition;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            transform.position += moveDir * cameraBaseMoveSpeed * Time.deltaTime;
        }

        private void HandleCameraRotation() 
        {
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
            if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

            currentCameraRotateSpeed = (followOffset.y / cameraBaseRotateSpeed) * 1000;

            transform.eulerAngles += new Vector3(0, rotateDir * currentCameraRotateSpeed * Time.deltaTime, 0);
        }

        private void HandleCameraZoom_FieldOfView() 
        {
            if (Input.mouseScrollDelta.y > 0) {
                targetFieldOfView -= 5;
            }
            if (Input.mouseScrollDelta.y < 0) {
                targetFieldOfView += 5;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

            InputCam.m_Lens.FieldOfView =
                Mathf.Lerp(InputCam.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraZoom_MoveForward() 
        {
            Vector3 zoomDir = followOffset.normalized;

            float zoomAmount = 3f;
            if (Input.mouseScrollDelta.y > 0) {
                followOffset -= zoomDir * zoomAmount;
            }
            if (Input.mouseScrollDelta.y < 0) {
                followOffset += zoomDir * zoomAmount;
            }

            if (followOffset.magnitude < followOffsetMin) {
                followOffset = zoomDir * followOffsetMin;
            }

            if (followOffset.magnitude > followOffsetMax) {
                followOffset = zoomDir * followOffsetMax;
            }

            InputCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(InputCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraZoom_LowerY() 
        {
            float zoomAmount = 3f;
            if (Input.mouseScrollDelta.y > 0) {
                followOffset.y -= zoomAmount;
            }
            if (Input.mouseScrollDelta.y < 0) {
                followOffset.y += zoomAmount;
            }

            followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);

            InputCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
                Vector3.Lerp(InputCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

        }

    }

}