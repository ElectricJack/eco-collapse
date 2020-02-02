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
        //TODO: Format numbers
        
        var s = (int) (EcoGameMain.TimeRemainging / 1000f % 60f);
        var m = (int) (EcoGameMain.TimeRemainging / (1000f * 60f));
        TextField.text = $"{m}:{s}";
    }
}
