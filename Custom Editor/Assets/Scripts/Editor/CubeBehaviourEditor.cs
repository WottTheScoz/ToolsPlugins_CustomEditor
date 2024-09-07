using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CubeBehaviour)), CanEditMultipleObjects]
public class CubeBehaviourEditor : Editor
{
    Color enabledColor = Color.green;
    Color disabledColor = Color.red;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var size = serializedObject.FindProperty("size");

        EditorGUILayout.PropertyField(size);

        serializedObject.ApplyModifiedProperties();

        using(new EditorGUILayout.HorizontalScope())
        {
            if(GUILayout.Button("Select all cubes"))
            {
                var allCubeBehaviour = GameObject.FindObjectsOfType<CubeBehaviour>();
                var allCubeGameObjects = allCubeBehaviour
                    .Select(cube => cube.gameObject)
                    .ToArray();
                Selection.objects = allCubeGameObjects;
            }

            if(GUILayout.Button("Clear selection"))
            {
                Selection.objects = new Object[]
                {
                    (target as CubeBehaviour).gameObject
                };
            }
        }

        // saves background color
        Color cachedColor = GUI.backgroundColor;

        // changes the next button's color depending on whether cubes are active
        foreach(var cube in GameObject.FindObjectsOfType<CubeBehaviour>(true))
        {
            if(cube.gameObject.activeSelf)
            {
                GUI.backgroundColor = disabledColor;
            }
            else
            {
                GUI.backgroundColor = enabledColor;
                break;
            }
        }

        if(GUILayout.Button("Disable/Enable all cubes", GUILayout.Height(40)))
        {
            foreach(var cube in GameObject.FindObjectsOfType<CubeBehaviour>(true))
            {
                Undo.RecordObject(cube.gameObject, "Disable/Enable cube");
                cube.gameObject.SetActive(!cube.gameObject.activeSelf);
            }
        }

        // returns GUI to background color
        GUI.backgroundColor = cachedColor;

        if(size.floatValue < 1)
        {
            EditorGUILayout.HelpBox("Size cannot be less than 1!", MessageType.Warning);
        }
    }
}
