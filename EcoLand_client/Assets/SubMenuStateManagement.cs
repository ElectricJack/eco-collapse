using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubMenuStateManagement : MonoBehaviour
{

    public Button FloraButton;
    public Button FaunaButton;

    public GameObject floraMenu;
    public GameObject faunaMenu;
    
    public enum ActiveMenu
    {
        None,
        Flora,
        Fauna
    }

    private ActiveMenu _menu;
    private ActiveMenu menu
    {
        set
        {
            if (value != _menu)
            {
                _menu = value;
            }
            else
            {
                _menu = ActiveMenu.None;
            }
            
            faunaMenu.SetActive(_menu == ActiveMenu.Fauna);
            floraMenu.SetActive(_menu == ActiveMenu.Flora);
        }
    }

    public void Awake()
    {
        FloraButton.onClick.AddListener(() => { menu = ActiveMenu.Flora; });
        FaunaButton.onClick.AddListener(() => { menu = ActiveMenu.Fauna; });

        menu = ActiveMenu.None;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu = ActiveMenu.None;
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
