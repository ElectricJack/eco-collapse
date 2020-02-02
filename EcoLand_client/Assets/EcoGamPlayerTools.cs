using System.Collections;
using System.Collections.Generic;
using EntitySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EcoGamPlayerTools : MonoBehaviour
{
    public List<Tool> FloraTools;
    public List<Tool> FaunaTools;

    private Tool activeTool;

    public MouserManager Manager;

    public WorldStepper Stepper;
    
    public void UpdateTool(Tool tool)
    {
        activeTool = tool;
        Manager.SetPlayerTool(tool);
    }

    public void Update()
    {
        if (!Stepper.isReady || activeTool == null || Stepper.enabled)
            return;
    }
}

[System.Serializable]
public class Tool
{
    public string ToolName;
    public string ToolDescription;
    public Image mouseIcon;
    public float ENERGY_REQUIREMENT;
    public Button UI_Link;
    public EntityProfile targetEntity;
}