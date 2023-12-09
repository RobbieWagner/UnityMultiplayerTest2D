using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AvocadoShark;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playersInLobby;

    private void Awake()
    {
        GameManager.Instance.OnPlayerJoinGame += UpdatePlayerCountText;
        UpdatePlayerCountText();
    }

    private void UpdatePlayerCountText(PlayerStats playerStats = null)
    {
        playersInLobby.text = GameManager.Instance.playersInGame.Count + "/" + GameManager.Instance.playerLimit;
    }
}
