using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvocadoShark
{
    public class Menu : MonoBehaviour
    {
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        public void OpenProVersion()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/slug/264984");
        }
        public void OpenSupport()
        {
            Application.OpenURL("https://discord.gg/mP4yfHxXPa");
        }
        public void OpenAvocadoShark()
        {
            Application.OpenURL("https://avocadoshark.com/");
        }
    }
}
