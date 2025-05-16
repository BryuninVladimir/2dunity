using UnityEngine;
using UnityEngine.Tilemaps; // Необходимо для работы с Tilemap
using System.Collections.Generic; // Необходимо для использования List

public class EggSpawner : MonoBehaviour {
    [Header("Настройки Префаба и Количества")]
    public GameObject eggPrefab; // Префаб вашего яйца
    public int numberOfEggsToSpawn = 10; // Сколько яиц нужно создать

    [Header("Ссылки на Tilemaps")]
    public Tilemap groundTilemap; // Tilemap, на которой можно размещать яйца (пол)
    public Tilemap wallTilemap;   // Tilemap со стенами (чтобы не спавнить на них) - необязательно

    [Header("Ограничения Спавна")]
    public Vector3Int playerStartCellPosition; // Координаты стартовой клетки игрока (чтобы не спавнить на ней)
    public bool avoidPlayerStartCell = true;   // Избегать ли стартовую клетку игрока

    private List<Vector3Int> validSpawnCellPositions = new List<Vector3Int>();

    void Start()
    {
        if (eggPrefab == null)
        {
            Debug.LogError("Префаб яйца (eggPrefab) не назначен в EggSpawner!");
            return;
        }

        if (groundTilemap == null)
        {
            Debug.LogError("Tilemap для земли (groundTilemap) не назначена в EggSpawner!");
            return;
        }

        FindValidSpawnPositions();
        SpawnEggs();
    }

    void FindValidSpawnPositions()
    {
        BoundsInt bounds = groundTilemap.cellBounds; // Получаем границы Tilemap, где есть тайлы

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // 1. Проверяем, есть ли тайл на groundTilemap в этой клетке
                if (groundTilemap.HasTile(cellPosition))
                {
                    // 2. Проверяем, нет ли тайла на wallTilemap в этой клетке (если wallTilemap указана)
                    if (wallTilemap != null && wallTilemap.HasTile(cellPosition))
                    {
                        continue; // Это стена, пропускаем
                    }

                    // 3. Проверяем, не является ли это стартовой клеткой игрока (если нужно избегать)
                    if (avoidPlayerStartCell && cellPosition == playerStartCellPosition)
                    {
                        continue; // Это стартовая клетка игрока, пропускаем
                    }

                    // Если все проверки пройдены, добавляем позицию клетки в список доступных
                    validSpawnCellPositions.Add(cellPosition);
                }
            }
        }

        if (validSpawnCellPositions.Count == 0)
        {
            Debug.LogWarning("Не найдено ни одной подходящей клетки для спавна яиц!");
        }
    }

    void SpawnEggs()
    {
        if (validSpawnCellPositions.Count == 0) return;

        int eggsToPlace = Mathf.Min(numberOfEggsToSpawn, validSpawnCellPositions.Count);
        List<Vector3Int> availableCells = new List<Vector3Int>(validSpawnCellPositions); // Копируем список, чтобы из него можно было удалять

        for (int i = 0; i < eggsToPlace; i++)
        {
            if (availableCells.Count == 0) break; // На случай, если что-то пошло не так

            // Выбираем случайную клетку из доступных
            int randomIndex = Random.Range(0, availableCells.Count);
            Vector3Int spawnCell = availableCells[randomIndex];

            // Удаляем выбранную клетку из списка, чтобы не заспавнить два яйца в одном месте
            availableCells.RemoveAt(randomIndex);

            // Получаем мировую позицию центра выбранной клетки
            Vector3 spawnWorldPosition = groundTilemap.GetCellCenterWorld(spawnCell);

            // Создаем экземпляр яйца
            Instantiate(eggPrefab, spawnWorldPosition, Quaternion.identity, transform); // transform здесь - родительский объект для яиц (этот Spawner)
            // Debug.Log($"Заспавнено яйцо в клетке: {spawnCell} (мир. коорд.: {spawnWorldPosition})");
        }

        if (eggsToPlace < numberOfEggsToSpawn)
        {
            Debug.LogWarning($"Не хватило места для всех яиц. Заспавнено: {eggsToPlace} из {numberOfEggsToSpawn}");
        }
    }
}