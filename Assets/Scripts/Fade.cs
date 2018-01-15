using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField]
    CanvasGroup group;

    public IEnumerator FadeIn()
    {
        while (group.alpha < 1)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            group.alpha += 0.01f;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}