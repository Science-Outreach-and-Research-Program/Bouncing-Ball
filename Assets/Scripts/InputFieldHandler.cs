using UnityEngine;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    public static int inputOffsetTimes = 10;

    public void OnBallHeightChange()
    {
        float ballHeight;

        if (float.TryParse(gameObject.GetComponent<InputField>().text, out ballHeight))
            ballHeight = Mathf.Min(Mathf.Max(0.2f, ballHeight), 3);
        else
            ballHeight = 0.2f;

        GameObject.Find("Ruler").transform.localScale = new Vector3(1, ballHeight * inputOffsetTimes / 2, 1);
        
        GameObject.Find("StartingHeightText").GetComponent<Text>().text = string.Format("Bounce Height: {0:N1} m", ballHeight);
        ball.GetComponent<BallManager>().ballHeight = ballHeight * inputOffsetTimes;
        ball.GetComponent<BallManager>().Init();
    }

    public void OnBallHeightAction()
    {
        gameObject.GetComponent<InputField>().text = (ball.GetComponent<BallManager>().ballHeight / inputOffsetTimes).ToString();
    }
}
