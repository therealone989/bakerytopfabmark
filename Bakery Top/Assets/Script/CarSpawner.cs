﻿using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;  // Das Prefab des Autos
    public Transform spawnPoint;  // Der Punkt, an dem die Autos erscheinen sollen
    public float spawnInterval = 8f;  // Intervall in Sekunden, wie oft ein Auto spawnt
    public bool isLeft; // Gibt an, ob das Auto nach links fahren soll
    public bool randomScale = false;

    private void Start()
    {
        InvokeRepeating(nameof(B), 0f, spawnInterval);  // Autos spawnen alle X Sekunden
    }

    private void A(bool isLeft)
    {
        Quaternion rotation = isLeft ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        Instantiate(carPrefab, spawnPoint.position, rotation);

        if (randomScale)
        {
            float randomScaleFactor = Random.Range(0.5f, 2f);  // Skalierung zwischen 0.5 und 2
            carPrefab.transform.localScale = new Vector3(randomScaleFactor, randomScaleFactor, randomScaleFactor);
        }
    }

    private void B()
    {
        A(isLeft); // Ruft A() mit dem gesetzten isLeft-Wert auf
    }
}
