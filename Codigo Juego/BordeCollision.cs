using UnityEngine;

public class BorderCollision : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Obtener la cámara principal
    }

    void Update()
    {
        // Llamar al método para verificar colisiones con los bordes
        CheckBorders();
    }

    void CheckBorders()
    {
        // Obtener las coordenadas de la cámara
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float screenHeight = mainCamera.orthographicSize;

        // Obtener la posición actual del objeto
        Vector2 position = transform.position;

        // Verificar colisiones con los bordes
        if (position.x < -screenWidth || position.x > screenWidth || 
            position.y < -screenHeight || position.y > screenHeight)
        {
            Debug.Log("Colisión con el borde de la pantalla detectada: " + position);
            // Aquí puedes agregar la lógica que deseas ejecutar en caso de colisión con el borde
            // Por ejemplo, puedes destruir el objeto o devolverlo al centro de la pantalla
            // Destroy(gameObject); // Descomentar para destruir el objeto
        }
    }
}
