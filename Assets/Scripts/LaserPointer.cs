using UnityEngine;
using UnityEngine.InputSystem; // 👈 Necesario

public class LaserPointer : MonoBehaviour
{
    [SerializeField, Tooltip("Cámara que se usa para convertir la posición del ratón a mundo. Si se deja vacío, usa Camera.main.")]
    private Camera targetCamera;

    [SerializeField, Tooltip("Si está activo, se ocultará el cursor del sistema.")]
    private bool hideSystemCursor = true;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (hideSystemCursor)
            Cursor.visible = false;
    }

    private void OnDestroy()
    {
        if (hideSystemCursor)
            Cursor.visible = true;
    }

    private void Update()
    {
        if (targetCamera == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = targetCamera.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(targetCamera.transform.position.z))
        );

        mouseWorldPos.z = 0f; // Suponiendo escena 2D
        transform.position = mouseWorldPos;
    }
}