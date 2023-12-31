﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class MapInitializer : MonoBehaviour {

    public GameObject Roadway;  // Дорожное полотно, используемое в построении карты.
    public GameObject GrassSurface;  // Травяная поверхность, используемая в построении карты.
    public GameObject Car;
    public GameObject Truck;
    public GameObject CarBarrier;
    public GameObject PlayerTemplate;
    public GameObject Tree;
    public Camera PlayerCamera;
    public int CarsToTrucks = 3;
    public float CarSpeed = 10;
    public float MinCarSpawnTime = 1;
    public float MaxCarSpawnTime = 10;
    public int GenerationGap = 20;
    public float InitialPlayerZ = (float)0;
    public float StandardDeviation = 1;
    public float ExpectedValue = 0;
    // public float LineSize = 30;

    System.Random random = new System.Random();
    private List<GameObject> Roads = new List<GameObject>();
    private List<GameObject> Grasses = new List<GameObject>();
    private List<GameObject> Cars = new List<GameObject>();
    private List<GameObject> Trees = new List<GameObject>();
    private GameObject Player;
    private float RoadSize;
    private float GrassSurfaceSize;
    private float LineSize;
    public float LineLength;
    public float LineRatio;
    private int InitialFieldLength = 5;
    private int CurrentFieldLength = -5;
    public List<List<int>> Map = new List<List<int>>();  // Карта игрового поля.
    public Vector3 Spawn;


    void Start()
    {
        // CarDestruction.singleton.CarIsDeleted += DeleteCar;
        // Создаём карту.
        RoadSize = Roadway.GetComponent<Renderer>().bounds.size.z;  // Ширина дороги.
        Debug.Log("RoadSize: " + RoadSize);
        GrassSurfaceSize = GrassSurface.GetComponent<Renderer>().bounds.size.z;  // Ширина травяного покрова.
        Debug.Log("GrassSurfaceSize: " + GrassSurfaceSize);
        LineSize = Roadway.GetComponent<Renderer>().bounds.size.z;
        LineLength = Roadway.GetComponent<Renderer>().bounds.size.x;
        Debug.Log("LineLength: " + LineLength);
        LineRatio = LineLength / LineSize;
        // GenerateMap(InitialFieldLength);  // Получаем двоичную карту.
        // // Record.text;
        //
        CreateMap();


        // Делаем барьеры для автомобилей.
        GameObject LeftCarBarrier = Instantiate(CarBarrier, new Vector3(0 - GrassSurface.GetComponent<Renderer>().bounds.size.x / 2,GrassSurface.GetComponent<Renderer>().bounds.size.z / 2 , 0 + CarBarrier.GetComponent<Renderer>().bounds.size.x / 2), Quaternion.Euler(0, 0, 90));
        GameObject RightCarBarrier = Instantiate(CarBarrier, new Vector3(0 + GrassSurface.GetComponent<Renderer>().bounds.size.x / 2, GrassSurface.GetComponent<Renderer>().bounds.size.z / 2 , 0 + CarBarrier.GetComponent<Renderer>().bounds.size.x / 2), Quaternion.Euler(0, 0, 90));

        // Спавним игрока.
        Spawn = new Vector3((float)(Math.Round((LineRatio) / 2) * (LineSize)) - LineLength / 2 + LineSize / 2, PlayerTemplate.GetComponent<Renderer>().bounds.size.y / 2+1f, 0);
        Player = Instantiate(PlayerTemplate, Spawn, Quaternion.Euler(0, 0, 0));
        Player.name = "_Dude_";

        Instantiate(PlayerCamera, new Vector3(0, 40, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
        EnlargeMap();
        CleanDeletedCars();
        SetCarsSpeed();
        UpdateRecord();
    }


    int GetRandomDirection() {
        return (random.Next(0, 4) * 90);
    }

    void CreateMap() {
        for (int i = -5; i < InitialFieldLength; i++) {
            Map.Add(new List<int>());
            Grasses.Add(Instantiate(GrassSurface, new Vector3(0, 0 - GrassSurface.GetComponent<Renderer>().bounds.size.y / 2 + 1, LineSize * CurrentFieldLength), Quaternion.identity));
            // Debug.Log("Grasses.Count " + Grasses.Count);
            // Debug.Log("LineRatio " + LineRatio);
            for (int j = 0; j < LineRatio; j++) {
                if (random.Next(0, 1000000) < GetChanceOfSpawn(j - LineRatio / 2) * 1000000) {
                    Map.Last().Add(1); // Трава с деревом.
                    Trees.Add(Instantiate(Tree, new Vector3(LineSize * j - LineLength / 2 + LineSize / 2, 0, LineSize * CurrentFieldLength), Quaternion.Euler(0, GetRandomDirection(), 0)));
                } else {
                    Map.Last().Add(0); // Трава без дерева.
                }
            }
            foreach (int number in Map.Last()) {
                Debug.Log(number);
            }
            CurrentFieldLength++;
        }
    }

    void EnlargeMap() {

        if (Map.Count - ((PlayerPrefs.GetInt("CurrentZPosition"))) < GenerationGap) {
            // Debug.Log("EnlargeMap");

            int k = random.Next(0, 2);

            Map.Add(new List<int>());
            if (k == 1) {  // Дорога.
                for (int i = 0; i < LineRatio; i++) {
                    Map.Last().Add(2);
                }
                Roads.Add(Instantiate(Roadway, new Vector3(0, 0 - Roadway.GetComponent<Renderer>().bounds.size.y / 2, LineSize * CurrentFieldLength), Quaternion.identity));
                // Рандомно ставим дороге имя, исходя их которого будет определяться направление движения машин.
                switch (random.Next(0, 2)) {
                    case 0: Roads.Last().name = "Road" + CurrentFieldLength + "_ToRight"; break;
                    case 1: Roads.Last().name = "Road" + CurrentFieldLength + "_ToLeft";  break;
                }
                StartCarSpawner(Roads.Last());
            } else
            if (k == 0) {  // Трава.
                // Создаём травяную поверхность.
                Grasses.Add(Instantiate(GrassSurface, new Vector3(0, 0 - GrassSurface.GetComponent<Renderer>().bounds.size.y / 2 + 1, LineSize * CurrentFieldLength), Quaternion.identity));
                Grasses.Last().name = "Grass" + CurrentFieldLength;
                // Создаём деревья на дороге, используя нормальное распределение.
                for (int j = 0; j < LineRatio; j++) {
                    if (random.Next(0, 1000000) < GetChanceOfSpawn(j - LineRatio / 2) * 1000000) {
                        Map.Last().Add(1); // Трава с деревом.
                        Trees.Add(Instantiate(Tree, new Vector3(LineLength / LineRatio * j - LineLength / 2 + LineLength / LineRatio / 2, 0, LineSize * CurrentFieldLength), Quaternion.Euler(0, GetRandomDirection(), 0)));
                    } else {
                        Map.Last().Add(0); // Трава без дерева.
                    }
                }
            }
            CurrentFieldLength++;
        }
    }


    void StartCarSpawn() {
        foreach (GameObject Road in Roads) {
            StartCoroutine(CarCoroutine(Road));
        }
    }

    void StartCarSpawner(GameObject Road) {
        StartCoroutine(CarCoroutine(Road));
    }

    IEnumerator CarCoroutine(GameObject Road) {
        while (true) {
            // Debug.Log("Road " + Road.transform.position.z / LineSize + " Player " + PlayerPrefs.GetInt("CurrentZPosition"));

            if (Road.transform.position.z / LineSize > PlayerPrefs.GetInt("CurrentZPosition") - 3) {
                // Исходя из имени дороги определяем, в какую сторону смотрят машины.
                if (Road.name.Contains("_ToRight")) {  // Машины едут направо.
                    // Случайно определяем, будет машина грузовиком или легковушкой.
                    if (random.Next(0, CarsToTrucks) == 0) {  // Легковушка.
                        Cars.Add(Instantiate(Truck, new Vector3(0 - Road.GetComponent<Renderer>().bounds.size.x / 2 + Truck.GetComponent<Renderer>().bounds.size.x + 25, Truck.GetComponent<Renderer>().bounds.size.y + 2.5f, Road.transform.position.z), Quaternion.Euler(0, 90, 0)));
                    } else {  // Грузовик.
                        Cars.Add(Instantiate(Car, new Vector3(0 - Road.GetComponent<Renderer>().bounds.size.x / 2 + Car.GetComponent<Renderer>().bounds.size.x + 10, Car.GetComponent<Renderer>().bounds.size.y + 1.5f, Road.transform.position.z), Quaternion.Euler(0, 0, 0)));
                    }
                    Cars.Last().name = "Car_ToRight";
                } else
                if (Road.name.Contains("_ToLeft")) {
                    // Случайно определяем, будет машина грузовиком или легковушкой.
                    if (random.Next(0, CarsToTrucks) == 0) {  // Грузовик.
                        Cars.Add(Instantiate(Truck, new Vector3(0 + Road.GetComponent<Renderer>().bounds.size.x / 2 - Truck.GetComponent<Renderer>().bounds.size.x - 25, Truck.GetComponent<Renderer>().bounds.size.y + 2.5f, Road.transform.position.z), Quaternion.Euler(0, 270, 0)));
                    } else {  // Легковушка.
                        Cars.Add(Instantiate(Car, new Vector3(0 + Road.GetComponent<Renderer>().bounds.size.x / 2 - Car.GetComponent<Renderer>().bounds.size.x - 10, Car.GetComponent<Renderer>().bounds.size.y + 1.5f, Road.transform.position.z), Quaternion.Euler(0, 180, 0)));
                    }
                    Cars.Last().name = "Car_ToLeft";
                }


                // switch (MovementDirection) {
                //     case 0: Cars.Last().name = "Car_ToRight"; break;
                //     case 180: Cars.Last().name = "Car_ToLeft"; break;
                // }
            }
            // Cars.Last().name = "Car" + i;
            float delay = UnityEngine.Random.Range(MinCarSpawnTime, MaxCarSpawnTime);
            yield return new WaitForSeconds(delay);
        }
    }


    // void GenerateMap(int MapSize = 10) {
    //     for (int i = 0; i < MapSize; i++) {
    //         int kek = random.Next(0, 2);
    //         // Debug.Log(kek);
    //         Map.Add(kek);
    //     }
    // }

    void CleanDeletedCars() {
        for (int i = 0; i < Cars.Count; i++) {
            if (Cars[i] == null) {
                Cars.Remove(Cars[i]);
                // Debug.Log("Car is deleted");
            }
        }
        // Debug.Log(Cars.Count + " Машин на карте в данный момент");
    }

    public void DeleteCar(GameObject Car) {
        Cars.Remove(Car);
    }

    void SetCarsSpeed() {
        for (int i = 0; i < Cars.Count; i++) {
            if (Cars[i] != null) {
                if (Cars[i].name.Contains("_ToRight")) {
                    // Cars[i].GetComponent<Rigidbody>().velocity = new Vector3(CarSpeed + (float)Math.Round(Cars[i].transform.position.z / LineSize), 0, 0);
                    Cars[i].GetComponent<Rigidbody>().velocity = new Vector3(CarSpeed, 0, 0);
                } else
                if (Cars[i].name.Contains("_ToLeft")) {
                    // Cars[i].GetComponent<Rigidbody>().velocity = new Vector3(-CarSpeed - Cars[i].transform.position.z / LineSize, 0, 0);
                    Cars[i].GetComponent<Rigidbody>().velocity = new Vector3(-CarSpeed, 0, 0);
                }
            } else
            if (Cars[i] == null) {
                // Debug.Log("Moving deleted car");
                // Судя по всему, в обработке удалённой машины нет ничего критичного.
            }
        }
    }

    void UpdateRecord() {
        // Debug.Log("Grasses.Count " + Grasses.Count);
        if (Input.GetKeyDown(KeyCode.R)) {
            PlayerPrefs.SetInt("Record", 0);
            Debug.Log("Рекорд сброшен");
        }
    }


    double NormalDistribution(float x) {
        return (1 / (System.Math.Sqrt(2 * System.Math.PI) * StandardDeviation) * System.Math.Pow(System.Math.E, -System.Math.Pow(x - ExpectedValue, 2) / (2 * System.Math.Pow(StandardDeviation, 2))));
    }

    double GetChanceOfSpawn(float x) {
        return (1 - NormalDistribution(x) / NormalDistribution(ExpectedValue));
    }

}