using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI goalTextObj;
    public TextMeshProUGUI promptTextObj;
    public GameObject goalPanel;

    public Transform onScreenPos;
    public Transform offScreenPos;

    public ProgressBar buttonHoldProgBar;
    public bool goalShowing;

    private bool m_ShowGoal;
    private bool m_HideGoal;
    private float m_SpeedMod;

    void Start()
    {
        goalPanel.transform.position = offScreenPos.position;
        goalShowing = false;
        goalTextObj.text = SetGoalText(0);
        buttonHoldProgBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_SpeedMod = 1;
            m_ShowGoal = true;
            m_HideGoal = false;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            m_SpeedMod = 1;
            m_ShowGoal = false;
            m_HideGoal = true;
        }

        if (m_ShowGoal)
        {
            ShowGoal();
        }
        if (m_HideGoal)
        {
            HideGoal();
        }
    }

    public void UpdateButtonHoldProgBar(float percentage)
    {
        buttonHoldProgBar.currentPercent = percentage;
        if (buttonHoldProgBar.currentPercent == 0)
        {
            buttonHoldProgBar.gameObject.SetActive(false);
        }
        else
        {
            buttonHoldProgBar.gameObject.SetActive(true);
        }
    }

    public void ShowButtonPrompt(string promptText)
    {
        promptTextObj.text = promptText;
    }

    private void ShowGoal()
    {
        if (Vector3.Distance(goalPanel.transform.position, onScreenPos.transform.position) > Vector3.kEpsilon)
        {
            // If not in position yet then keep on moving
            m_SpeedMod = m_SpeedMod * 1.1f;
            goalPanel.transform.position = Vector3.MoveTowards(goalPanel.transform.position, onScreenPos.transform.position, 8 * m_SpeedMod);
        }
        else
        {
            m_ShowGoal = false;
            goalShowing = true;
        }
    }

    private void HideGoal()
    {
        if (Vector3.Distance(goalPanel.transform.position, offScreenPos.transform.position) > Vector3.kEpsilon)
        {
            m_SpeedMod = m_SpeedMod * 1.1f;
            goalPanel.transform.position = Vector3.MoveTowards(goalPanel.transform.position, offScreenPos.transform.position, 8 * m_SpeedMod);
        }
        else
        {
            m_HideGoal = false;
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
