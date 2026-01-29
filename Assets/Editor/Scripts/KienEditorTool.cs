using Sirenix.OdinInspector;
using UnityEngine;

namespace GameGum.Editor
{
    [CreateAssetMenu(fileName = "KienEditorTool", menuName = "Scriptable Objects/KienEditorTool")]
    public class KienEditorTool : ScriptableObject
    {
        private static KienEditorTool _instance;
        public static KienEditorTool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<KienEditorTool>("ScriptableObjects/Editor/KienEditorTool");
                    if (_instance == null)
                    {
                        Debug.LogError("KienEditorTool asset not found in Resources folder.");
                    }
                }
                return _instance;
            }
        }
        [DisplayAsString]
        [HideLabel]
        public string HelloString = "Hello from KienEditorTool!";
    }
}
