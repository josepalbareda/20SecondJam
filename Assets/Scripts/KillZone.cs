using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    public bool activateOther;

    [SerializeField] KillZone otherKillZone;
    [SerializeField] GameObject laser;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (otherKillZone.activateOther)
        {
            ActivateLaser();
        }
        else{
            laser.SetActive(false);
        }
    }

    void ToggleLaser()
    {
        otherKillZone.activateOther = false;
        activateOther = true;
        laser.SetActive(false);
    }

    void ActivateLaser()
    {
        activateOther = false;
        laser.SetActive(true);
        StartTimer();
    }

    void StartTimer()
    {
        Invoke("ToggleLaser", 2.5f);
    }
}