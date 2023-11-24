using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateMachine))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyStateMachine fow = (EnemyStateMachine)target;
        if(fow != null)
        {
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.radiusToCheck);

            Vector3 viewAngleA = DirectionFromAngle(fow.transform.eulerAngles.y, -fow.angle / 2);
            Vector3 viewAngleB = DirectionFromAngle(fow.transform.eulerAngles.y, fow.angle / 2);


            Handles.color = Color.red;
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.radiusToCheck);
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.radiusToCheck);


            if (fow.canSeeTarget)
            {
                Handles.color = Color.green;

                if(fow.target != null)
                    Handles.DrawLine(fow.transform.position, fow.target.transform.position);
            }
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
