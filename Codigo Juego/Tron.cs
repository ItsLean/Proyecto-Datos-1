using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tron : MonoBehaviour
{
    public GameObject spawned;
    private Vector2 moveDirection = Vector2.zero; // Dirección inicial (estática)
    public float moveSpeed = 2.5f; // Velocidad de movimiento
    private bool isMoving = false; // Estado de si la moto está en movimiento
    public int maxClones = 3; // Número máximo de clones visibles
    private List<GameObject> clones = new List<GameObject>(); // Lista para almacenar los clones

    void Start()
    {
        StartCoroutine(Spawn()); // Iniciar la creación de clones
    }

    void Update()
    {
        HandleInput(); // Manejar las teclas de entrada
        if (isMoving) // Solo mover si la moto está en movimiento
        {
            Move();
        }
    }

    // Método para manejar la entrada de teclado
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
            isMoving = true; // Comenzar a mover la moto
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
            isMoving = true;
        }

        // Rotación para ajustar la dirección del objeto según la dirección del movimiento
        if (isMoving)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Ajuste de ángulo
        }
    }

    // Método para aplicar el movimiento
    void Move()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    // Método que se ejecuta cuando hay una colisión
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        // Aquí puedes agregar la lógica para manejar la colisión
        isMoving = false; // Detener la moto al colisionar
        // Aquí podrías también destruir la moto, reiniciar el juego, etc.
        // Destroy(gameObject); // Descomentar si deseas destruir la moto al colisionar
    }

    // Método IEnumerator para generar clones
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Espera antes de crear el clon

            // Crear el clon
            spawned = Instantiate(gameObject);

            // Reducir el tamaño del clon al 40% del original
            spawned.transform.localScale = transform.localScale * 0.4f;

            // Desactivar el script en el clon para que no cree más clones
            spawned.GetComponent<Tron>().enabled = false;

            // Agregar el clon a la lista de clones
            clones.Add(spawned);

            // Si el número de clones excede el límite máximo, destruir el clon más antiguo
            if (clones.Count > maxClones)
            {
                Destroy(clones[0]); // Destruir el clon más antiguo
                clones.RemoveAt(0); // Eliminarlo de la lista
            }
        }
    }
}
