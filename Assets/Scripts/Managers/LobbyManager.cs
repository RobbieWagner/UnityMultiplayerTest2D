using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AvocadoShark;
using System.Linq;

public class LobbyManager : MonoBehaviour
{
    //keep track of players waiting, players ready, and compares with players in game
    private HashSet<PlayerStats> playersReady;

    public static LobbyManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        }

        playersReady = new HashSet<PlayerStats>();
    }

    public void AddReadyPlayer(PlayerStats readyPlayer)
    {
        playersReady.Add(readyPlayer);
        OnAddReadyPlayer(readyPlayer);
    }

    public delegate void OnAddReadyPlayerDelegate(PlayerStats playerStats);
    public event OnAddReadyPlayerDelegate OnAddReadyPlayer;
}
