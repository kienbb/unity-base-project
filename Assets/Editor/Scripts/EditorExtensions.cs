using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorExtensions
{
    public static List<T> GetAllAssetsAtFolder<T>(this string pathToFolder) where T : UnityEngine.Object
    {
        // Get all asset paths in the specified folder
        string[] assetPaths = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { pathToFolder });

        // Create a list to hold the assets
        List<T> assets = new List<T>();

        // Loop through each asset path and load the asset
        foreach (string assetPath in assetPaths)
        {
            string fullPath = AssetDatabase.GUIDToAssetPath(assetPath);
            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }

        // Return the array of assets
        return assets;
    }
}
