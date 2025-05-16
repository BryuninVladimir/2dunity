using UnityEngine;

public class CollectibleItem : MonoBehaviour {
    // �������������: ���� �� ������, ����� ������ �������� ������ ������ ���������� �����
    public int scoreValue = 10;

    // �������������: ������ �� ������ ��� ���� ��� �����
    // public GameObject collectionEffectPrefab;
    // public AudioClip collectionSound;

    // ���� ����� ����������, ����� ������ ��������� ������ � ������� ����� �������
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // ���������, �������� �� �������� ��������� ���������� ������.
        // ��� ����� � ��������� ������ ������ ���� ��� "Player".
        // �������� ������ ��������� � � ���������� ������ ���������� Tag -> Add Tag... -> "+" -> �������� "Player" -> ��������� -> ����� �������� ��������� � ���������� ��� ��� "Player".
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            Collect(otherCollider.gameObject);
        }
    }

    void Collect(GameObject playerObject)
    {
        Debug.Log("������ ������!");

        // ����� �� ������ �������� ������:
        // 1. ��������� ���� ������ (������ ����� ��������� � GameManager'�)
        //    GameManager.instance.AddScore(scoreValue); // ������, ���� � ��� ���� GameManager

        // 2. ������������� ���� �����
        //    if (collectionSound != null)
        //    {
        //        AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        //    }

        // 3. ������� ���������� ������ �����
        //    if (collectionEffectPrefab != null)
        //    {
        //        Instantiate(collectionEffectPrefab, transform.position, Quaternion.identity);
        //    }

        // 4. ���������� ���� ���������� ������
        Destroy(gameObject);
    }
}