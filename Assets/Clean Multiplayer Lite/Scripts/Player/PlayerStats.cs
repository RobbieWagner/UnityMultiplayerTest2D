#if CMLSETUP_COMPLETE
using UnityEngine;
using Fusion;
using TMPro;
using System;
using UnityEngine.UI;

namespace AvocadoShark
{
    public class PlayerStats : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }

        [SerializeField] TextMeshPro playerNameLabel;
        public static PlayerStats instance;

        private void Start()
        {
            if (HasStateAuthority)
            {
                PlayerName = FusionConnection.instance._playerName;
                if (instance == null) { instance = this; }
            }

        }

        protected static void UpdatePlayerName(Changed<PlayerStats> changed)
        {
            if (!changed.Behaviour.HasStateAuthority)
                changed.Behaviour.playerNameLabel.text = changed.Behaviour.PlayerName.ToString();
            else
                changed.Behaviour.playerNameLabel.text = "";
        }
    }
}
#endif
