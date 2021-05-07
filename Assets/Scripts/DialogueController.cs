using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueController : MonoBehaviour
{
    private static DialogueController _instance;
    public static DialogueController Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private Conversation nextConversation;
    private int nextConversationIndex;

    void Start()
    {
        nextConversationIndex = 0;
        SetNextConversation();
    }

    void Update()
    {        
    }

    public void SetNextConversation()
    {
        List<Conversation> allConversations = DialogueManager.databaseManager.defaultDatabase.conversations;
        nextConversation = allConversations[nextConversationIndex];
        nextConversationIndex++;
    }

    public void StartNextConversation()
    {        
        DialogueManager.StartConversation(nextConversation.Title);
    }
}
