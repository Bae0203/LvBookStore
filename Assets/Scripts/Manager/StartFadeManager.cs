using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFadeManager : MonoBehaviour
{
    public static StartFadeManager Instance {
        get {
            return instance;
        }
    }
    
    private static StartFadeManager instance;

    
    public Image Fade_img;
    private float time = 0;
    public float F_time = 1f;
    private void Awake()
    {
        if (instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
        Fade_img.gameObject.SetActive(false);
    }
    
    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
        // Fade_img.gameObject.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        Fade_img.gameObject.SetActive(true);
        time = 0f;
        Color alpha = new Color();
        alpha.a = 0;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0,1, time);
            Fade_img.color = alpha;
            yield return null;
        }

        time = 0f;
    }
    
}
