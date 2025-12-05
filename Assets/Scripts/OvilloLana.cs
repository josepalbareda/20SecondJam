using UnityEngine;

public class OvilloLana : MonoBehaviour
{
    public float shrinkRate = 0.003f;
    public float minScale = 0.05f;

    private TrailRenderer trail;
    private bool active = false;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        trail.time = Mathf.Infinity; // el trail no desaparece jamás
    }

    private void Update()
    {
        if (!active) return;

        // Reducir tamaño del ovillo
        float shrinkAmount = shrinkRate * Time.deltaTime;
        transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, 0f);

        // Cuando ya no quede lana...
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
        // 1. Desvincular el trail del ovillo
        trail.transform.parent = null;

        // 2. Dejar de emitir nueva lana
        trail.emitting = false;

        // 3. Asegurar que permanece en la escena
        trail.time = Mathf.Infinity;

        // 4. Destruir el ovillo PERO NO el trail
        Destroy(gameObject);
    }
}
