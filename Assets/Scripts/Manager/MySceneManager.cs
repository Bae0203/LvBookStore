using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct EndWeekScene
{
    public GameObject EndWeekUI;
    public TextMeshProUGUI EndWeekMoney;
}

[Serializable]
public struct NormalEventLoadingImage
{
    public GameObject NormalEventUI;
    public TextMeshProUGUI NormalEventText;
    public Image NormalEventImage;
    public Sprite LoadingImage1;
    public Sprite LoadingImage2;
}

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance {
        get {
            return instance;
        }
    }
    
    private static MySceneManager instance;
    
    // public GameObject DialogueUI;
    // public GameObject StoreUI;
    
    public Image Fade_img;
    private float time = 0;
    public float F_time = 1f;

    public NormalEventLoadingImage NormalEventImage;
    public EndWeekScene EndWeekSceneInfo;

    public TextMeshProUGUI LoadingDateText;
    
    private void Awake()
    {
        if (instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        NormalEventImage.NormalEventUI.SetActive(false);
    }
    
    public void LoadNormalEventScene()
    {
        StartCoroutine(NormalEventLoad());
    }

    public void RunNextDateScene()
    {
        StartCoroutine(LoadingNextDate());
    }

    IEnumerator NormalEventLoad()
    {
        StartFadeIn();
        yield return new WaitForSeconds(1.0f);
        NormalEventImage.NormalEventUI.SetActive(true);
        NormalEventImage.NormalEventText.text = ".";
        NormalEventImage.NormalEventImage.sprite = NormalEventImage.LoadingImage1;
        yield return new WaitForSeconds(0.3f);
        NormalEventImage.NormalEventText.text = ". .";
        NormalEventImage.NormalEventImage.sprite = NormalEventImage.LoadingImage2;
        yield return new WaitForSeconds(0.3f);
        NormalEventImage.NormalEventText.text = ". . .";
        NormalEventImage.NormalEventImage.sprite = NormalEventImage.LoadingImage1;
        yield return new WaitForSeconds(0.3f);
        NormalEventImage.NormalEventUI.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        StartFadeOut();
    }
    
    IEnumerator LoadingNextDate()
    {
        StartFadeIn();
        yield return new WaitForSeconds(1.0f);
        if (GameManager.instance.Date  % 5 == 0 && GameManager.instance.Date > 0)
        {
            yield return new WaitForSeconds(1.0f);
            EndWeekSceneInfo.EndWeekMoney.text = String.Format("+ {0}원", GameManager.instance.CalculateWeekMoney());
            EndWeekSceneInfo.EndWeekUI.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            EndWeekSceneInfo.EndWeekUI.SetActive(false);
            yield return new WaitForSeconds(1.0f);
        }
        LoadingDateText.text = String.Format("{0} 일차", GameManager.instance.Date+1);
        StartCoroutine(DateFadeIn());
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(DateFadeOut());
        yield return new WaitForSeconds(1.0f);
        StartFadeOut();
    }
    
    public void LoadScene()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        StartFadeIn();
        yield return new WaitForSeconds(1.0f);
        StartFadeOut();
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }
    
    public void StartFadeOut()
    {
        Debug.Log("dfdfd");
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        Fade_img.gameObject.SetActive(true);
        time = 0f;

        Color alpha = Fade_img.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0,1, time);
            Fade_img.color = alpha;
            yield return null;
        }

        time = 0f;
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        Fade_img.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Fade_img.color;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1,0, time);
            Fade_img.color = alpha;
            yield return null;
        }
        time = 0f;
        Fade_img.gameObject.SetActive(false);
        yield return null;

    }
    IEnumerator DateFadeIn()
    {
        LoadingDateText.gameObject.SetActive(true);
        time = 0f;

        Color alpha = LoadingDateText.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0,1, time);
            LoadingDateText.color = alpha;
            yield return null;
        }

        time = 0f;
    }
    IEnumerator DateFadeOut()
    {
        yield return new WaitForSeconds(1f);
        LoadingDateText.gameObject.SetActive(true);
        time = 0f;
        Color alpha = LoadingDateText.color;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1,0, time);
            LoadingDateText.color = alpha;
            yield return null;
        }
        time = 0f;
        LoadingDateText.gameObject.SetActive(false);
        yield return null;

    }
    
}