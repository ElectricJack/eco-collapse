using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouserManager : MonoBehaviour
{
    public Mouser mouser;

    public void SetPlayerTool(Tool tool)
    {
        if (tool == null)
        {
            mouser.gameObject.SetActive(false);
        }
        else
        {
            mouser.gameObject.SetActive(true);

            mouser.img.sprite = tool.mouseIcon;
        }
    }
}
