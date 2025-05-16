using UnityEngine;

public class CollectibleItem : MonoBehaviour {
    // Необязательно: если вы хотите, чтобы разные предметы давали разное количество очков
    public int scoreValue = 10;

    // Необязательно: ссылка на эффект или звук при сборе
    // public GameObject collectionEffectPrefab;
    // public AudioClip collectionSound;

    // Этот метод вызывается, когда другой коллайдер входит в триггер этого объекта
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Проверяем, является ли вошедший коллайдер персонажем игрока.
        // Для этого у персонажа игрока должен быть тег "Player".
        // Выберите вашего персонажа и в Инспекторе сверху установите Tag -> Add Tag... -> "+" -> назовите "Player" -> сохраните -> снова выберите персонажа и установите ему тег "Player".
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            Collect(otherCollider.gameObject);
        }
    }

    void Collect(GameObject playerObject)
    {
        Debug.Log("Объект собран!");

        // Здесь вы можете добавить логику:
        // 1. Увеличить счет игрока (обычно через обращение к GameManager'у)
        //    GameManager.instance.AddScore(scoreValue); // Пример, если у вас есть GameManager

        // 2. Воспроизвести звук сбора
        //    if (collectionSound != null)
        //    {
        //        AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        //    }

        // 3. Создать визуальный эффект сбора
        //    if (collectionEffectPrefab != null)
        //    {
        //        Instantiate(collectionEffectPrefab, transform.position, Quaternion.identity);
        //    }

        // 4. Уничтожить этот собираемый объект
        Destroy(gameObject);
    }
}