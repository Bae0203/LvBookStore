using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct Libreation
{
    public bool isLiberationEvent;
    public int liberationLevel;
    public bool istngml;
}

[Serializable]
public struct Event
{
    public string Readme;
    public GameObject EventButton;
    public int date;
    public int index;
    public bool isWatch;
    public bool isNormalEvent;
    public Libreation LibreationInfo;
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance {
        get {
            return instance;
        }
    }
    
    private static EventManager instance;
    
    
    public Event[] EventList;

    private void Awake()
    {
        if (instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
    }
    
    public void SetEvent()
    {
        foreach (var eventInfo in EventList)
        {
            //우선 순위에 따라 보이고 안보임
            eventInfo.EventButton.SetActive(false);
            //날짜가 맞으면 true (우선순위 꼴등)
            if (GameManager.instance.Date == eventInfo.date)
            {
                eventInfo.EventButton.SetActive(true);
            }
            else
            {
                eventInfo.EventButton.SetActive(false);
            }
            //해방이벤트 일시 true(우선순위 3등)
            if (eventInfo.LibreationInfo.isLiberationEvent)
            {
                //수희 해방이벤트인지, 날짜가 해방이벤트를 볼 수 있는 날짜인지 체크
                if (eventInfo.LibreationInfo.istngml &&
                    eventInfo.LibreationInfo.liberationLevel == GameManager.instance.likeability.tngml.liberation &&
                    GameManager.instance.Date >= eventInfo.date)
                {
                    eventInfo.EventButton.SetActive(true);
                }
                if (!eventInfo.LibreationInfo.istngml &&
                    eventInfo.LibreationInfo.liberationLevel == GameManager.instance.likeability.gPwl.liberation &&
                    GameManager.instance.Date >= eventInfo.date)
                {
                    eventInfo.EventButton.SetActive(true);
                }
            }
            // 이벤트를 봤을시 false (우선순위 2등)
             if (eventInfo.isWatch)
             {
                 eventInfo.EventButton.SetActive(false);
             }
            //기본이벤트일시 true (우선순위 1등)
            if (eventInfo.isNormalEvent)
            {
                eventInfo.EventButton.SetActive(true);
                if (eventInfo.date > GameManager.instance.Date) eventInfo.EventButton.SetActive(false);
            }
            
        }
    }

    // public void WatchEvent(int index)
    // {
    //     for (int i = 0; i < EventList.Length; i++)
    //     {
    //         // if (EventList[i].index == index)
    //         // {
    //         //     EventList[i].isWatch = true;
    //         //     Debug.Log("watch!!");
    //         // }
    //     }
    //     SetEvent();
    // }
}
