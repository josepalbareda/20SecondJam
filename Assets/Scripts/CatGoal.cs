using UnityEngine;

public class CatGoalSimple : MonoBehaviour
{
    public GameObject blackBG;   // Fondo
    public GameObject winImage;  // foto victoria

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))   
        {
            blackBG.SetActive(true);
            winImage.SetActive(true);

            // pausa el juego
            Time.timeScale = 0f;
        }
    }
}