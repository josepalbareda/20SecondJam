using UnityEngine;

public class CatGoalSimple : MonoBehaviour
{
    public GameObject blackBG;   // Fondo
    public GameObject winImage;  // foto victoria
    public bool catHasJewels;    // si el gato tiene joyas
    [SerializeField] GameObject jewelsSprite; // sprite de las joyas

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal") && catHasJewels)   
        {
            blackBG.SetActive(true);
            winImage.SetActive(true);

            // pausa el juego
            Time.timeScale = 0f;
        }
        if (other.CompareTag("Jewels") && !catHasJewels)
        {
            catHasJewels = true;
            jewelsSprite.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }
}