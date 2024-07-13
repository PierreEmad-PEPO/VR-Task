using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Task
{
    [SerializeField] string instructionText;
    [SerializeField] AudioClip instructionMusic;
    [SerializeField] GameObject requiredTool;
    [SerializeField] TaskEnum taskType;
    [SerializeField] UnityEvent onTaskDone;
    public string InstructionText { get { return instructionText; } }
    public AudioClip InstructionMusic { get { return instructionMusic; } }
    public GameObject RequiredTool { get { return requiredTool; } }
    public TaskEnum TaskType { get { return taskType; } }
    public UnityEvent OnTaskDone {  get { return onTaskDone; } }

}
