using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public GameObject[] roomPositions; 
    public Canvas[] roomCanvases; 
    public Canvas initialCanvas; 
    public GameObject ExitButton;
    public Slider transitionSpeedSlider; 

    private int Counter = -1; 
    private float transitionSpeed = 2f;

    private void Start()
    {
        DisableAllCanvases();
        ExitButton.SetActive(true); 
        initialCanvas.gameObject.SetActive(true); 

        
        transitionSpeedSlider.onValueChanged.AddListener(UpdateTransitionSpeed);
    }

    private void DisableAllCanvases()
    {
        foreach (Canvas canvas in roomCanvases)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActualStart()
    {
        Counter = 0; 
        MoveCamera();
    }

    private void UpdateTransitionSpeed(float value)
    {
        transitionSpeed = value;
    }

    private void MoveCamera()
    {
        if (Counter >= 0 && Counter < roomPositions.Length && roomPositions[Counter] != null)
        {
            transform.DOMove(roomPositions[Counter].transform.position, transitionSpeed).SetEase(Ease.InOutSine);
            transform.rotation = roomPositions[Counter].transform.rotation; 

            ActivateCanvas(Counter);
        }
    }

    private void ActivateCanvas(int index)
    {
        if (index >= 0 && index < roomCanvases.Length)
        {
            DisableAllCanvases();
            roomCanvases[index].gameObject.SetActive(true);
        }
    }

    public void NextScene()
    {
        Counter++;
        if (Counter >= roomPositions.Length)
        {
            Counter = 0; 
            MoveCamera();
            initialCanvas.gameObject.SetActive(true); 
           
            foreach (Canvas canvas in roomCanvases)
            {
                if (canvas != initialCanvas)
                {
                    canvas.gameObject.SetActive(false);
                }
            }


            RestartApplication();
        }
        else
        {
            MoveCamera();
        }

        
        ExitButton.SetActive(true);
    }

    public void Previous()
    {
        Counter--;
        if (Counter < 0)
        {
            Counter = roomPositions.Length - 1; 
        }
        MoveCamera();
    }

    public void ExitApp()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void RestartApplication()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
