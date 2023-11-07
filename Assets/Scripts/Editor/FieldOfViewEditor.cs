using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateMachine))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyStateMachine fow = (EnemyStateMachine)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.radius);

        Vector3 viewAngleA = DirectionFromAngle(fow.transform.eulerAngles.y, -fow.angle / 2);
        Vector3 viewAngleB = DirectionFromAngle(fow.transform.eulerAngles.y, fow.angle / 2);


        Handles.color = Color.red;
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.radius);

        
        if (fow.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fow.transform.position, fow.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
