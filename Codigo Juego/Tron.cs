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
    private FuelManager fuelManager; // Referencia al FuelManager

    void Start()
    {
        fuelManager = FindObjectOfType<FuelManager>(); // Encontrar el FuelManager en la escena
        StartCoroutine(Spawn()); // Iniciar la creación de clones
        AddHitbox(); // Añadir hitbox
    }

    void Update()
    {
        HandleInput(); // Manejar las teclas de entrada
        if (isMoving) // Solo mover si la moto está en movimiento
        {
            Move();
            CheckBoundary(); // Verificar si la moto está fuera de los límites
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
        if (fuelManager != null && fuelManager.fuel > 0) // Verificar si hay combustible
        {
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    // Método para verificar si la moto está fuera de los límites
    void CheckBoundary()
    {
        if (transform.position.x < -10.27 || transform.position.x > 10.27 ||
            transform.position.y < -4.3 || transform.position.y > 4.27)
        {
            Debug.Log("La moto ha salido del mapa y se desintegra.");
            Destroy(gameObject); // Destruir la moto

            // Destruir todos los clones
            foreach (GameObject clone in clones)
            {
                Destroy(clone);
            }
            clones.Clear(); // Vaciar la lista de clones
        }
    }

    // Método para añadir la hitbox
    void AddHitbox()
    {
        CircleCollider2D hitbox = gameObject.AddComponent<CircleCollider2D>();
        hitbox.radius = 0.5f; // Establecer el radio de la hitbox
        hitbox.isTrigger = true; // Hacer que la hitbox sea un trigger
    }

    // Método que se ejecuta cuando hay una colisión
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Motorcycle")) // Asegúrate de que las otras motos tengan esta etiqueta
        {
            Debug.Log("Colisión detectada con: " + other.gameObject.name);
            Destroy(other.gameObject); // Destruir la otra moto
            Destroy(gameObject); // Destruir esta moto
        }
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

            // Ajustar la posición del clon para que aparezca detrás de la moto
            spawned.transform.position = (Vector3)transform.position - (Vector3)moveDirection * 0.3f; // Ajusta la distancia según sea necesario

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
