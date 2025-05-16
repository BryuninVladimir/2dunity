using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;    // Скорость плавного перемещения к клетке
    public float gridSize = 1f;     // Размер одной клетки на вашей сетке

    private Vector2 targetPosition;   // Куда персонаж должен переместиться
    private bool isMoving = false;    // Двигается ли персонаж сейчас?

    // Ваши спрайты для разных направлений (если используете)
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Устанавливаем начальную позицию как текущую целевую, чтобы персонаж не двигался при старте
        targetPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Установить начальный спрайт (например, вид спереди)
        if (spriteRenderer != null && spriteDown != null)
        {
            spriteRenderer.sprite = spriteDown;
        }
    }

    void Update()
    {
        // Если персонаж не находится в процессе движения к клетке
        if (!isMoving)
        {
            // Проверяем ввод и пытаемся начать движение
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AttemptMove(Vector2.up, spriteUp);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AttemptMove(Vector2.down, spriteDown);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AttemptMove(Vector2.left, spriteLeft);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AttemptMove(Vector2.right, spriteRight);
            }
        }

        // Если персонаж должен двигаться (isMoving == true)
        if (isMoving)
        {
            // Плавно перемещаем персонажа к targetPosition
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Если персонаж достиг (или очень близко к) targetPosition
            if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition; // Устанавливаем точную позицию
                isMoving = false;                    // Завершаем движение
            }
        }
    }

    void AttemptMove(Vector2 direction, Sprite directionSprite)
    {
        // Рассчитываем потенциальную следующую позицию ровно на одну клетку
        Vector2 potentialNextCell = (Vector2)transform.position + direction * gridSize;

        if (!IsCellWalkable(potentialNextCell))
        {
            return; // Не двигаемся, если клетка заблокирована
        }

        // Если клетка проходима, устанавливаем ее как новую цель
        targetPosition = potentialNextCell;
        isMoving = true; // Начинаем движение

        // Обновляем спрайт в соответствии с направлением (если используете)
        if (spriteRenderer != null && directionSprite != null)
        {
            spriteRenderer.sprite = directionSprite;
            // Логика для отражения спрайта влево/вправо, если нужно
            if (direction == Vector2.right) spriteRenderer.flipX = false;
        }
    }

    // Пример функции проверки проходимости клетки (заглушка)
    // Вам нужно будет реализовать ее на основе вашей Tilemap или системы коллайдеров
    bool IsCellWalkable(Vector2 cellPosition)
    {
        // Пример: проверка на наличие коллайдера стены в этой точке
        Collider2D hitCollider = Physics2D.OverlapPoint(cellPosition, LayerMask.GetMask("Obstacles")); // У стен должен быть слой "Obstacles"
        if (hitCollider != null)
        {
            return false; // Клетка заблокирована
        }
        return true; // Клетка свободна
    }
}