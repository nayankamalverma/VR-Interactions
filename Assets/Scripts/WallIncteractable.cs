using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallIncteractable : XRSimpleInteractable
{
    [SerializeField] private XRSocketInteractor wallSocket;
    [SerializeField] private GameObject[] wallCubes;

    private void Start()
    {
        if (wallSocket != null)
        {
            wallSocket.selectEntered.AddListener(OnWallScoketEnetered);
            wallSocket.selectExited.AddListener(OnWallSocketExited);
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
