using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class WallIncteractable : XRSimpleInteractable
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;
    [SerializeField] private GameObject wallCubePrefab;
    [SerializeField] private GameObject socketWallPrefab;
    [SerializeField] private List<GeneratedColumn> generatedColumns;

    [SerializeField] private int socketPosistion;
    [SerializeField] private float cubeSpacing = 0.005f;
    [SerializeField] bool buildWall = false;
    [SerializeField] bool deleteWall = false;
    [SerializeField] bool destroyWall = false;
    [SerializeField] int destroyWallPower = 2000;

    private XRSocketInteractor wallSocket;
    private GameObject[] wallCubes;
    private Vector3 cubeSize;
    private Vector3 cubeSpawnPosition;
    private Vector3 wallSpawnPosition;

    private void Start()
    {

    }

    private void AddSocketListeners()
    {
        wallSocket.selectEntered.AddListener(OnWallSocketEntered);
        wallSocket.selectExited.AddListener(OnWallSocketExited);
    }

    private void BuildWall()
    {
        if (wallCubePrefab != null)
        {
            cubeSize = wallCubePrefab.GetComponent<Renderer>().bounds.size;
        }
        wallSpawnPosition = cubeSpawnPosition = transform.position;
        int socketedColumn = Random.Range(0, columns);
        for (int i = 0; i < columns; i++)
        {
            if (i == socketedColumn) GenerateColumn(i, rows, true);
            else GenerateColumn(i, rows, false);
            cubeSpawnPosition.x += cubeSize.x + cubeSpacing;
        }
        transform.position = wallSpawnPosition;
    }

    private void GenerateColumn(int index, int height, bool isSocketed)
    {
        GeneratedColumn tempColumn = new GeneratedColumn();
        tempColumn.InitColumn(transform, index, height, isSocketed);
        cubeSpawnPosition.y = transform.position.y;
        wallCubes = new GameObject[height];
        for (int i = 0; i < wallCubes.Length; i++)
        {
            wallCubes[i] = Instantiate(wallCubePrefab, cubeSpawnPosition, transform.rotation);
            tempColumn.SetCube(wallCubes[i]);
            cubeSpawnPosition.y += cubeSize.y + cubeSpacing;
        }
        if (isSocketed && socketWallPrefab != null)
        {
            if (socketPosistion < 0 || socketPosistion >= height)
            {
                socketPosistion = 0;
            }
            AddSocketWall(tempColumn);
        }
        generatedColumns.Add(tempColumn);
    }

    private void AddSocketWall(GeneratedColumn socketedColumn)
    {
        if (wallCubes[socketPosistion] != null)
        {
            Vector3 pos = wallCubes[socketPosistion].transform.position;
            DestroyImmediate(wallCubes[socketPosistion]);
            wallCubes[socketPosistion] = Instantiate(socketWallPrefab, pos, transform.rotation);
            socketedColumn.SetCube(wallCubes[socketPosistion]);
            if (socketPosistion == 0)
            {
                wallCubes[socketPosistion].transform.SetParent(transform);
            }
            else
            {
                wallCubes[socketPosistion].transform.SetParent(wallCubes[0].transform);
            }
            wallSocket = wallCubes[socketPosistion].GetComponentInChildren<XRSocketInteractor>();
            AddSocketListeners();
        }
    }

    private void OnWallSocketEntered(SelectEnterEventArgs arg)
    {
        if (generatedColumns.Count >= 1)
        {
            foreach (GeneratedColumn generatedColumn in generatedColumns)
            {
                generatedColumn.DestroyColumn(destroyWallPower);
            }
        }
    }

    private void OnWallSocketExited(SelectExitEventArgs arg)
    {
        if (generatedColumns.Count >= 1)
        {
            foreach (GeneratedColumn generatedColumn in generatedColumns)
            {
                generatedColumn.ResetColumn();
            }
        }
    }

    private void Update()
    {
        if (buildWall)
        {
            buildWall = false;
            BuildWall();
        }
        if (deleteWall)
        {
            deleteWall = false;
            for (int i = 0; i < generatedColumns.Count; i++)
            {
                generatedColumns[i].DeleteColumn();
            }
            generatedColumns.Clear();
        }
    }
}

[System.Serializable]
public class GeneratedColumn
{
    [SerializeField] private GameObject[] wallCubes;
    [SerializeField] private bool isSocketed;
    private bool isParented;
    private Transform parenTransform;
    private Transform columnTransform;
    private const string columnName = "Column";
    private int index;

    public void InitColumn(Transform parent, int index, int rows, bool isSocketed)
    {
        parenTransform = parent;
        wallCubes = new GameObject[rows];
        this.isSocketed = isSocketed;
        this.index = index;
    }

    public void SetCube(GameObject cube)
    {
        for (int i = 0; i < wallCubes.Length; i++)
        {
            if (!isParented)
            {
                isParented = true;
                cube.name = (!isSocketed) ? columnName + index : "Socketed" + columnName;
                cube.transform.SetParent(parenTransform);
                columnTransform = cube.transform;
            }
            else
            {
                cube.transform.SetParent(columnTransform);
            }

            if (wallCubes[i] == null)
            {
                wallCubes[i] = cube;
                break;
            }
        }
    }

    public void DeleteColumn()
    {
        foreach (GameObject cube in wallCubes)
        {
            Object.DestroyImmediate(cube);
        }

        wallCubes = Array.Empty<GameObject>();
    }

    public void DestroyColumn(int pow)
    {
        foreach (GameObject wall in wallCubes)
        {
            Rigidbody rb = wall.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            wall.transform.SetParent(parenTransform);
            rb.AddRelativeForce(Random.onUnitSphere * pow);
        }
    }

    public void ResetColumn()
    {
        foreach (GameObject wall in wallCubes)
        {
            Rigidbody rb = wall?.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}