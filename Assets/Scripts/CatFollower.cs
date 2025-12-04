using UnityEngine;
using UnityEngine.InputSystem; // New Input System

[RequireComponent(typeof(Rigidbody2D))]
public class CatFollower : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Camera targetCamera;

    [Header("Movimiento hacia adelante")]
    [Tooltip("Velocidad max")]
    [SerializeField] private float maxSpeed = 8f;

    [Tooltip("Aceleración forward del gato")]
    [SerializeField] private float forwardAcceleration = 25f;

    [Header("Stop")]
    [Tooltip("Radio en el que el gato deja de intentar seguir al puntero")]
    [SerializeField] private float stopRadius = 0.8f;

    [Tooltip("Smooth stopping dentro del stopRadius (0 = solo drag)")]
    [SerializeField] private float softBrakeStrength = 10f;

    [Tooltip("Velocidad por debajo de la cual forzamos que se considere parado.")]
    [SerializeField] private float stopSpeedThreshold = 0.05f;

    [Header("Giro")]
    [Tooltip("Vel max de giro normal en grados por segundo")]
    [SerializeField] private float maxTurnSpeed = 160f;

    [Header("Reorientación desde stop")]
    [Tooltip("Vel de giro cuando el gato ha salido del stopRadius y se está reorientando")]
    [SerializeField] private float reorientTurnSpeed = 360f;

    [Tooltip("Ángulo por debajo del cual salimos del giro rapido")]
    [SerializeField] private float angleToExitReorient = 10f;

    [Header("Físicas")]
    [Tooltip("Drag lineal Rigidbody2D")]
    [SerializeField] private float linearDrag = 3f;

    private Rigidbody2D _rb;
    private Vector3 _currentMouseWorldPos;

    // Estados
    private bool _inStopZone = false;   // Estaba dentro del stopRadius en el frame anterior
    private bool _fastTurnMode = false; // Acaba de salir del stopRadius == giro rápido

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearDamping = linearDrag;

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void Update()
    {
        if (targetCamera == null) return;
        if (Mouse.current == null) return;

        // puntero

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        var screenPoint = new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            Mathf.Abs(targetCamera.transform.position.z)
        );

        Vector3 mouseWorldPos = targetCamera.ScreenToWorldPoint(screenPoint);
        mouseWorldPos.z = transform.position.z;

        _currentMouseWorldPos = mouseWorldPos;
    }

    private void FixedUpdate()
    {
        // vector al puntero desde el gato 

        Vector2 toTarget = (Vector2)(_currentMouseWorldPos - transform.position);
        float distance = toTarget.magnitude;
        float currentSpeed = _rb.linearVelocity.magnitude;

        bool insideStop = distance <= stopRadius;

        // Miramos si esta dentro del stop radius
        if (insideStop)
        {
            _inStopZone = true; // Marcamos que ha estado dentro

            if (currentSpeed > 0.0001f && softBrakeStrength > 0f)
            {
                Vector2 brakeDir = -_rb.linearVelocity.normalized;
                _rb.AddForce(brakeDir * softBrakeStrength, ForceMode2D.Force);
            }

            // Cuando va muy lento paramos para evitar jitters raros
            if (currentSpeed <= stopSpeedThreshold)
            {
                _rb.linearVelocity = Vector2.zero;
            }

            // No perseguimos mientras estamos dentro del radio 
            return;
        }

        // Sale de stop radius == giro rapido

        if (!insideStop && _inStopZone)
        {
            _inStopZone = false;
            _fastTurnMode = true; // activamos giro rápido
        }

        // Fuera del stop radius == movimiento normal 

        Vector2 dirToTarget = toTarget.normalized;

        float currentAngle = _rb.rotation;

        float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;

        // Diferencia de ángulo antes de girar
        float angleDiffBefore = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Elegimos la velocidad de giro según el modo
        float turnSpeed = _fastTurnMode ? reorientTurnSpeed : maxTurnSpeed;
        float maxStep = turnSpeed * Time.fixedDeltaTime;
        float angleStep = Mathf.Clamp(angleDiffBefore, -maxStep, maxStep);

        float newAngle = currentAngle + angleStep;
        _rb.MoveRotation(newAngle);

        // Revisamos si ya podemos salir de giro rápido
        if (_fastTurnMode)
        {
            float angleDiffAfter = Mathf.DeltaAngle(newAngle, targetAngle);
            if (Mathf.Abs(angleDiffAfter) <= angleToExitReorient)
            {
                _fastTurnMode = false;
            }
        }

        // Movimiento hacia delante con giro
        
        Vector2 forward = transform.up;
        _rb.AddForce(forward * forwardAcceleration, ForceMode2D.Force);

        // Limitamos max vel
        float speed = _rb.linearVelocity.magnitude;
        if (speed > maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
        }
    }

   
}