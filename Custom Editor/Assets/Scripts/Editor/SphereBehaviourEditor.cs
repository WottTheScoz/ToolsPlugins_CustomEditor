using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(SphereBehaviour)), CanEditMultipleObjects]
public class SphereBehaviourEditor : Editor
{
    Color enabledColor = Color.green;
    Color disabledColor = Color.red;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var radius = serializedObject.FindProperty("radius");

        EditorGUILayout.PropertyField(radius);

        serializedObject.ApplyModifiedProperties();

        using(new EditorGUILayout.HorizontalScope())
        {
            if(GUILayout.Button("Select all spheres"))
            {
                var allSphereBehaviour = GameObject.FindObjectsOfType<SphereBehaviour>();
                var allSphereGameObjects = allSphereBehaviour
                    .Select(sphere => sphere.gameObject)
                    .ToArray();
                Selection.objects = allSphereGameObjects;
            }

            if(GUILayout.Button("Clear selection"))
            {
                Selection.objects = new Object[]
                {
                    (target as SphereBehaviour).gameObject
                };
            }
        }

        // saves background color
        Color cachedColor = GUI.backgroundColor;

        // changes the next button's color depending on whether spheres are active
        foreach(var sphere in GameObject.FindObjectsOfType<SphereBehaviour>(true))
        {
            if(sphere.gameObject.activeSelf)
            {
                GUI.backgroundColor = disabledColor;
            }
            else
            {
                GUI.backgroundColor = enabledColor;
                break;
            }
        }

        if(GUILayout.Button("Disable/Enable all spheres", GUILayout.Height(40)))
        {
            foreach(var sphere in GameObject.FindObjectsOfType<SphereBehaviour>(true))
            {
                Undo.RecordObject(sphere.gameObject, "Disable/Enable sphere");
                sphere.gameObject.SetActive(!sphere.gameObject.activeSelf);
            }
        }

        // returns GUI to background color
        GUI.backgroundColor = cachedColor;

        if(radius.floatValue < 1)
        {
            EditorGUILayout.HelpBox("Radius cannot be less than 1!", MessageType.Warning);
        }

        if(radius.floatValue > 10)
        {
            EditorGUILayout.HelpBox("Radius cannot be greater than 10!", MessageType.Warning);
        }
    }
}
