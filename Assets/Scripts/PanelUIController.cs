using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanelUIController : MonoBehaviour
{
    [SerializeField] private string[] msgStrings;
    [SerializeField] private TextMeshProUGUI msgText;
    [SerializeField] private GameObject keyPointLight;
    [SerializeField] private XRButtonInteractable startButton;

    private void Start()
    {
        msgText.text = msgStrings[0];
        startButton.selectEntered.AddListener(OnGameStart);
    }

    private void OnGameStart(SelectEnterEventArgs arg)
    {
        msgText.text = msgStrings[1];
        keyPointLight.SetActive(true);
    }
}
