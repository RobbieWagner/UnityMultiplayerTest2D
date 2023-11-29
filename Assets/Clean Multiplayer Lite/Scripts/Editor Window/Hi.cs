#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace AvocadoShark
{
    public class Hi : EditorWindow
    {
        private Texture2D headerSectionTexture;
        private Vector2 scrollPosition;
        bool ifshowreview = false;
        bool ifopenpro = false;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (SessionState.GetBool("CML_ShowedWindow", false) == false)
            {
                if (EditorPrefs.GetBool("CML_NeverShowAgain", false) == false)
                {
                    int startCount = EditorPrefs.GetInt("CML_StartCount", 0);

                    if (startCount == 3)
                    {
                        EditorApplication.delayCall += ShowWindow;
                    }

                    EditorPrefs.SetInt("CML_StartCount", ++startCount);

                    SessionState.SetBool("CML_ShowedWindow", true);
                }
            }
        }

        [MenuItem("Tools/CML/Review Asset")]
        public static void ShowWindow()
        {
            EditorApplication.delayCall -= ShowWindow;
            GetWindow(typeof(Hi));
        }
        private void OnEnable()
        {
            InitTextures();
            Repaint();
        }
        void InitTextures()
        {
            headerSectionTexture = Resources.Load("Green Check") as Texture2D;
        }
        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            GUIStyle Title = new GUIStyle(GUI.skin.label);
            Title.fontSize = 28;
            Title.fontStyle = FontStyle.Bold;
            Title.wordWrap = true;

            GUIStyle SubTitle = new GUIStyle(GUI.skin.label);
            SubTitle.fontSize = 15;
            SubTitle.wordWrap = true;

            int hour = DateTime.Now.Hour;

            if (hour >= 0 && hour < 12)
            {
                GUILayout.Label("Good Morning!", Title);
            }
            else if (hour >= 12 && hour < 17)
            {
                GUILayout.Label("Good Afternoon!", Title);
            }
            else
            {
                GUILayout.Label("Good Evening!", Title);
            }

            GUILayout.Space(20);

            GUILayout.Label("Thank you for using Clean Multiplayer Lite! If you enjoy the asset, please consider leaving a review:", SubTitle);
            if (GUILayout.Button("Review Asset"))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/slug/190640");
                ifshowreview = true;
            }
            if (ifshowreview)
            {
                EditorGUILayout.HelpBox("Thank you!", MessageType.Info);
            }

            GUILayout.Space(20);

            GUILayout.Label("Also, if you need more features, check out the pro version:", SubTitle);
            if (GUILayout.Button("Clean Multiplayer Pro"))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/slug/264984");
                ifopenpro = true;
            }
            if (ifopenpro)
            {
                EditorGUILayout.HelpBox("See you on the Pro side!", MessageType.Info);
            }

            GUILayout.Space(40);
            if (GUILayout.Button("Later"))
            {
                EditorPrefs.SetInt("CML_StartCount", 0);
                SessionState.SetBool("CML_ShowedWindow", false);
                EditorPrefs.SetBool("CML_NeverShowAgain", false);
                this.Close();
            }
            if (GUILayout.Button("Don't show again"))
            {
                EditorPrefs.SetBool("CML_NeverShowAgain", true);
                this.Close();
            }
            if (GUILayout.Button("If at any time you need help, feel free to ask on the Discord server", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://discord.gg/mP4yfHxXPa");
            }
            if (GUILayout.Button("For more, check out Avocado Shark", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://avocadoshark.com/");
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif
