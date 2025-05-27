using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallIncteractable : XRSimpleInteractable
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;
    [SerializeField] private GameObject wallCubePrefab;
    [SerializeField] private GameObject socketWallPrefab;
    [SerializeField] private XRSocketInteractor wallSocket;
    [SerializeField] int socketPosistion;
    [SerializeField] private GameObject[] wallCubes;
    [SerializeField] private float cubeSpacing = 0.005f;

    private Vector3 cubeSize;
    private Vector3 spawnPosition;

    private void Start()
    {
        if(wallCubePrefab != null)
        {
            cubeSize = wallCubePrefab.GetComponent<Renderer>().bounds.size;
        }        
        spawnPosition = transform.position;
        BuildWall();
    }

    private void BuildWall()
    {
        for (int i = 0; i < columns; i++)
        {
            GenerateColoumn(rows, true);
            spawnPosition.x += cubeSize.x + cubeSpacing;
        }
    }

    private void GenerateColoumn(int height, bool socketed)
    {
        spawnPosition.y = transform.position.y;
        wallCubes = new GameObject[height];
        for (int i = 0; i < wallCubes.Length; i++)
        {
            wallCubes[i] = Instantiate(wallCubePrefab, spawnPosition, transform.rotation);
            if (i == 0)
            {
                wallCubes[i].name = "column_" + i.ToString();
                wallCubes[i].transform.SetParent(transform);
            }
            else
            {
                wallCubes[i].transform.SetParent(wallCubes[0].transform);
            }
                spawnPosition.y += cubeSize.y + cubeSpacing;
        }     
                
        if ( socketed && socketWallPrefab != null)
        {
            if(socketPosistion < 0 || socketPosistion >= height) 
            {
                socketPosistion = 0;
            }
            if (wallCubes[socketPosistion] != null)
            {
                Vector3 pos = wallCubes[socketPosistion].transform.position; 
                DestroyImmediate(wallCubes[socketPosistion]);
                wallCubes[socketPosistion] = Instantiate(socketWallPrefab, pos, transform.rotation);
                if(socketPosistion == 0)
                {
                    wallCubes[socketPosistion].transform.SetParent(transform);
                }
                else
                {
                    wallCubes[socketPosistion].transform.SetParent(wallCubes[0].transform);
                }
                    wallSocket = wallCubes[socketPosistion].GetComponentInChildren<XRSocketInteractor>();
                if (wallSocket != null)
                {
                    wallSocket.selectEntered.AddListener(OnWallScoketEnetered);
                    wallSocket.selectExited.AddListener(OnWallSocketExited);
                }
            }            
        }
    }

    private void OnWallScoketEnetered(SelectEnterEventArgs arg)
    {
        foreach(GameObject wall in wallCubes)
        {
            Rigidbody rb = wall.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }

    private void OnWallSocketExited(SelectExitEventArgs arg) 
    {
        foreach (GameObject wall in wallCubes)
        {
            Rigidbody rb = wall.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
