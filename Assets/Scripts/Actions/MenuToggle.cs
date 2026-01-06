using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuToggle : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject mainPanel;
    public GameObject taskPanel;

    [Header("Lista de tareas")]
    public Transform taskListParent;
    public GameObject taskPrefab;

    [Header("Input")]
    public InputActionAsset inputActions;

    private InputAction toggleMenuAction;
    private List<GameObject> activeTasks = new List<GameObject>();

    void Start()
    {
    }

    void OnDisable()
    {
        if (toggleMenuAction != null)
            toggleMenuAction.Disable();
    }

    public void ShowTasks()
    {
        mainPanel.SetActive(false);
        taskPanel.SetActive(true);
    }

    public void ShowMain()
    {
        mainPanel.SetActive(true);
        taskPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        bool isVisible = mainPanel.activeSelf || taskPanel.activeSelf;
        mainPanel.SetActive(!isVisible);
        taskPanel.SetActive(false);
    }

    public void AddTask(string taskText)
    {
        GameObject taskItem = Instantiate(taskPrefab, taskListParent);
        taskItem.GetComponent<Text>().text = taskText;
        activeTasks.Add(taskItem);
    }
}
