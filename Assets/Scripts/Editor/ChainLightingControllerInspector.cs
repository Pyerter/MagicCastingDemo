using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChainLightningController))]
public class ChainLightingControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        ChainLightningController controller = (ChainLightningController)target;

        if (GUILayout.Button("Update Chain Lighting"))
        {
            controller.TriggerLightning();
        }

        if (GUILayout.Button("Reset Chain Lighting"))
        {
            controller.ResetLighting();
        }
    }
}
