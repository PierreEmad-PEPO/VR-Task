using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    static TaskManager instance;

    [SerializeField] List<Task> tasks = new List<Task>();
    int currentTaskIndex = 0;

    [SerializeField] GameObject guide;
    AudioSource audioSource;

    public static TaskManager Instance 
    { 
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType <TaskManager>();
            }
            return instance;
        }
    }

    public int RemainingTasks { get { return tasks.Count - currentTaskIndex; } }

    public Task CurrentTask
    {
        get
        {
            if (currentTaskIndex < tasks.Count) return tasks[currentTaskIndex];
            return null;
        }
    }

    private void Start()
    {
        audioSource = guide.GetComponent<AudioSource>();
        ShowTaskUI();
    }

    public bool CheckIfCurTaskDone(TaskEnum taskType, GameObject currentTool)
    {
        if (currentTaskIndex >= tasks.Count) return false;

        Task cur = tasks[currentTaskIndex];

        if (cur.TaskType == taskType && cur.RequiredTool == currentTool)
        {
            cur.OnTaskDone?.Invoke();
            currentTaskIndex++;
            ShowTaskUI();

            return true;
        }

        return false;
    }

    void ShowTaskUI()
    {
        if (currentTaskIndex >= tasks.Count) return;

        Debug.Log(tasks[currentTaskIndex].InstructionText + " " + tasks[currentTaskIndex].RequiredTool.name);
        audioSource.PlayOneShot(tasks[currentTaskIndex].InstructionMusic);
        InteractionManager.Instance.StopInteractionForSeconds(tasks[currentTaskIndex].InstructionMusic.length);
    }
    
}
