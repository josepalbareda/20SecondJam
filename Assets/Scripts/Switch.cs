using System;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [NonSerialized] public bool isActive = false;
    
    public void Toggle()
    {
        isActive = true;
        Debug.Log("Switch is now " + (isActive ? "ON" : "OFF"));

        GetComponent<SpriteRenderer>().color = Color.yellow;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ovillo") && !isActive) { 
            Toggle();
        }
    }

}
