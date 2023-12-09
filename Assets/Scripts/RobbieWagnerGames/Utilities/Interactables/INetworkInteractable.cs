using System.Collections;
using System.Collections.Generic;
using AvocadoShark;
using UnityEngine;

public class INetworkInteractable : MonoBehaviour
{
    public virtual void Interact(PlayerStats player)
    {
        Debug.Log(player.PlayerName);
    }
}
