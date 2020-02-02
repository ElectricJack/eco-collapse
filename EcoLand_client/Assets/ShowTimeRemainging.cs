using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTimeRemainging : MonoBehaviour
{
    public EcoGameMain EcoGameMain;

    public TMPro.TMP_Text TextField;

    // Update is called once per frame
    void Update()
    {
        var t = Mathf.Clamp(EcoGameMain.TimeRemainging, 0f, EcoGameMain.GameLength);
        
        var s = (int) (t  % 60f);
        var m = (int) (t / 60f);
        TextField.text = $"{m}:{s}";
    }
}
