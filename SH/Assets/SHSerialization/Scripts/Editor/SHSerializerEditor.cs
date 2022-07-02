using UnityEditor;
using UnityEngine;

namespace SH.Editor
{
    [CustomEditor(typeof(SHSerializer))]
    public class SHSerializerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Serialize"))
            {
                var serializer = (SHSerializer) target;
                Undo.RecordObject(serializer, "Record SH");
                serializer.Serialize();
                EditorUtility.SetDirty(serializer);
            }
        }
    }
}