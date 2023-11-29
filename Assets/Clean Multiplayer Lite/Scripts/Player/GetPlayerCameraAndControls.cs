#if CMLSETUP_COMPLETE
using Cinemachine;
using UnityEngine;
using Fusion;
using StarterAssets;

namespace AvocadoShark
{
    public class GetPlayerCameraAndControls : MonoBehaviour
    {
        [SerializeField] Transform playerCameraRoot;
        [SerializeField] Transform PlayerModel;
        [SerializeField] Transform InterpolationPoint;

        private void Start()
        {
            NetworkObject thisObject = GetComponent<NetworkObject>();

            if (thisObject.HasStateAuthority)
            {
                GameObject virtualCamera = GameObject.Find("Player Follow Camera");
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;

                GetComponent<ThirdPersonController>().enabled = true;
            }
            else
            {
                PlayerModel.SetParent(InterpolationPoint);
            }
        }
    }
}
#endif
