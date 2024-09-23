using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tron5 : MonoBehaviour 
{
    public GameObject spawned;
    private Vector2 moveDirection = Vector2.zero; // Dirección inicial
    public float moveSpeed = 2.5f; // Velocidad de movimiento
    private bool isMoving = true; // Estado de si la moto está en movimiento
    public int maxClones = 3; // Número máximo de clones visibles
    private List<GameObject> clones = new List<GameObject>(); // Lista para almacenar clones

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
        }
    }

    // Método para cambiar la dirección aleatoriamente
    IEnumerator ChangeDirectionRandomly()
    {
        // Cambiar a moverse hacia la derecha
        moveDirection = Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0); // Ajustar rotación hacia la derecha
        yield return new WaitForSeconds(2.0f); // Mantener la dirección durante 2 segundos

        // Iniciar moviéndose hacia abajo
        moveDirection = Vector2.down;
        transform.rotation = Quaternion.Euler(0, 0, 270); // Ajustar rotación hacia abajo
        yield return new WaitForSeconds(2.0f); // Mantener la dirección inicial por 2 segundos

        // Ahora comenzar el movimiento aleatorio
        while (true)
        {
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
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f)); // Cambiar dirección cada 1 a 2 segundos
        }
    }

    // Método para aplicar el movimiento
    void Move()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    // Método que se ejecuta cuando hay una colisión
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada");
        // Lógica en caso de colisión
        // Destroy(gameObject); // Destruye el objeto en caso de colisión
    }

    // Método IEnumerator para generar clones
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Espera antes de crear el clon

            // Crear el clon
            GameObject clone = Instantiate(gameObject); // Clon de la moto actual
            clone.transform.localScale = transform.localScale * 0.4f; // Reducir el tamaño del clon

            // Desactivar el script en el clon para que no cree más clones
            clone.GetComponent<Tron5>().enabled = false; // Asegúrate de que el nombre del script sea correcto

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

            // Ajustar la posición del clon para que aparezca detrás de la moto
            float distanceBehind = transform.localScale.x * 0.4f; // Ajustar la distancia según la escala
            clone.transform.position = (Vector3)transform.position - (Vector3)moveDirection * distanceBehind; 

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
