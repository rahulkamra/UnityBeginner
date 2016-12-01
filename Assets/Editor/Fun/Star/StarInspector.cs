using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty Points =  serializedObject.FindProperty("Points");
        SerializedProperty Frequency = serializedObject.FindProperty("Frequency");

        int numPoints = Points.arraySize* Frequency.intValue;
        if (!serializedObject.isEditingMultipleObjects)
        {
            if (numPoints < 3)
            {
                EditorGUILayout.HelpBox("ATleast 3 Points Needed", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Num Points : " + numPoints, MessageType.Info);
            }
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Center"));
        EditorList.ShowList(Points, EditorListOptions.DEFAULT | EditorListOptions.ADD_BUTTONS);
        EditorGUILayout.PropertyField(Frequency);
        
        
        if(serializedObject.ApplyModifiedProperties() || (Event.current.type == EventType.ValidateCommand || Event.current.commandName == "UndoRedoPerformed"))
        {
            foreach(Star star in serializedObject.targetObjects)
            {
                if(PrefabUtility.GetPrefabType(star) != PrefabType.Prefab)
                    star.UpdateMesh();
            }
        }

    }

    private static Vector3 pointSnap = Vector3.one*0.1f;
    void OnSceneGUI()
    {
        Star star = target as Star;
        Transform transform = star.transform;
        float numDegress = -360f / (star.Points.Length * star.Frequency);
      
        for (int idx = 0; idx < star.Points.Length; idx++)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, numDegress*idx);
            Vector3 localCoordinate = rotation*star.Points[idx].Position;
            Vector3 worldCoordinate = transform.TransformPoint(localCoordinate);
            Vector3 newWorldCoordinate =  Handles.FreeMoveHandle(worldCoordinate, rotation, 0.02f, pointSnap, Handles.DotCap);
            if (newWorldCoordinate != worldCoordinate)
            {
                Undo.RecordObject(star, "Move");
                Vector3 newLocalCoordinate = transform.InverseTransformPoint(newWorldCoordinate);
                Vector3 pointWithoutRotation = Quaternion.Inverse(rotation) * newLocalCoordinate;
                star.Points[idx].Position = pointWithoutRotation;
                
                star.UpdateMesh();

            }
        
        }
        
            

    }


}
