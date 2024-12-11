using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChainLightningController))]
public class ChainLightingControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ChainLightningController controller = (ChainLightningController)target;

        if (GUILayout.Button("Update Chain Lighting (MST)"))
        {
            controller.TrySpawnTreeLightning();
            controller.TriggerLightning();
        }

        if (GUILayout.Button("Update Chain Lighting (Target Sequence)"))
        {
            controller.TrySpawnLightning();
            controller.TriggerLightning();
        }

        if (GUILayout.Button("Reset Chain Lighting"))
        {
            controller.ResetLighting();
        }
        
        base.OnInspectorGUI();
    }
}
