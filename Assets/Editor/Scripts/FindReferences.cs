using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

namespace Sabo.Editor
{
    public class FindReferences
    {
        [MenuItem("CONTEXT/Component/Find All Referencing This And Log")]
        static void FindAllReferencingThisAndLog(MenuCommand command)
        {
            Component targetComponent = command.context as Component;
            if (targetComponent == null)
            {
                Debug.LogWarning("No component selected.");
                return;
            }

            Debug.Log($"Find references to {targetComponent.GetType().Name} on {targetComponent.gameObject.name}", targetComponent);

            Component[] allComponents = Object.FindObjectsOfType<Component>(true);

            bool foundReference = false;
            foreach (Component comp in allComponents)
            {
                if (comp == targetComponent) continue; // Skip the target itself

                SerializedObject so = new SerializedObject(comp);
                SerializedProperty prop = so.GetIterator();

                bool refComp, refGO;
                while (prop.NextVisible(true))
                {
                    refComp = prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue == targetComponent;
                    refGO = prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue == targetComponent.gameObject;
                    if (refComp || refGO)
                    {
                        foundReference = true;

                        //log field information
                        string fieldName = prop.propertyPath;
                        string refGOText = refGO ? "GameObject: " : "";
                        if (!string.IsNullOrEmpty(fieldName))
                        {
                            Debug.Log($"\t{refGOText} <color=yellow>{fieldName}</color> on <color=cyan>{comp.GetType().Name}</color> in <color=gray>{comp.gameObject.name}</color>", comp);
                        }
                    }
                }
            }
            if(!foundReference)
            {
                Debug.Log("No components found that reference the target component.");
            }
        }

        private static string GetGameObjectPath(GameObject obj)
        {
            string path = obj.name;
            Transform parent = obj.transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }
}

