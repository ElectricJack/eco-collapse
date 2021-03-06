﻿using System.Collections;
using System.Collections.Generic;
using EntitySystem;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public EntityManager entityManager;

    public Camera camera;

    public EcoGamePlayerEnergy PlayerEnergy;
    
    public void UpdateTool(Tool tool)
    {
        activeTool = tool;
        Manager.SetPlayerTool(tool);
    }

    public void Awake()
    {
        for(int f = 0; f < FloraTools.Count; f++)
        {
            int index = f;
            FloraTools[index].UI_Link.onClick.AddListener(() =>
            {
                UpdateTool(FloraTools[index]);
            });
        }
        
        for(int f = 0; f < FaunaTools.Count; f++)
        {
            int index = f;
            FaunaTools[index].UI_Link.onClick.AddListener(() =>
            {
                UpdateTool(FaunaTools[index]);
            });
        }
    }
    
    public void Update()
    {
        if (!Stepper.isReady || !Stepper.enabled)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement())
            {
                if (activeTool != null && PlayerEnergy.current >= activeTool.ENERGY_REQUIREMENT)
                {
                    RaycastHit hit;
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
                    if (Physics.Raycast(ray, out hit)) {
                        entityManager.SpawnEntity(hit.point, activeTool.targetEntity);

                        PlayerEnergy.current -= activeTool.ENERGY_REQUIREMENT;
                    }
                }
                else
                {
                    UpdateTool(null);
                }
            }
        }
    }
    
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {   
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }
}

[System.Serializable]
public class Tool
{
    public string ToolName;
    public string ToolDescription;
    public Sprite mouseIcon;
    public float ENERGY_REQUIREMENT;
    public Button UI_Link;
    public EntityProfile targetEntity;
}