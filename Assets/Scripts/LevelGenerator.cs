using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{


    [Header("Префабы блоков")]
    public GameObject blockGround; // Символ "g"
    public GameObject blockLava;   // Символ "l"
    public GameObject blockBorder;    // Символ "b"
    public GameObject playerPrefab;    // Преф призрака символ "S"
    public GameObject coinPrefab;      // Монетка символ "C" си

    [Header("Карта уровня (12x12)")]
    [TextArea(5, 10)]
    private string[] levelMap = new string[]
    {
        "bbbbbbbbbbbb",
        "blCllClCgggb",
        "blgllgllllCb",
        "blgCgggCgggb",
        "bllllgllllCb",
        "bCggCgllllgb",
        "bglllgSgglgb",
        "bglgggllggCb",
        "bClClllllglb",
        "bglllggClglb",
        "bgClCglggClb",
        "bbbbbbbbbbbb"
    };

    private Dictionary<char, GameObject> blockMapping;
    private float blockSize = 1.0f;
    private Vector3 playerSpawnPosition = Vector3.zero;
    public int AllCoins = 0;

    void Start()
    {
        InitBlockMapping();
        GenerateLevelFromMap();
    }

    void InitBlockMapping()
    {
        blockMapping = new Dictionary<char, GameObject>();

        // Маппинг символов на префабы
        if (blockGround != null) blockMapping['g'] = blockGround;
        if (blockLava != null) blockMapping['l'] = blockLava;
        if (blockBorder != null) blockMapping['b'] = blockBorder;
    }

    void SpawnBlock (char symbol, float x, float z)
    {
        GameObject prefabToSpawn = blockMapping[symbol];
        Vector3 position = new Vector3(x, prefabToSpawn.transform.localPosition.y, z);
        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }

    void GenerateLevelFromMap()
    {
        int rows = levelMap.Length;
        if (rows == 0) return;

        int cols = levelMap[0].Length;

        // Центрирование поля
        float offsetX = (cols - 1) * blockSize / 2f;
        float offsetZ = (rows - 1) * blockSize / 2f;

        for (int z = 0; z < rows; z++)
        {
            string rowString = levelMap[z];

            for (int x = 0; x < cols; x++)
            {
                char symbol = rowString[x];
                float coordX = (x * blockSize) - offsetX;
                float coordZ = (z * blockSize) - offsetZ;

                if (blockMapping.ContainsKey(symbol))
                {
                    SpawnBlock(symbol, coordX, coordZ);
                }
                // Символ "S" - респ
                else if (symbol == 'S')
                {
                    playerSpawnPosition = new Vector3(
                        coordX,
                        1.0f, // Высота над землей
                        coordZ
                    );

                    SpawnBlock('g', coordX, coordZ);
                }
                // Символа "C" - монетка на земле
                else if (symbol == 'C')
                {
                    if (coinPrefab != null)
                    {
                        // Позиция монетки
                        Vector3 coinPosition = new Vector3(
                            coordX,
                            1.0f,
                            coordZ
                        );
                        
                        // Создаем монетку
                        GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
                        coin.transform.Rotate(0, UnityEngine.Random.Range(0, 360), 0);
                        SpawnBlock('g', coordX, coordZ);
                        AllCoins++;
                    }
                    else
                    {
                        Debug.LogWarning("Coin prefab не назначен в Inspector!");
                    }
                }
                else
                {
                     Debug.LogWarning($"Неизвестный символ '{symbol}' в позиции ({x}, {z})");
                }
            }
        }

        Debug.Log($"Точка появления игрока установлена на {playerSpawnPosition}");
        GameObject player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);

        Debug.Log($"Уровень {cols}x{rows} сгенерирован!");
    }
}