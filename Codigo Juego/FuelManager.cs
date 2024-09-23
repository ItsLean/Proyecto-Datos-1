using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    public int fuel = 100; // Combustible inicial
    public Text fuelText; // Referencia al componente Text en el Canvas
    public GameObject moto; // Referencia al GameObject de la moto

    void Start()
    {
        UpdateFuelText(); // Inicializar el texto del combustible
        StartCoroutine(ConsumeFuel()); // Iniciar el consumo de combustible
    }

    // Método IEnumerator para consumir combustible
    IEnumerator ConsumeFuel()
    {
        while (fuel > 0)
        {
            yield return new WaitForSeconds(2f); // Esperar 2 segundos
            fuel -= 1; // Disminuir el combustible
            UpdateFuelText(); // Actualizar el texto del combustible
        }

        // Si el combustible llega a 0, destruir la moto
        if (fuel <= 0)
        {
            DestroyMoto();
        }
    }

    // Método para actualizar el texto del combustible en el Canvas
    void UpdateFuelText()
    {
        if (fuelText != null)
        {
            fuelText.text = "Combustible: " + fuel.ToString();
        }
    }

    // Método para destruir la moto cuando el combustible se acaba
    void DestroyMoto()
    {
        if (moto != null)
        {
            Destroy(moto); // Destruir la moto
            Debug.Log("Moto destruida por falta de combustible");
        }
    }
}
