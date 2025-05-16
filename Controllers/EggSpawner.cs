using UnityEngine;
using UnityEngine.Tilemaps; // ���������� ��� ������ � Tilemap
using System.Collections.Generic; // ���������� ��� ������������� List

public class EggSpawner : MonoBehaviour {
    [Header("��������� ������� � ����������")]
    public GameObject eggPrefab; // ������ ������ ����
    public int numberOfEggsToSpawn = 10; // ������� ��� ����� �������

    [Header("������ �� Tilemaps")]
    public Tilemap groundTilemap; // Tilemap, �� ������� ����� ��������� ���� (���)
    public Tilemap wallTilemap;   // Tilemap �� ������� (����� �� �������� �� ���) - �������������

    [Header("����������� ������")]
    public Vector3Int playerStartCellPosition; // ���������� ��������� ������ ������ (����� �� �������� �� ���)
    public bool avoidPlayerStartCell = true;   // �������� �� ��������� ������ ������

    private List<Vector3Int> validSpawnCellPositions = new List<Vector3Int>();

    void Start()
    {
        if (eggPrefab == null)
        {
            Debug.LogError("������ ���� (eggPrefab) �� �������� � EggSpawner!");
            return;
        }

        if (groundTilemap == null)
        {
            Debug.LogError("Tilemap ��� ����� (groundTilemap) �� ��������� � EggSpawner!");
            return;
        }

        FindValidSpawnPositions();
        SpawnEggs();
    }

    void FindValidSpawnPositions()
    {
        BoundsInt bounds = groundTilemap.cellBounds; // �������� ������� Tilemap, ��� ���� �����

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // 1. ���������, ���� �� ���� �� groundTilemap � ���� ������
                if (groundTilemap.HasTile(cellPosition))
                {
                    // 2. ���������, ��� �� ����� �� wallTilemap � ���� ������ (���� wallTilemap �������)
                    if (wallTilemap != null && wallTilemap.HasTile(cellPosition))
                    {
                        continue; // ��� �����, ����������
                    }

                    // 3. ���������, �� �������� �� ��� ��������� ������� ������ (���� ����� ��������)
                    if (avoidPlayerStartCell && cellPosition == playerStartCellPosition)
                    {
                        continue; // ��� ��������� ������ ������, ����������
                    }

                    // ���� ��� �������� ��������, ��������� ������� ������ � ������ ���������
                    validSpawnCellPositions.Add(cellPosition);
                }
            }
        }

        if (validSpawnCellPositions.Count == 0)
        {
            Debug.LogWarning("�� ������� �� ����� ���������� ������ ��� ������ ���!");
        }
    }

    void SpawnEggs()
    {
        if (validSpawnCellPositions.Count == 0) return;

        int eggsToPlace = Mathf.Min(numberOfEggsToSpawn, validSpawnCellPositions.Count);
        List<Vector3Int> availableCells = new List<Vector3Int>(validSpawnCellPositions); // �������� ������, ����� �� ���� ����� ���� �������

        for (int i = 0; i < eggsToPlace; i++)
        {
            if (availableCells.Count == 0) break; // �� ������, ���� ���-�� ����� �� ���

            // �������� ��������� ������ �� ���������
            int randomIndex = Random.Range(0, availableCells.Count);
            Vector3Int spawnCell = availableCells[randomIndex];

            // ������� ��������� ������ �� ������, ����� �� ���������� ��� ���� � ����� �����
            availableCells.RemoveAt(randomIndex);

            // �������� ������� ������� ������ ��������� ������
            Vector3 spawnWorldPosition = groundTilemap.GetCellCenterWorld(spawnCell);

            // ������� ��������� ����
            Instantiate(eggPrefab, spawnWorldPosition, Quaternion.identity, transform); // transform ����� - ������������ ������ ��� ��� (���� Spawner)
            // Debug.Log($"���������� ���� � ������: {spawnCell} (���. �����.: {spawnWorldPosition})");
        }

        if (eggsToPlace < numberOfEggsToSpawn)
        {
            Debug.LogWarning($"�� ������� ����� ��� ���� ���. ����������: {eggsToPlace} �� {numberOfEggsToSpawn}");
        }
    }
}