using _Scripts.Enemy.Management;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts.GameFlow.Transition
{
    public class CameraPanHandler : MonoBehaviour
    {
        public static CameraPanHandler instance;
        
        [SerializeField] private CameraController controller;
        [SerializeField] private Transform camRig;
        [SerializeField] private Transform cam;
        [SerializeField] private VisualEffect lowerLeftPortal;
        [SerializeField] private VisualEffect lowerRightPortal;
        [SerializeField] private VisualEffect[] middleLeftPortal;
        [SerializeField] private VisualEffect[] middleRightPortal;
        [SerializeField] private VisualEffect[] upperLeftPortal;
        [SerializeField] private VisualEffect[] upperRightPortal;
        [SerializeField] private Vector3[] portalViewPositions;
        [SerializeField] private Quaternion[] portalViewRotations;
        [SerializeField] private Vector3[] portalViewHeights;
        [SerializeField] private Quaternion cameraAngle;
        private Vector3 currentCameraPosition;
        private Quaternion currentCameraRotation;
        private Vector3 currentCameraHeight;
        
        private bool isActive;
        private bool viewedRightPortal;
        private bool restoredCamera;
        private int currentStage;
        private float timer;
        
        void Awake()
        {
            instance = this;
        }

        void Update()
        {
            if (isActive)
            {
                if (timer >= 5f && !viewedRightPortal)
                {
                    ViewRightPortal();
                }
                if (timer >= 10f && !restoredCamera)
                {
                    RestoreCameraPosition();
                    PlayerManager.instance.canGatherResources = true;
                }
                if (timer >= 12f)
                {
                    StartStage();
                }
                
                timer += Time.deltaTime;
            }
        }

        private void StartStage()
        {
            isActive = false;
            EnemySpawnManager.instance.previousPeriodTick = 0;
            EnemySpawnManager.instance.stageIsActive = true;
        }

        public void PanCameraToPortal(int stageIndex)
        {
            controller.enabled = false;
            SaveCameraPosition();
            currentStage = stageIndex;
            isActive = true;
            timer = 0f;
            restoredCamera = false;
            viewedRightPortal = false;
            ViewLeftPortal();
        }

        private void ViewLeftPortal()
        {
            switch (currentStage)
            {
                case 0:
                    camRig.SetPositionAndRotation(portalViewPositions[0], portalViewRotations[0]);
                    cam.SetLocalPositionAndRotation(portalViewHeights[0], cameraAngle);
                    lowerLeftPortal.enabled = true;
                    break;
                case 1:
                    camRig.SetPositionAndRotation(portalViewPositions[2], portalViewRotations[2]);
                    cam.SetLocalPositionAndRotation(portalViewHeights[2], cameraAngle);
                    foreach (VisualEffect portal in middleLeftPortal)
                    {
                        portal.enabled = true;
                    }
                    break;
                case 2:
                    camRig.SetPositionAndRotation(portalViewPositions[4], portalViewRotations[4]);
                    cam.SetLocalPositionAndRotation(portalViewHeights[4], cameraAngle);
                    foreach (VisualEffect portal in upperLeftPortal)
                    {
                        portal.enabled = true;
                    }
                    break;
            }
        }

        private void ViewRightPortal()
        {
            viewedRightPortal = true;
            
            switch (currentStage)
            {
                case 0:
                    camRig.SetPositionAndRotation(portalViewPositions[1], portalViewRotations[1]);
                    cam.SetLocalPositionAndRotation(portalViewHeights[1], cameraAngle);
                    lowerRightPortal.enabled = true;
                    break;
                case 1:
                    camRig.SetPositionAndRotation(portalViewPositions[3], portalViewRotations[3]);
                    cam.SetLocalPositionAndRotation(portalViewHeights[3], cameraAngle);
                    foreach (VisualEffect portal in middleRightPortal)
                    {
                        portal.enabled = true;
                    }
                    break;
                case 2:
                    camRig.SetPositionAndRotation(portalViewPositions[5], portalViewRotations[5]);
                    cam.SetLocalPositionAndRotation(portalViewHeights[5], cameraAngle);
                    foreach (VisualEffect portal in upperRightPortal)
                    {
                        portal.enabled = true;
                    }
                    break;
            }
        }
        
        private void SaveCameraPosition()
        {
            currentCameraHeight = cam.position;
            currentCameraRotation = camRig.rotation;
            currentCameraPosition = camRig.position;
        }
        
        private void RestoreCameraPosition()
        {
            camRig.SetPositionAndRotation(currentCameraPosition, currentCameraRotation);
            cam.SetLocalPositionAndRotation(currentCameraHeight, cameraAngle);
            controller.enabled = true;
            restoredCamera = true;
        }
    }
}