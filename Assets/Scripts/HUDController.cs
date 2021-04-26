using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI goalText;
    public GameObject goalPanel;

    public Transform onScreenPos;
    public Transform offScreenPos;

    public bool goalShowing;

    private bool showGoal;
    private bool hideGoal;
    private float speedMod;

    void Start()
    {
        goalPanel.transform.position = offScreenPos.position;
        goalShowing = false;
        goalText.text = SetGoalText(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            speedMod = 1;
            showGoal = true;
            hideGoal = false;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            speedMod = 1;
            showGoal = false;
            hideGoal = true;
        }

        if (showGoal)
        {
            ShowGoal();
        }
        if (hideGoal)
        {
            HideGoal();
        }
    }

    private void ShowGoal()
    {
        if (Vector3.Distance(goalPanel.transform.position, onScreenPos.transform.position) > Vector3.kEpsilon)
        {
            // If not in position yet then keep on moving
            speedMod = speedMod * 1.1f;
            goalPanel.transform.position = Vector3.MoveTowards(goalPanel.transform.position, onScreenPos.transform.position, 8 * speedMod);
        }
        else
        {
            showGoal = false;
            goalShowing = true;
        }
    }

    private void HideGoal()
    {
        if (Vector3.Distance(goalPanel.transform.position, offScreenPos.transform.position) > Vector3.kEpsilon)
        {
            speedMod = speedMod * 1.1f;
            goalPanel.transform.position = Vector3.MoveTowards(goalPanel.transform.position, offScreenPos.transform.position, 8 * speedMod);
        }
        else
        {
            hideGoal = false;
            goalShowing = false;
        }
    }

    private string SetGoalText(int levIndex = 0)
    {
        string resultText = "";
        switch (levIndex)
        {
            case 0:
                resultText = "Plant the first disruptor.";
                break;
            default:
                resultText = "No goal set.";
                break;
        }
        return resultText;
    }
}
