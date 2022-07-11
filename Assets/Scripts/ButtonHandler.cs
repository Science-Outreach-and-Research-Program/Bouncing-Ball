using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject ruler;

    public void StartMeasurement()
    {
        ruler.SetActive(true);
        GameObject.Find("StartDropButton").GetComponent<LeanButton>().interactable = true;
        GameObject.Find("StartingHeightInputField").GetComponent<InputField>().interactable = true;
    }

    public void StartBounceBall()
    {
        ball.GetComponent<BallManager>().StartBounce();
    }
    
    public void DrawBestFitLine()
    {
        GameObject.Find("ScatterChart").GetComponent<ChartManager>().DrawBestFitLine();
    }
}
