using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;
    // public int index;
    public string EventTitle;
    public TextMeshProUGUI MoneyUI;
    public TextMeshProUGUI LikeabilityUI;
    public int money;
    public int tngmlLeastLikeability = 0;
    public int gPwlLeastLikeability = 0;
    public bool istngml = false;
    public bool isLiberationEvent = false;
    public int LibreationEventIdx = 1;
    public int index;
    

    private void Start()
    {
        if (tngmlLeastLikeability == 0 && gPwlLeastLikeability == 0) LikeabilityUI.text = "조건 : 없음";
        else if(tngmlLeastLikeability > 0 && gPwlLeastLikeability == 0) LikeabilityUI.text = String.Format("조건 : 수희 호감도 {0} 이상", tngmlLeastLikeability);
        else if(tngmlLeastLikeability == 0 && gPwlLeastLikeability > 0) LikeabilityUI.text = String.Format("조건 : 혜지 호감도 {0} 이상", tngmlLeastLikeability);
        else if(tngmlLeastLikeability > 0 && gPwlLeastLikeability > 0) LikeabilityUI.text = "조건 : 수희 호감도 "+tngmlLeastLikeability+" / 혜지 호감도 "+gPwlLeastLikeability+" 이상";
        if (isLiberationEvent)
        {
            if (istngml && LibreationEventIdx != GameManager.instance.likeability.tngml.liberation+1) gameObject.SetActive(false);
            if (!istngml && LibreationEventIdx != GameManager.instance.likeability.gPwl.liberation+1) gameObject.SetActive(false);
        }
        if (money == 0) MoneyUI.text = "기본 금액";
        else
        {
            if(money > 0) MoneyUI.text = String.Format("+{0}", money);
            else MoneyUI.text = String.Format("{0}", money);
        }
    }

    
    public void Trigger()
    {
        Debug.Log("dddd");
        if ((GameManager.instance.Date > 0 && MainUIManager.instance.getBookShelfInfo() <= 0) ||
            money + GameManager.instance.Money < 0 ||
            tngmlLeastLikeability > GameManager.instance.likeability.tngml.likeability ||
            gPwlLeastLikeability > GameManager.instance.likeability.gPwl.likeability)
        {
            MainUIManager.instance.SetEventBlockText("필수조건 및 조건이 맞지 않습니다!\n(돈/ 호감도를 확인해주세요!)");
            MainUIManager.instance.EventBlockHandler(true);
            return;
        }

        
        StartCoroutine(TriggerLoad());
        OnClick();
    }

    void OnClick()
    {
        if (isLiberationEvent)
        {
            for (int i = 0; i < EventManager.Instance.EventList.Length; i++)
            {
                if (EventManager.Instance.EventList[i].index == index)
                    EventManager.Instance.EventList[i].isWatch = true;
            }
        }
        DoneInfo doneInfo = new DoneInfo();
        doneInfo.money = money;
        doneInfo.title = EventTitle;
        doneInfo.normalMoney = GameManager.instance.CalculateDayMoney();
        Debug.Log("normal : "+doneInfo.normalMoney+", money : "+doneInfo.money +", title : "+doneInfo.title);
        MainUIManager.instance.SetDoneEvent(doneInfo);
    }
    
    IEnumerator TriggerLoad()
    {
        MySceneManager.Instance.LoadScene();
        yield return new WaitForSeconds(1.0f);
        if(EventTitle != "Normal") gameObject.SetActive(false);
        if (isLiberationEvent)
        {
            if (istngml) GameManager.instance.Addtngmlliberation();
            else GameManager.instance.AddgPwlliberation();
        }
        DialogueManager.Instance.Init(info);
        // EventManager.Instance.WatchEvent(index);
    }
}

/*
 yield return new WaitForSeconds(1.0f);
        if(isTalk)
        {
            Debug.Log("hi");
            DialogueUI.SetActive(true);
            StoreUI.SetActive(false);
        }
        else
        {
            Debug.Log("bye");
            DialogueUI.SetActive(false);
            StoreUI.SetActive(true);
        }

*/