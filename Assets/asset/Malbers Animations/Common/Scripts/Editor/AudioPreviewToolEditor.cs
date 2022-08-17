using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioPreviewTool))]
[CanEditMultipleObjects]
public class AudioPreviewToolEditor : Editor
{
    SerializedProperty propSources;

    void OnEnable()
    {
        propSources = serializedObject.FindProperty("sources");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tool = target as AudioPreviewTool;
        serializedObject.Update();

        if (GUILayout.Button("Play"))
        {
            Debug.Log($"{propSources.arraySize}");
            for (int i = 0; i < propSources.arraySize; i++)
            {
                var element = propSources.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue != null)
                {
                    var audioSource = (AudioSource)element.objectReferenceValue;
                    audioSource.Play();
                }
            }
        }

        if (GUILayout.Button("Stop"))
        {
            Debug.Log($"{propSources.arraySize}");
            for (int i = 0; i < propSources.arraySize; i++)
            {
                var element = propSources.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue != null)
                {
                    var audioSource = (AudioSource)element.objectReferenceValue;
                    audioSource.Stop();
                }
            }
        }
    }
}
