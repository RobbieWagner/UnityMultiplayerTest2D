#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace AvocadoShark
{
    [InitializeOnLoad]
    public class SetupWizard : EditorWindow
    {
        static SetupWizard()
        {
#if !CMLSETUP_COMPLETE
        EditorApplication.delayCall += OnInitialize;
#endif
        }

        static void OnInitialize()
        {
            ShowWindow();
        }

        private Texture2D headerSectionTexture;
        private ListRequest request;
        private Vector2 scrollPosition;
        bool showTMPHelpBox = false;
        bool showInputSystemHelpBox = false;
        bool iscompleteshowing = false;

        [MenuItem("Tools/CML/Setup Wizard")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SetupWizard));
        }

        private void OnEnable()
        {
            InitTextures();
            request = Client.List();  // List packages currently installed
            EditorApplication.update += Progress;
            Repaint();
            InitTextures();
        }
        private void OnDisable()
        {
            EditorApplication.update -= Progress;
        }
        private void Progress()
        {
            if (request.IsCompleted)
            {
                EditorApplication.update -= Progress;
            }
        }
        void InitTextures()
        {
            headerSectionTexture = Resources.Load("Identity") as Texture2D;
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            // Draw the header
            GUILayout.Label(new GUIContent(headerSectionTexture));
            Repaint();
            GUILayout.Space(20);

            GUIStyle Title = new GUIStyle(GUI.skin.label);
            Title.fontSize = 28;
            Title.fontStyle = FontStyle.Bold;
            Title.wordWrap = true;

            GUIStyle SubTitle = new GUIStyle(GUI.skin.label);
            SubTitle.fontSize = 15;
            SubTitle.wordWrap = true;

            GUIStyle checkStyle = new GUIStyle(GUI.skin.label);
            checkStyle.normal.textColor = Color.green;
            checkStyle.hover.textColor = Color.green;
            checkStyle.fontSize = 13;
            checkStyle.wordWrap = true;

            GUILayout.Label("Welcome to Clean Multiplayer Lite!", Title);
            GUILayout.Label("Thank you for choosing Avocado Shark, your multiplayer journey is about to begin!", SubTitle);
            GUILayout.Space(20);
            GUILayout.Label("Setup", Title);
            GUILayout.Label("There are a few things that need to be done before you can begin using Clean Multiplayer Lite:", SubTitle);
            if (GUILayout.Button("Click here to see the Getting Started tutorial", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://youtu.be/DmYfVZWT2x4");
            }
            if (GUILayout.Button("Need Help? Ask on the Discord server", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://discord.gg/mP4yfHxXPa");
            }
            if (GUILayout.Button("For more, check out Avocado Shark", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://avocadoshark.com/");
            }
            GUILayout.Space(20);

            // Color Space Setup
            EditorGUILayout.Space();
            GUILayout.Label("Color Space Setup", EditorStyles.boldLabel);
            if (PlayerSettings.colorSpace != ColorSpace.Linear)
            {
                if (GUILayout.Button("Switch to Linear Color Space"))
                {
                    PlayerSettings.colorSpace = ColorSpace.Linear;
                }
            }
            else
            {
                GUILayout.Label("Done ✔️", checkStyle);
            }
            EditorGUILayout.Space();

            // Scene Setup
            EditorGUILayout.Space();
            GUILayout.Label("Scene Setup", EditorStyles.boldLabel);

            //Scene paths
            string[] scenePathsToAdd = new string[]
            {
    "Assets/Clean Multiplayer Lite/Scenes/Menu.unity",
    "Assets/Clean Multiplayer Lite/Scenes/Game.unity",
            };

            bool scenesAreInBuild = AreScenesInBuild(new List<string>(scenePathsToAdd));

            if (!scenesAreInBuild)
            {
                if (GUILayout.Button("Add Scenes to Build"))
                {
                    // Get the current list of scenes in build settings
                    List<EditorBuildSettingsScene> currentScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

                    // Add scenes to the build settings (if they aren't there already)
                    foreach (var scenePath in scenePathsToAdd)
                    {
                        if (!currentScenes.Exists(s => s.path == scenePath))
                        {
                            currentScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                        }
                    }

                    // Update the build settings
                    EditorBuildSettings.scenes = currentScenes.ToArray();
                }
            }
            else
            {
                GUILayout.Label("Done ✔️", checkStyle);
            }

            EditorGUILayout.Space();


            bool AreScenesInBuild(List<string> scenePaths)
            {
                List<EditorBuildSettingsScene> currentScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

                // Check if all scenes exist in the current build settings
                foreach (var scenePath in scenePaths)
                {
                    if (!currentScenes.Exists(s => s.path == scenePath))
                    {
                        return false;
                    }
                }

                return true;
            }


            GUILayout.Space(10);

            GUILayout.Label("Package Management", EditorStyles.boldLabel);

            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                {
                    GUI.enabled = !IsFusionInstalled();  // Disable the GUI if Fusion is installed
                    if (GUILayout.Button("Install Fusion"))
                    {
                        Application.OpenURL("https://doc.photonengine.com/fusion/current/getting-started/sdk-download");
                    }
                    GUI.enabled = true;  // Enable the GUI for subsequent controls
                    if (IsFusionInstalled())
                    {
                        GUILayout.Label("Installed ✔️", checkStyle);
                    }

#if ENABLE_INPUT_SYSTEM
                    GUI.enabled = false;
#endif
                    if (GUILayout.Button("Install InputSystem & Enable it"))
                    {
                        bool isInstalled = IsPackageInstalled("com.unity.inputsystem");
                        using (new EditorGUI.DisabledScope(isInstalled))
                        {
                            Client.Add("com.unity.inputsystem");
                        }
                        showInputSystemHelpBox = true;
                    }
                    GUI.enabled = true;
                    if (IsPackageInstalled("com.unity.inputsystem"))
                    {
#if ENABLE_INPUT_SYSTEM
                        GUILayout.Label("Installed and Enabled ✔️", checkStyle);
                        showInputSystemHelpBox = false;
#else
                    showInputSystemHelpBox = true;
                    GUILayout.Label("Installed, but not enabled");
#endif
                    }
                    if (showInputSystemHelpBox)
                    {
                        EditorGUILayout.HelpBox("To enable the input system: Go to 'Edit' -> 'Project Settings' -> 'Player'. Expand the 'Other Settings' section. Locate the 'Active Input Handling' option. Set it to 'Both'. This should restart your editor", MessageType.Info);
                    }
#if ENABLE_INPUT_SYSTEM
                    showInputSystemHelpBox = false;
#endif
                    bool isTMPinstall = IsTextMeshProInstalled();
                    if (isTMPinstall)
                        GUI.enabled = false;
                    if (GUILayout.Button("Install TextMeshPro"))
                    {
                        showTMPHelpBox = true;
                    }
                    if (showTMPHelpBox)
                    {
                        EditorGUILayout.HelpBox("Please go to 'Window -> TextMeshPro -> Import TMP Essential Resources' to install it", MessageType.Info);
                    }
                    GUI.enabled = true;
                    if (isTMPinstall)
                    {
                        GUILayout.Label("Installed ✔️", checkStyle);
                        showTMPHelpBox = false;
                    }
                    AddPackageButton("com.unity.cinemachine", "Cinemachine");
                    AddPackageButton("com.unity.postprocessing", "Post Processing");
                }
                else
                {
                    EditorGUILayout.LabelField("Failed to list packages.");
                }
            }
            else
            {
                EditorGUILayout.LabelField("Loading package list...");
            }
            EditorGUILayout.EndScrollView();

            if (request.IsCompleted)
            {
                bool packagesinstalled = AreAllPackagesInstalled();
                if (packagesinstalled)
                {
                    AddDefineSymbols("CMLSETUP_COMPLETE");
                    if (!iscompleteshowing)
                    {
                        SetupComplete.ShowWindow();
                        iscompleteshowing = true;
                    }
                }
            }
        }
        private void AddPackageButton(string packageId, string displayName)
        {
            bool isInstalled = IsPackageInstalled(packageId);
            using (new EditorGUI.DisabledScope(isInstalled))
            {
                if (GUILayout.Button($"Install {displayName}"))
                {
                    Client.Add(packageId);
                }
            }

            if (isInstalled)
            {
                GUIStyle checkStyle = new GUIStyle(GUI.skin.label);
                checkStyle.normal.textColor = Color.green;
                checkStyle.hover.textColor = Color.green;
                checkStyle.fontSize = 13;
                checkStyle.wordWrap = true;
                GUILayout.Label("Installed ✔️", checkStyle);
            }
        }
        private void AddDefineSymbols(string defineSymbol)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            if (!allDefines.Contains(defineSymbol))
            {
                allDefines.Add(defineSymbol);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup,
                    string.Join(";", allDefines.ToArray())
                );
            }
        }
        bool IsFusionInstalled()
        {
#if FUSION_WEAVER
            return true;
#else
        return false;
#endif
        }
        bool IsTextMeshProInstalled()
        {
            return Directory.Exists("Assets/TextMesh Pro");
        }
        private bool IsPackageInstalled(string packageId)
        {
            foreach (var package in request.Result)
            {
                if (package.packageId.StartsWith(packageId))
                {
                    return true;
                }
            }
            return false;
        }
        bool AreAllPackagesInstalled()
        {
            string[] packageIds = new string[]
            {
        "com.unity.cinemachine",
        "com.unity.inputsystem",
        "com.unity.textmeshpro",
        "com.unity.postprocessing"
            };

            foreach (string packageId in packageIds)
            {
                if (!IsPackageInstalled(packageId))
                {
                    return false; // At least one package isn't installed, so return false
                }
            }

            return true; // If it got through the loop without returning, all packages are installed
        }
    }
}
#endif
