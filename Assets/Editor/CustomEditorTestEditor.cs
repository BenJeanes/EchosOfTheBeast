using System;
using UnityEditor;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [CustomEditor(typeof(CustomEditorTest))]
    class CustomEditorTestEditor : Editor
    {
        SerializedObject serObj;

        SerializedProperty mode;
        SerializedProperty sensitivityDepth;
        SerializedProperty sensitivityNormals;

        SerializedProperty lumThreshold;

        SerializedProperty edgesOnly;
        SerializedProperty edgesOnlyBgColor;
        SerializedProperty mainColor;
        SerializedProperty edgeExp;
        SerializedProperty sampleDist;
        SerializedProperty timeStep;
        SerializedProperty width;
        SerializedProperty distance;

        void OnEnable()
        {
            serObj = new SerializedObject(target);
            mode = serObj.FindProperty("mode");
            sensitivityDepth = serObj.FindProperty("sensitivityDepth");
            sensitivityNormals = serObj.FindProperty("sensitivityNormals");

            lumThreshold = serObj.FindProperty("lumThreshold");

            edgesOnly = serObj.FindProperty("edgesOnly");
            edgesOnlyBgColor = serObj.FindProperty("edgesOnlyBgColor");
            mainColor = serObj.FindProperty("mainColor");
            edgeExp = serObj.FindProperty("edgeExp");
            sampleDist = serObj.FindProperty("sampleDist");
            timeStep = serObj.FindProperty("timeStep");
            width = serObj.FindProperty("width");
            distance = serObj.FindProperty("distance");
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            GUILayout.Label("Detects spatial differences and converts into black outlines", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(mode, new GUIContent("Mode"));

            if (mode.intValue < 2)
            {
                EditorGUILayout.PropertyField(sensitivityDepth, new GUIContent(" Depth Sensitivity"));
                EditorGUILayout.PropertyField(sensitivityNormals, new GUIContent(" Normals Sensitivity"));
            }
            else if (mode.intValue < 4)
            {
                EditorGUILayout.PropertyField(edgeExp, new GUIContent(" Edge Exponent"));
            }
            else
            {
                // lum based mode
                EditorGUILayout.PropertyField(lumThreshold, new GUIContent(" Luminance Threshold"));
            }

            EditorGUILayout.PropertyField(sampleDist, new GUIContent(" Sample Distance"));

            EditorGUILayout.Separator();

            GUILayout.Label("Background Options");
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


