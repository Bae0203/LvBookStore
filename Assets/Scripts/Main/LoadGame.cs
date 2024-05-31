using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct LoadingImage
{
    public Image Image;
    
    public Sprite RightIdle;
    public Sprite Right;
    public Sprite RightRun;
    public Sprite LeftIdle;
    public Sprite Left;
    public Sprite LeftRun;
}
public class LoadGame : MonoBehaviour
{
    public LoadingImage LoadingImage;
    public GameObject StoreScene;
    public GameObject StartScene;
    
    private void Start()
    {
        LoadingImage.Image.gameObject.SetActive(false);
        StoreScene.SetActive(false);
        StartScene.SetActive(true);
    }

    void LoadScene()
    {
        StoreScene.SetActive(true);
        StartScene.SetActive(false);
        MainUIManager.instance.OnStart();
        GameManager.instance.OnStart();
        EventManager.Instance.SetEvent();
    }
    
    public void OnClickStartGame()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        StartFadeManager.Instance.StartFadeIn();
        yield return new WaitForSeconds(1.0f);
        LoadingImage.Image.gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(SetLoading());
            yield return new WaitForSeconds(1.0f);
        }
        LoadingImage.Image.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        LoadScene();
    }

    IEnumerator SetLoading()
    {
        LoadingImage.Image.sprite = LoadingImage.RightIdle;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.Right;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.RightRun;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.Right;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.RightIdle;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.LeftIdle;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.Left;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.LeftRun;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.Left;
        yield return new WaitForSeconds(0.1f);
        LoadingImage.Image.sprite = LoadingImage.LeftIdle;
        yield return new WaitForSeconds(0.1f);
        
    }

    public void LoadSaveGame()
    {
        Debug.Log(PlayerPrefs.GetString("Shlef"));
        if (PlayerPrefs.GetString("Shlef") == null || PlayerPrefs.GetString("Shlef") == "") return;
        LoadData loadData = new LoadData();
        loadData.Money = PlayerPrefs.GetInt("Money");
        loadData.Date = PlayerPrefs.GetInt("Date");
        loadData.Shlef = PlayerPrefs.GetString("Shlef");
        loadData.tngmlLikeability = PlayerPrefs.GetInt("tngmlLike");
        loadData.tngmlLibreation = PlayerPrefs.GetInt("tngmlLiberation");
        loadData.gPwlLikeability = PlayerPrefs.GetInt("gPwlLike");
        loadData.gPwlLibreation = PlayerPrefs.GetInt("gPwlLiberation");
        loadData.isNight = PlayerPrefs.GetInt("isNight");
        GameManager.instance.LoadGame(loadData);
        // GameManager.instance.dgdg();
        StartCoroutine(Load());
    }
}
