using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;
using System.Xml.Serialization;

public class Guide : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Vector3 tar;
    Animator animator;

    float elapsedTime, totalTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        animator = GetComponent<Animator>();
        UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < totalTime) elapsedTime += Time.deltaTime;
        

        if (TaskManager.Instance.CurrentTask != null && TaskManager.Instance.CurrentTask.TaskType == TaskEnum.Teleport) 
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        tar.y = transform.position.y;

        if (elapsedTime >=  totalTime) 
        {
            transform.LookAt(tar);
            elapsedTime = 0;
        }

        if (Vector3.Distance(transform.position, tar + Vector3.forward) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, tar + Vector3.forward, moveSpeed * Time.deltaTime);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        tar = TaskManager.Instance.CurrentTask.RequiredTool.transform.position;
    }

    public void Clap()
    {
        animator.SetBool("Clap", true);
    }
}
