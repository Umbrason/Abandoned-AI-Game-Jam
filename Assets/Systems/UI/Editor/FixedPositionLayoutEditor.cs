using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(FixedPositionLayout))]
public class FixedPositionLayoutEditor : Editor
{
    void OnSceneGUI()
    {
        var fixedPositionLayout = target as FixedPositionLayout;

        for (int i = 0; i < fixedPositionLayout.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            if (!fixedPositionLayout[i].ValidTRS())
                fixedPositionLayout[i] = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            var pos = (Vector3)(fixedPositionLayout[i] * new Vector4(0, 0, 0, 1));
            var rot = fixedPositionLayout[i].rotation;
            pos += fixedPositionLayout.transform.position;
            rot *= fixedPositionLayout.transform.rotation;
            Handles.TransformHandle(ref pos, ref rot);
            pos -= fixedPositionLayout.transform.position;
            rot *= Quaternion.Inverse(fixedPositionLayout.transform.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(fixedPositionLayout, "change matrices");
                fixedPositionLayout[i] = Matrix4x4.TRS(pos, rot, Vector3.one);
                fixedPositionLayout.DoLayout();
                EditorUtility.SetDirty(fixedPositionLayout);
                EditorSceneManager.MarkSceneDirty(fixedPositionLayout.gameObject.scene);
            }
        }


    }
}
