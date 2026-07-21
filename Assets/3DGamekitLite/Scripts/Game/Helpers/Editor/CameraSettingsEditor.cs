using UnityEditor;
using UnityEngine;

namespace Gamekit3D
{
    [CustomEditor(typeof(CameraSettings))]
    public class CameraSettingsEditor : Editor
    {
        SerializedProperty m_ScriptProp;
        SerializedProperty m_FollowProp;
        SerializedProperty m_LookAtProp;
        SerializedProperty m_ThirdPersonCameraProp;
        SerializedProperty m_AllowRuntimeCameraSettingsChangesProp;

        GUIContent m_ScriptContent = new GUIContent("Script");
        GUIContent m_FollowContent = new GUIContent("Follow", "Used to determine how the camera moves.  It should be set to Ellen.");
        GUIContent m_LookAtContent = new GUIContent("Look At", "Used to determine how the camera aims.  It should be set to HeadTarget (this is a child within Ellen's hierarchy).");
        GUIContent m_ThirdPersonCameraContent = new GUIContent("Third Person Camera", "The virtual camera that stays behind the player.");
        GUIContent m_AllowRuntimeCameraSettingsChangesContent = new GUIContent("Allow Runtime Camera Settings Changes", "When checked this makes it possible to change the Camera Settings' fields while the game is playing in order to test out what feels nice.");

        void OnEnable()
        {
            m_ScriptProp = serializedObject.FindProperty("m_Script");
            m_FollowProp = serializedObject.FindProperty("follow");
            m_LookAtProp = serializedObject.FindProperty("lookAt");
            m_ThirdPersonCameraProp = serializedObject.FindProperty("thirdPersonCamera");
            m_AllowRuntimeCameraSettingsChangesProp = serializedObject.FindProperty("allowRuntimeCameraSettingsChanges");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(m_ScriptProp, m_ScriptContent);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(m_FollowProp, m_FollowContent);
            EditorGUILayout.PropertyField(m_LookAtProp, m_LookAtContent);
            EditorGUILayout.PropertyField(m_ThirdPersonCameraProp, m_ThirdPersonCameraContent);
            EditorGUILayout.PropertyField(m_AllowRuntimeCameraSettingsChangesProp, m_AllowRuntimeCameraSettingsChangesContent);

            serializedObject.ApplyModifiedProperties();
        }
    }

}