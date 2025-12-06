using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class Door : MonoBehaviour
{
    [SerializeField]
    Switch _switch;

    Color doorColor;
    Color unactiveColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorColor = gameObject.GetComponent<SpriteRenderer>().color;
        unactiveColor = new Color(doorColor.r, doorColor.g, doorColor.b, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_switch.isActive)
        {
            ActivateDoor();
        }
    }

    void ActivateDoor()
    {

        gameObject.GetComponent<SpriteRenderer>().color = unactiveColor;
        gameObject.GetComponent<Collider2D>().isTrigger = true;

    }
}


