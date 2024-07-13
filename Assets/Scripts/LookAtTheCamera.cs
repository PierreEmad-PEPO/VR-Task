using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LookAtTheCamera : MonoBehaviour
{
    [SerializeField] Text remainingTasks;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        remainingTasks.text = TaskManager.Instance.RemainingTasks + " Tasks Remaining";
    }
}
