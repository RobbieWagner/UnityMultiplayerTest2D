#if CMLSETUP_COMPLETE
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

namespace AvocadoShark
{
    public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static FusionConnection instance;
        private NetworkRunner runner;

        [Header("Player")]
        [SerializeField] public GameObject playerPrefab;

        [Header("Name Entry")]
        public GameObject mainObject;
        public Button submitButton;
        public TMP_InputField nameField;

        [Header("Room List")]
        public GameObject roomEntryPrefab;
        public GameObject roomListObject;
        public Transform content;
        public Button createRoomButton;
        public TextMeshProUGUI NoRoomsText;

        [Header("Room List Refresh (s)")]
        [SerializeField] private float refreshInterval = 1f;

        [Header("Player Spawn Location")]
        [SerializeField] private bool UseCustomLocation;
        [SerializeField] private Vector3 CustomLocation;

        [Header("Loading Screen")]
        public LoadingScreen loadingScreenScript;

        private bool initialRoomListPopulated = false;
        private List<SessionInfo> _sessionList = new List<SessionInfo>();

        [HideInInspector] public string _playerName = null;
        
        [SerializeField] private PlayerCharacterSetupController playerCharacterSetupController;

        private void Awake()
        {
            if (instance == null) { instance = this; }
#if UNITY_2022_3_OR_NEWER
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#else
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
#endif
        }

        private void RefreshRoomList()
        {
            InitialRoomListSetup();
        }

        private IEnumerator AutoRefreshRoomList()
        {
            while (true)
            {
                RefreshRoomList();
                yield return new WaitForSeconds(refreshInterval);
            }
        }

        public void CreateRoom()
        {
            loadingScreenScript.gameObject.SetActive(true);
            Invoke("ContinueCreateRoom", loadingScreenScript.lerpSpeed);
        }
        void ContinueCreateRoom()
        {
            int randomInt = UnityEngine.Random.Range(1000, 9999);
            string sessionName = "Room-" + randomInt;

            JoinRoom(sessionName);

            StopCoroutine(AutoRefreshRoomList());
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            if (initialRoomListPopulated == false)
            {
                StartCoroutine(AutoRefreshRoomList());
                loadingScreenScript.FadeOutAndDisable();
            }

            _sessionList = sessionList;
        }

        private void InitialRoomListSetup()
        {
            if (roomListObject == null)
                return;
            initialRoomListPopulated = true;
            roomListObject.SetActive(true);

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (SessionInfo session in _sessionList)
            {
                GameObject entry = GameObject.Instantiate(roomEntryPrefab, content);
                entry.transform.localScale = Vector3.one;

                RoomEntry entryScript = entry.GetComponent<RoomEntry>();
                entryScript.roomName.text = session.Name;
                entryScript.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;
                entryScript.joinButton.interactable = session.IsOpen;
            }

            if (_sessionList.Count == 0)
            {
                NoRoomsText.gameObject.SetActive(true);
            }
            else
            {
                NoRoomsText.gameObject.SetActive(false);
            }

        }

        public void ConnectToRunner()
        {
            loadingScreenScript.gameObject.SetActive(true);
            Invoke("ContinueConnectToRunner", loadingScreenScript.lerpSpeed);
        }
        void ContinueConnectToRunner()
        {
            _playerName = nameField.text;
            mainObject.SetActive(false);

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            runner.JoinSessionLobby(SessionLobby.Shared);
        }
        public async void JoinRoom(string sessionName)
        {
            int buildIndex = -1;

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                if (sceneName == "Game")
                {
                    buildIndex = i;
                    break;
                }
            }
            StopCoroutine(AutoRefreshRoomList());

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = sessionName,
                Scene = buildIndex
            });
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("OnConnectedToServer");
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {

        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {

        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {

        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {

        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {

        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {

        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {

        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("OnPlayerJoined");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {

        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {

        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log("Scene Load Done.");

            if (runner.GetPlayerObject(runner.LocalPlayer) == null)
            {
                NetworkObject playerObject;
                if (!UseCustomLocation)
                {
                    playerObject = runner.Spawn(playerPrefab, new Vector3(UnityEngine.Random.Range(-7.6f, 14.2f), 0, UnityEngine.Random.Range(-31.48f, -41.22f)));
                    runner.SetPlayerObject(runner.LocalPlayer, playerObject);
                }
                else
                {
                    playerObject = runner.Spawn(playerPrefab, CustomLocation);
                    runner.SetPlayerObject(runner.LocalPlayer, playerObject);
                } 
                
                playerCharacterSetupController?.SetupCharacter(playerObject);
            }
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {

        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {

        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {

        }
        public void Checkforname()
        {
            if (nameField.text != "")
                submitButton.interactable = true;
            else
                submitButton.interactable = false;
        }
    }
}
#endif
