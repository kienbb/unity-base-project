using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.Serialization;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

namespace GameGum.Editor
{
    public class GameGumEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Game Gum/Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<GameGumEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                //{ "Home",                           this,                           EditorIcons.House                       }, // Draws the this.someData field in this case.
                //{ "JX GenZ", JXGenZTool.Instance, EditorIcons.Pen },
                { "Kien", KienEditorTool.Instance, EditorIcons.StarPointer },
                //{ "Art Tool", ArtTool.Instance, EditorIcons.Bell }
            };

            return tree;
        }
    }
}
