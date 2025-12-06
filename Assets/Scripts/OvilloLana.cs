using UnityEngine;

public class OvilloLana : MonoBehaviour
{
    public float shrinkRate = 0.003f;
    public float minScale = 0.05f;

    public TrailRenderer trail;   // <- referencia al TrailRenderer del hijo (YarnTrail)
    private bool active = false;

    private void Start()
    {
        // Si no lo arrastras manualmente en el inspector, puedes hacer:
        if (trail == null)
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }

        trail.emitting = false;
        trail.time = Mathf.Infinity; // la lana no desaparece por tiempo
    }

    private void Update()
    {
        if (!active) return;

        float shrinkAmount = shrinkRate * Time.deltaTime;
        transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, 0f);

        if (transform.localScale.x <= minScale)
        {
            LeaveTrailForever();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active = true;
            trail.emitting = true;
        }
    }

    void LeaveTrailForever()
    {
        // 1. Desparentar el objeto que TIENE el TrailRenderer
        trail.transform.SetParent(null);   // ahora es independiente

        // 2. Parar de emitir nueva lana
        trail.emitting = false;

        // 3. Asegurar que el trail se queda
        trail.time = Mathf.Infinity;

        // 4. Destruir SOLO el ovillo (este GameObject)
        Destroy(gameObject);
    }
}
