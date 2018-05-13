using System;
using UnityEditor;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [CustomEditor(typeof(RadialEdgeDetection))]
    class CustomEditorTestEditor : Editor
    {
        SerializedObject serObj;        
        SerializedProperty sensitivityDepth;
        SerializedProperty sensitivityNormals;
        SerializedProperty edgesOnly;
        SerializedProperty edgesOnlyBgColor;
        SerializedProperty mainColor;
        SerializedProperty sampleDist;
        SerializedProperty timeStep;
        SerializedProperty width;
        SerializedProperty distance;

        void OnEnable()
        {
            serObj = new SerializedObject(target);
            sensitivityDepth = serObj.FindProperty("sensitivityDepth");
            sensitivityNormals = serObj.FindProperty("sensitivityNormals");

            edgesOnly = serObj.FindProperty("edgesOnly");
            edgesOnlyBgColor = serObj.FindProperty("edgesOnlyBgColor");
            mainColor = serObj.FindProperty("mainColor");
            sampleDist = serObj.FindProperty("sampleDist");
            timeStep = serObj.FindProperty("timeStep");
            width = serObj.FindProperty("width");
            distance = serObj.FindProperty("distance");
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            GUILayout.Label("Edge Detection using Roberts Cross Edge Detection", EditorStyles.miniBoldLabel);

            EditorGUILayout.PropertyField(sampleDist, new GUIContent(" Sample Distance"));
            EditorGUILayout.PropertyField(sensitivityDepth, new GUIContent(" Depth Sensitivity"));
            EditorGUILayout.PropertyField(sensitivityNormals, new GUIContent(" Normals Sensitivity"));            
            EditorGUILayout.Separator();
            GUILayout.Label("Background Options", EditorStyles.miniBoldLabel);
            edgesOnly.floatValue = EditorGUILayout.Slider("Non Edge Fade", edgesOnly.floatValue, 0.0f, 1.0f);
            EditorGUILayout.PropertyField(edgesOnlyBgColor, new GUIContent("Edge Color"));
            EditorGUILayout.PropertyField(mainColor, new GUIContent("Main Color"));
            timeStep.floatValue = EditorGUILayout.Slider("Time Step", timeStep.floatValue, 0.0f, 25.0f);
            width.floatValue = EditorGUILayout.Slider("Bar Width", width.floatValue, 0.0f, 100.0f);
            EditorGUILayout.PropertyField(distance, new GUIContent("distance"));
            serObj.ApplyModifiedProperties();
        }
    }
}