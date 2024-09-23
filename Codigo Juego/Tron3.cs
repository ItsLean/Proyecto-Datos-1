using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tron3 : MonoBehaviour
{
    public GameObject spawned;
    private Vector2 moveDirection = Vector2.zero; // Dirección inicial
    public float moveSpeed = 2.5f; // Velocidad de movimiento
    private bool isMoving = true; // Estado de si la moto está en movimiento
    public int maxClones = 3; // Número máximo de clones visibles
    private List<GameObject> clones = new List<GameObject>(); // Lista para almacenar clones

    // Definir los límites del mapa
    public float minX = -10.27f;
    public float maxX = 10.27f;
    public float minY = -4.3f;
    public float maxY = 4.27f;

    // Distancia mínima a los bordes para cambiar de dirección
    public float boundaryThreshold = 0.5f;

    void Start()
    {
        StartCoroutine(Spawn());  // Iniciar la creación de clones
        StartCoroutine(ChangeDirectionRandomly()); // Iniciar el cambio de dirección aleatorio
    }

    void Update()
    {
        if (isMoving)  // Solo mover si la moto está en movimiento
        {
            Move();
            CheckBounds(); // Verificar si la moto está dentro de los límites
        }
    }

    // Método para cambiar la dirección aleatoriamente o evitar los bordes
    IEnumerator ChangeDirectionRandomly()
    {
        // Iniciar moviéndose hacia arriba
        moveDirection = Vector2.up;
        transform.rotation = Quaternion.Euler(0, 0, 0); // Ajustar rotación hacia arriba
        yield return new WaitForSeconds(1.0f); // Mantener la dirección inicial por 1 segundos

        // Cambiar a moverse hacia la derecha
        moveDirection = Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 90); // Ajustar rotación hacia la derecha
        yield return new WaitForSeconds(1.0f); // Mantener la dirección durante 1 segundos

        // Ahora comenzar el movimiento aleatorio
        while (true)
        {
            // Evitar los bordes antes de elegir una nueva dirección aleatoria
            AvoidEdges();

            // Elegir una nueva dirección aleatoria
            float randomDirection = Random.Range(0f, 4f);
            if (randomDirection < 1)
                moveDirection = Vector2.up;
            else if (randomDirection < 2)
                moveDirection = Vector2.down;
            else if (randomDirection < 3)
                moveDirection = Vector2.left;
            else
                moveDirection = Vector2.right;

            // Rotación para ajustar la dirección del objeto
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Ajuste de ángulo

            // Esperar un intervalo de tiempo antes de cambiar de dirección
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f)); // Cambiar dirección cada 1 a 3 segundos
        }
    }

    // Método para aplicar el movimiento
    void Move()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    // Método para verificar si la moto sale de los límites
    void CheckBounds()
    {
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            Debug.Log("Moto fuera de los límites, destruyendo moto y clones");
            DestroyMotoAndClones();
        }
    }

    // Método para evitar que la moto se acerque demasiado a los bordes
    void AvoidEdges()
    {
        Vector3 position = transform.position;

        // Evitar el borde izquierdo
        if (position.x - boundaryThreshold < minX && moveDirection == Vector2.left)
        {
            moveDirection = Vector2.right; // Cambiar dirección
        }
        // Evitar el borde derecho
        else if (position.x + boundaryThreshold > maxX && moveDirection == Vector2.right)
        {
            moveDirection = Vector2.left; // Cambiar dirección
        }
        // Evitar el borde inferior
        if (position.y - boundaryThreshold < minY && moveDirection == Vector2.down)
        {
            moveDirection = Vector2.up; // Cambiar dirección
        }
        // Evitar el borde superior
        else if (position.y + boundaryThreshold > maxY && moveDirection == Vector2.up)
        {
            moveDirection = Vector2.down; // Cambiar dirección
        }
    }

    // Método que destruye la moto y los clones
    void DestroyMotoAndClones()
    {
        // Destruir todos los clones
        foreach (GameObject clone in clones)
        {
            Destroy(clone);
        }

        // Destruir la moto
        Destroy(gameObject);
    }

    // Método IEnumerator para generar clones
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Espera antes de crear el clon

            // Crear el clon
            GameObject clone = Instantiate(gameObject); // Clon de la moto actual

            // Reducir el tamaño del clon al 40% del original
            clone.transform.localScale = transform.localScale * 0.4f;

            // Desactivar el script en el clon para que no cree más clones
            clone.GetComponent<Tron3>().enabled = false;

            // Cambiar el color del clon y crear un borde negro
            SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer cloneRenderer = clone.GetComponent<SpriteRenderer>();

            // Asumimos que el clon tiene un SpriteRenderer
            cloneRenderer.color = originalRenderer.color; // Mismo color que el original

            // Crear borde negro
            GameObject border = new GameObject("Border");
            border.transform.position = clone.transform.position;
            border.transform.localScale = clone.transform.localScale * 1.1f; // Aumentar el tamaño para simular un borde
            border.transform.SetParent(clone.transform); // Hacer que el borde sea hijo del clon

            SpriteRenderer borderRenderer = border.AddComponent<SpriteRenderer>();
            borderRenderer.sprite = originalRenderer.sprite; // Usar el mismo sprite
            borderRenderer.color = Color.black; // Borde negro
            borderRenderer.sortingOrder = cloneRenderer.sortingOrder - 1; // Asegurarse de que el borde esté detrás

            // Agregar el clon a la lista de clones
            clones.Add(clone);

            // Si el número de clones excede el límite máximo, destruir el clon más antiguo
            if (clones.Count > maxClones)
            {
                Destroy(clones[0]); // Destruir el clon más antiguo
                clones.RemoveAt(0); // Eliminarlo de la lista
            }
        }
    }
}