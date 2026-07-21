using Cinemachine;
using UnityEngine;

namespace Gamekit3D
{
    public class CameraSettings : MonoBehaviour
    {
        public Transform follow;
        public Transform lookAt;
        public CinemachineVirtualCamera thirdPersonCamera;
        public bool allowRuntimeCameraSettingsChanges;

        void Reset()
        {
            if (thirdPersonCamera == null)
                thirdPersonCamera = GetComponentInChildren<CinemachineVirtualCamera>();

            PlayerController playerController = FindFirstObjectByType<PlayerController>();
            if (playerController != null && playerController.name == "Ellen")
            {
                follow = playerController.transform;

                lookAt = follow.Find("HeadTarget");

                if (playerController.cameraSettings == null)
                    playerController.cameraSettings = this;
            }
        }

        void Awake()
        {
            UpdateCameraSettings();
        }

        void Update()
        {
            if (allowRuntimeCameraSettingsChanges)
            {
                UpdateCameraSettings();
            }
        }

        void UpdateCameraSettings()
        {
            if (thirdPersonCamera == null)
                return;

            thirdPersonCamera.Follow = follow;
            thirdPersonCamera.LookAt = lookAt;
        }
    }
}
