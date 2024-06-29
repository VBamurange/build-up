using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public GameObject[] positions;
    public Canvas[] canvases;
    public Canvas firstCanvas;
    public GameObject ExitButton;
    public Slider SpeedSlider;

    private int Counter = -1;
    private float transitionSpeed = 5f;

    private void Start()
    {
        SetInitialState();
        SpeedSlider.onValueChanged.AddListener(UpdateTransitionSpeed);
    }

    private void SetInitialState()
    {
        HideAllCanvases();
        ExitButton.SetActive(true);
        firstCanvas.gameObject.SetActive(true);
    }

    private void HideAllCanvases()
    {
        foreach (var canvas in canvases)
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
        TransitionCamera();
    }

    private void UpdateTransitionSpeed(float value)
    {
        transitionSpeed = value;
    }

    private void TransitionCamera()
    {
        if (IsValidCounter())
        {
            var targetPosition = positions[Counter].transform.position;
            var targetRotation = positions[Counter].transform.rotation;
            transform.DOMove(targetPosition, transitionSpeed).SetEase(Ease.InOutSine);
            transform.rotation = targetRotation;

            ShowCanvas(Counter);
        }
    }

    private bool IsValidCounter()
    {
        return Counter >= 0 && Counter < positions.Length && positions[Counter] != null;
    }

    private void ShowCanvas(int index)
    {
        if (index >= 0 && index < canvases.Length)
        {
            HideAllCanvases();
            canvases[index].gameObject.SetActive(true);
        }
    }

    public void NextScene()
    {
        Counter++;
        if (Counter >= positions.Length)
        {
            ResetToFirstScene();
        }
        else
        {
            TransitionCamera();
        }

        ExitButton.SetActive(true);
    }

    private void ResetToFirstScene()
    {
        Counter = 0;
        TransitionCamera();
        firstCanvas.gameObject.SetActive(true);

        foreach (var canvas in canvases)
        {
            if (canvas != firstCanvas)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        RestartApplication();
    }

    public void Previous()
    {
        Counter--;
        if (Counter < 0)
        {
            Counter = positions.Length - 1;
        }
        TransitionCamera();
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
