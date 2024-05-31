using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct likeabilityStruct
{
    public int likeability;
    public int liberation;
}

[Serializable]
public struct likeabilityNames
{
    public likeabilityStruct tngml;
    public likeabilityStruct gPwl;
}

[Serializable]
public struct LoadData
{
    public int Money;
    public int Date;
    public string Shlef;
    public int tngmlLikeability;
    public int tngmlLibreation;
    public int gPwlLikeability;
    public int gPwlLibreation;
    public int isNight;
}


public class GameManager : MonoBehaviour
{
    
    
    public static GameManager instance;
    public TextMeshProUGUI MoneyUI;
    public TextMeshProUGUI DateUI;
    public int Money = 0;
    public int Date = 0;
    public likeabilityNames likeability;

    // public Bookshelf bookshelfInfomation;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
        }
    }

    public void OnStart()
    {
        DateUI.text = String.Format("{0} 일차", Date+1);
        MoneyUI.text = String.Format("{000} 원", Money);
    }

    int isActive(bool active)
    {
        if (active) return 1;
        return 0;
    }

    public void Save()
    {
        string shelf = isActive(MainUIManager.instance.MainUIList.FirstFloorBookShelf.firstBookShelf) + "," +
                       isActive(MainUIManager.instance.MainUIList.FirstFloorBookShelf.secondBookShelf) + "," +
                       isActive(MainUIManager.instance.MainUIList.SecondFloorBookShelf.firstBookShelf) + "," +
                       isActive(MainUIManager.instance.MainUIList.SecondFloorBookShelf.secondBookShelf) + ",";
        Debug.Log(shelf);
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("Date", Date);
        PlayerPrefs.SetString("Shlef", shelf);
        PlayerPrefs.SetInt("tngmlLike",likeability.tngml.likeability);
        PlayerPrefs.SetInt("tngmlLiberation",likeability.tngml.liberation);
        PlayerPrefs.SetInt("gPwlLike",likeability.gPwl.likeability);
        PlayerPrefs.SetInt("gPwlLiberation",likeability.gPwl.liberation);
        PlayerPrefs.SetInt("isNight", MainUIManager.instance.isNight ? 1:0);
        Debug.Log("gPwl : "+likeability.gPwl.likeability+" tngml : "+ likeability.tngml.likeability);
    }

    public void dgdg()
    {
        Debug.Log("dfdffdf");
    }

    public void LoadGame(LoadData loadData)
    {
        string[] result = loadData.Shlef.Split(new char[] { ',' });
        if (result[0] == "1")
        {
            MainUIManager.instance.MainUIList.FirstFloorBookShelf.firstBookShelf = true;
            Debug.Log("000");
        }
        if (result[1] == "1")
        {
            MainUIManager.instance.MainUIList.FirstFloorBookShelf.secondBookShelf = true;
            Debug.Log("111");

        }
        if (result[2] == "1")
        {
            MainUIManager.instance.MainUIList.SecondFloorBookShelf.firstBookShelf = true;
            Debug.Log("222");

        }
        if (result[3] == "1")
        {
            MainUIManager.instance.MainUIList.SecondFloorBookShelf.secondBookShelf = true;
            Debug.Log("333");

        }
        if (loadData.isNight == 1) MainUIManager.instance.isNight = true;
        Money = loadData.Money;
        Date = loadData.Date;
        likeability.tngml.likeability = loadData.tngmlLikeability;
        likeability.tngml.liberation = loadData.tngmlLibreation;
        likeability.gPwl.likeability = loadData.gPwlLikeability;
        likeability.gPwl.liberation = loadData.gPwlLibreation;
    }

    //호감도 관련
    //호감도 - 수희
    public void Addtngmllikeability(int likeValue)
    {
        likeability.tngml.likeability += likeValue;
    }
    //호감도 - 혜지
    public void AddgPwllikeability(int likeValue)
    {
        likeability.gPwl.likeability += likeValue;
    }
    //해방도 - 수희
    public void Addtngmlliberation()
    {
        likeability.tngml.liberation += 1;
    }
    //해방도 - 혜지
    public void AddgPwlliberation()
    {
        likeability.gPwl.liberation += 1;
    }
    
    //돈 관련
    //돈 추가
    public void AddMoney(int money)
    {
        Money += money;
        MoneyUI.text = String.Format("{000} 원", Money);
    }

    //일마다 번 돈 계산(기본)
    public int CalculateDayMoney()
    {
        int activeBookshelf = MainUIManager.instance.getBookShelfInfo();
        Debug.Log(activeBookshelf);
        Debug.Log("날짜 : "+Date+" / 계산 : "+ (Math.Truncate((float)(Date / 3)) + 1) +", "+ (Date / 3)+ " / 1000 * 책장 : " + 1000 * activeBookshelf);
        return 1000 * activeBookshelf * (Date / 3 + 1);
    }//1000 * 4 * 일차 수 / 3
    //주마다 번 돈 계산
    public int CalculateWeekMoney()
    {
        return 10000 * ((Date+1) / 5 + 1) + 1000 * (Date / 5);
    }//10000 * (주+1) + (10000*주) / 10
    
    //일자 지날때
    public void NextDay()
    {
        Debug.Log("OnClick!!!!");
        StartCoroutine(GoNextDay());
    }

    IEnumerator GoNextDay()
    {
        Date++;
        MySceneManager.Instance.RunNextDateScene();
        int result = CalculateDayMoney();
        // resetBookshelf();
        MainUIManager.instance.NextDayEvent();
        if (Date % 5 == 0 && Date > 0) EndWeek();
        DateUI.text = String.Format("{0} 일차", Date+1);
        AddMoney(result);
        EventManager.Instance.SetEvent();
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Money : "+Money+", Date : " + Date);
    }
    //주 지날때
    public void EndWeek()
    {
        Debug.Log("endednednd");
        AddMoney(CalculateWeekMoney());
    }
    
    
}
