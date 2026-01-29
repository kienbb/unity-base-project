using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenScene
{
    [MenuItem("Scene/0. Main Game", priority = 0)]
    static void OpenSceneMainGame()
    {
        //find the scene "MainGame" in AssetDatabase and load it
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        foreach (string scenePath in scenePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(scenePath);
            if (path.EndsWith("MainGame.unity"))
            {
                //open the scene
                EditorSceneManager.OpenScene(path);
                Debug.Log("Opened MainGame scene: " + path);
                return;
            }
        }
    }
}
