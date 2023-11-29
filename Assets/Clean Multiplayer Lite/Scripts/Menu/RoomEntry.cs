#if CMLSETUP_COMPLETE
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace AvocadoShark
{
    public class RoomEntry : MonoBehaviour
    {
        public TextMeshProUGUI roomName, playerCount;
        public Button joinButton;
        public CanvasGroup canvasGroup;
        public float lerpSpeed = 0.5f; // Speed for fade in. Adjust as needed.
        public void JoinRoom()
        {
            FusionConnection.instance.loadingScreenScript.gameObject.SetActive(true);
            Invoke("ContinueJoinRoom", FusionConnection.instance.loadingScreenScript.lerpSpeed);
        }
        void ContinueJoinRoom()
        {
            FusionConnection.instance.JoinRoom(roomName.text);
        }
        public void OnDisable()
        {
            joinButton.onClick.RemoveListener(JoinRoom);
        }
    }
}
#endif
