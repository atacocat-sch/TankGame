using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLoading : MonoBehaviour
{
    [SerializeField] TankGun driver;
    
    [Space]
    [SerializeField] TankLoaderShell shell;
    [SerializeField] TankLoaderCasing casing;

    [SerializeField] ChamberContents chamberContents;

    bool locked;

    public ChamberContents CurrentChamberContents
    {
        get => chamberContents;
        set 
        {
            driver.Loaded = value == ChamberContents.Shell;

            if (chamberContents == ChamberContents.Shell && value != ChamberContents.Shell)
            {
                shell.ResetShell();
                casing.ResetCasing();
            }

            chamberContents = value;
        }
    }

    public bool Locked { get => driver.Locked; set => driver.Locked = value; }

    private void OnEnable()
    {
        driver.OnShootEvent += OnShootEvent;
    }

    private void OnDisable()
    {
        driver.OnShootEvent -= OnShootEvent;
    }

    private void OnShootEvent()
    {
        CurrentChamberContents = ChamberContents.Casing;
    }

    public enum ChamberContents
    {
        Shell,
        Casing,
        Empty,
    }
}
