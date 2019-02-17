using UnityEditor;
using UnityEngine;

public class ReplaceWithPrefab : EditorWindow
{
    [SerializeField] private GameObject prefab;
    private bool toggleScale = false;
    private bool toggleYZ = false;

    private Vector3 positionOffset;
    private Vector3 rotationOffset;
    [MenuItem("Tools/Replace With Prefab")]
    private static void CreateReplaceWithPrefab()
    {
        EditorWindow.GetWindow<ReplaceWithPrefab>();
    }

    private void OnGUI()
    {
        // Prefab that will replaced selected.
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        // Offsets for prefabs build with different ZY
        positionOffset = EditorGUILayout.Vector3Field("Position Offset:", positionOffset);
        rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset:", rotationOffset);
        // Keep original prefab scale.
        toggleScale = GUILayout.Toggle(toggleScale, "Keep prefab scale.");
        // Switch Y/Z axis
        toggleYZ = GUILayout.Toggle(toggleYZ, "Switch Y/Z axis");

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var prefabType = PrefabUtility.GetPrefabType(prefab);
                GameObject newObject;

                if (prefabType == PrefabType.Prefab)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                    newObject.name = prefab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition + positionOffset;
                newObject.transform.localRotation = selected.transform.localRotation * Quaternion.Euler(rotationOffset);
                if (!toggleScale)
                {
                    if (toggleYZ)
                    {
                        newObject.transform.localScale = new Vector3(selected.transform.localScale.x, selected.transform.localScale.z, selected.transform.localScale.y);
                    }
                    else
                    {
                        newObject.transform.localScale = selected.transform.localScale;
                    }
                }
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }

        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }
}