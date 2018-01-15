using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject instructions;
    [SerializeField]
    CanvasGroup group;

    AudioSource buttonSound;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        buttonSound = GetComponent<AudioSource>();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenInstructions()
    {
        StartCoroutine(Instructions());
    }

    private IEnumerator Instructions()
    {
        group.alpha = 0;
        instructions.SetActive(true);

        while (group.alpha < 1)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            group.alpha += 0.04f;
        }
    }

    public void CloseInstructions()
    {
        StartCoroutine(Back());
    }

    private IEnumerator Back()
    {
        group.alpha = 1;
        instructions.SetActive(true);

        while (group.alpha > 0)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            group.alpha -= 0.04f;
        }
        instructions.SetActive(false);
    }

    public void ButtonSound()
    {
        if (!buttonSound.isPlaying)
            buttonSound.Play();
    }

    public void Exit()
    {
        Application.Quit();
    }
}