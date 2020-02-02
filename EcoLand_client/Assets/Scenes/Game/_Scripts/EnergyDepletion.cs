using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDepletion : MonoBehaviour
{
    [SerializeField]
    private Image mask;

    [SerializeField] private EcoGamePlayerEnergy _playerEnergy;

    // Update is called once per frame
    void Update()
    {
        mask.fillAmount = Mathf.Clamp01(_playerEnergy.current / _playerEnergy.max);
    }
}