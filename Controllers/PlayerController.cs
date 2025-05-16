using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;    // �������� �������� ����������� � ������
    public float gridSize = 1f;     // ������ ����� ������ �� ����� �����

    private Vector2 targetPosition;   // ���� �������� ������ �������������
    private bool isMoving = false;    // ��������� �� �������� ������?

    // ���� ������� ��� ������ ����������� (���� �����������)
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // ������������� ��������� ������� ��� ������� �������, ����� �������� �� �������� ��� ������
        targetPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        // ���������� ��������� ������ (��������, ��� �������)
        if (spriteRenderer != null && spriteDown != null)
        {
            spriteRenderer.sprite = spriteDown;
        }
    }

    void Update()
    {
        // ���� �������� �� ��������� � �������� �������� � ������
        if (!isMoving)
        {
            // ��������� ���� � �������� ������ ��������
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

        // ���� �������� ������ ��������� (isMoving == true)
        if (isMoving)
        {
            // ������ ���������� ��������� � targetPosition
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ���� �������� ������ (��� ����� ������ �) targetPosition
            if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition; // ������������� ������ �������
                isMoving = false;                    // ��������� ��������
            }
        }
    }

    void AttemptMove(Vector2 direction, Sprite directionSprite)
    {
        // ������������ ������������� ��������� ������� ����� �� ���� ������
        Vector2 potentialNextCell = (Vector2)transform.position + direction * gridSize;

        if (!IsCellWalkable(potentialNextCell))
        {
            return; // �� ���������, ���� ������ �������������
        }

        // ���� ������ ���������, ������������� �� ��� ����� ����
        targetPosition = potentialNextCell;
        isMoving = true; // �������� ��������

        // ��������� ������ � ������������ � ������������ (���� �����������)
        if (spriteRenderer != null && directionSprite != null)
        {
            spriteRenderer.sprite = directionSprite;
            // ������ ��� ��������� ������� �����/������, ���� �����
            if (direction == Vector2.right) spriteRenderer.flipX = false;
        }
    }

    // ������ ������� �������� ������������ ������ (��������)
    // ��� ����� ����� ����������� �� �� ������ ����� Tilemap ��� ������� �����������
    bool IsCellWalkable(Vector2 cellPosition)
    {
        // ������: �������� �� ������� ���������� ����� � ���� �����
        Collider2D hitCollider = Physics2D.OverlapPoint(cellPosition, LayerMask.GetMask("Obstacles")); // � ���� ������ ���� ���� "Obstacles"
        if (hitCollider != null)
        {
            return false; // ������ �������������
        }
        return true; // ������ ��������
    }
}