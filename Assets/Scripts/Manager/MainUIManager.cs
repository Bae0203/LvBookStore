using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct FloorBookShelf
{
    public bool firstBookShelf;
    public Image NoneFirstBookShelfUI;
    public Sprite NoneFirstBookShelfSprite;
    public Sprite ActiveFirstBookShelfSprite;
    public bool secondBookShelf;
    public Image NoneSecondBookShelfUI;
    public Sprite NoneSecondBookShelfSprite;
    public Sprite ActiveSecondBookShelfSprite;
}

[Serializable]
public struct LikeAblityInfo
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI likeablity;
    public TextMeshProUGUI context;
}

[Serializable]
public struct DoneInfo
{
    public int normalMoney;
    public string title;
    public int money;
}
[Serializable]
public struct DoneUIInfo
{
    public TextMeshProUGUI date;
    public TextMeshProUGUI normalMoney;
    public TextMeshProUGUI title;
    public TextMeshProUGUI money;
    public TextMeshProUGUI totalMoney;
}


[Serializable] 
public struct MainUI
{
    public GameObject EventList;
    public GameObject OpenEventBtn;
    
    public Image FloorUI;
    public GameObject UpFloorBtn;
    public GameObject DownFloorBtn;
    public GameObject FirstFloor;
    public FloorBookShelf FirstFloorBookShelf;
    public Sprite FirstFloorImage;
    public GameObject SecondFloor;
    public FloorBookShelf SecondFloorBookShelf;
    public Sprite SecondFloorImage;

    public GameObject TermUI;

    public GameObject LikeAbilityUI;
    public LikeAblityInfo LikeAbilityInfo;

    public GameObject DoneUI;
    public DoneUIInfo doneUIInfo;
    public GameObject FinishButton;

    public GameObject EventBlockUI;
    public TextMeshProUGUI EventBlockText;
    // public GameObject NightBlockUI;

    public Image BackgroundImage;
    public Sprite DayImage;
    public Sprite NightImage;
    
    public List<GameObject> FirstNoneUI;
}

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;

    public MainUI MainUIList;
    public int Floor = 1;

    public bool isNight = false;
    
    private void Awake()
    {
        if (MainUIManager.instance == null)
        {
            MainUIManager.instance = this;
        }
    }

    public void OnStart()
    {
        MainUIList.TermUI.SetActive(true);
        MainUIList.BackgroundImage.sprite = MainUIList.DayImage;
        if(GameManager.instance.Date == 0) MainUIList.TermUI.SetActive(false);
        if (GameManager.instance.Date < 1) SetNoneUI(false);
        else SetNoneUI(true);
        // Debug.Log("dfdfd");
        StartCoroutine(StartScene());
        Debug.Log(GameManager.instance.Date);
        MainUIList.LikeAbilityUI.SetActive(false);
        MainUIList.FinishButton.SetActive(false);
        //MainUIList.FloorUI = GetComponent<Image>();
        MainUIList.DoneUI.SetActive(false);
        MainUIList.EventBlockUI.SetActive(false);
        CloseEventList();
        GoFirstFloor();
        SetBookShlef();
        if (isNight)
        {
            DoneInfo doneInfo = new DoneInfo();   
            doneInfo.normalMoney= GameManager.instance.CalculateDayMoney();
            doneInfo.money = 0;
            doneInfo.title = "Normal";
            SetDoneEvent(doneInfo);
            DoneEvent();
        }
        // resetBookShelf();
    }

    public void SetNoneUI(bool isActive)
    {
        foreach (var UI in MainUIList.FirstNoneUI)
        {
            UI.SetActive(isActive);
        }
    }
    
    IEnumerator StartScene()
    {
        MySceneManager.Instance.LoadingDateText.gameObject.SetActive(true);
        MySceneManager.Instance.LoadingDateText.text = String.Format("{0} 일차", GameManager.instance.Date+1);;
        yield return new WaitForSeconds(2.0f);
        MySceneManager.Instance.LoadingDateText.gameObject.SetActive(false);
        MySceneManager.Instance.StartFadeOut();
    }

    //이벤트 (마지막 영수증)
    public void SetDoneEvent(DoneInfo doneInfo)
    {
        MainUIList.doneUIInfo.normalMoney.text = String.Format("+{0}",doneInfo.normalMoney);
        if (doneInfo.title != "Normal")
        {
            MainUIList.doneUIInfo.title.text = doneInfo.title;
            if(doneInfo.money>0) MainUIList.doneUIInfo.money.text = String.Format("+{0}",doneInfo.money);
            else MainUIList.doneUIInfo.money.text = String.Format("{0}",doneInfo.money);
            GameManager.instance.Money += doneInfo.money;
            GameManager.instance.MoneyUI.text = String.Format("{000} 원", GameManager.instance.Money);
        }
        else
        {
            MainUIList.doneUIInfo.title.text = "";
            MainUIList.doneUIInfo.money.text = "";
        }

        int totalMoney = doneInfo.money + doneInfo.normalMoney;
        if(totalMoney > 0) MainUIList.doneUIInfo.totalMoney.text = String.Format("+{0}", totalMoney);
        else MainUIList.doneUIInfo.totalMoney.text = String.Format("{0}", totalMoney);
    }

    public void DoneEvent()
    {
        isNight = true;
        MainUIList.FinishButton.SetActive(isNight);
        MainUIList.OpenEventBtn.SetActive(false);
        MainUIList.EventList.SetActive(false);
        MainUIList.BackgroundImage.sprite = MainUIList.NightImage;
        MainUIList.doneUIInfo.date.text = String.Format("{0} 일차", GameManager.instance.Date+1);
    }
    
    //호감도 관리
    public string LikeAbilityMessage(int liberation)
    {
        if (liberation == 3) return "친구로 볼 수 없는 사이";
        else if (liberation == 2) return "점점 친구로 안볼지도?";
        else if (liberation == 1) return "친한 친구 사이!!?";
        else return "아직은 친해지는 사이..";
    }
    //수희
    public void OnClicktngmlLikeAbility()
    {
        MainUIList.LikeAbilityInfo.title.text = String.Format("수희");
        MainUIList.LikeAbilityInfo.context.text = LikeAbilityMessage(GameManager.instance.likeability.tngml.liberation);
        MainUIList.LikeAbilityInfo.likeablity.text = String.Format("{0}",GameManager.instance.likeability.tngml.likeability);
        MainUIList.LikeAbilityUI.SetActive(true);
    }
    //혜지
    public void OnClickgPwlLikeAbility()
    {
        MainUIList.LikeAbilityInfo.title.text = String.Format("혜지");
        MainUIList.LikeAbilityInfo.context.text = LikeAbilityMessage(GameManager.instance.likeability.gPwl.liberation);
        MainUIList.LikeAbilityInfo.likeablity.text = String.Format("{0}",GameManager.instance.likeability.gPwl.likeability);
        MainUIList.LikeAbilityUI.SetActive(true);
    }
    public void CloseLikeAbility()
    {
        MainUIList.LikeAbilityUI.SetActive(false);
    }

    //층 관리 함수
    //1층 관리
    public void GoFirstFloor()
    {
        MainUIList.FloorUI.sprite = MainUIList.FirstFloorImage;
        Floor = 1;
        MainUIList.FirstFloor.SetActive(true);
        MainUIList.SecondFloor.SetActive(false);
        MainUIList.UpFloorBtn.SetActive(true);
        MainUIList.DownFloorBtn.SetActive(false);
    }
    //2층 관리
    public void GoSecondFloor()
    {
        MainUIList.FloorUI.sprite = MainUIList.SecondFloorImage;
        Floor = 2;
        MainUIList.FirstFloor.SetActive(false);
        MainUIList.SecondFloor.SetActive(true);
        MainUIList.UpFloorBtn.SetActive(false);
        MainUIList.DownFloorBtn.SetActive(true);
    }

    //영업 종료
    public void Done()
    {
        MainUIList.DoneUI.SetActive(true);
    }

    //이벤트 블럭
    public void EventBlockHandler(bool active)
    {
        MainUIList.EventBlockUI.SetActive(active);
    }

    public void SetEventBlockText(string text)
    {
        MainUIList.EventBlockText.text = text;
    }

    public void SetBookShlef()
    {
        if(MainUIList.FirstFloorBookShelf.firstBookShelf) 
            MainUIList.FirstFloorBookShelf.NoneFirstBookShelfUI.sprite =
                MainUIList.FirstFloorBookShelf.ActiveFirstBookShelfSprite;
        if(MainUIList.FirstFloorBookShelf.secondBookShelf)
            MainUIList.FirstFloorBookShelf.NoneSecondBookShelfUI.sprite =
                MainUIList.FirstFloorBookShelf.ActiveSecondBookShelfSprite;
        if(MainUIList.SecondFloorBookShelf.firstBookShelf)
            MainUIList.SecondFloorBookShelf.NoneFirstBookShelfUI.sprite =
                MainUIList.SecondFloorBookShelf.ActiveFirstBookShelfSprite;
        if(MainUIList.SecondFloorBookShelf.secondBookShelf)
            MainUIList.SecondFloorBookShelf.NoneSecondBookShelfUI.sprite =
                MainUIList.SecondFloorBookShelf.ActiveSecondBookShelfSprite;
    }
    
    //책장 관리 함수
    public void SetActiveBookShelf(int index)
    {
        if (GameManager.instance.Money - 3000 < 0)
        {
            SetEventBlockText("금액이 부족합니다!");
            MainUIList.EventBlockUI.SetActive(true);
            return;
        }
        if (isNight)
        {
            SetEventBlockText("밤입니다.\n가게를 정리한 뒤\n영업 종료를 해주세요.");
            MainUIList.EventBlockUI.SetActive(true);
            return;
        }
        if (Floor == 1)
        {
            if (index == 1)
            {
                if (MainUIList.FirstFloorBookShelf.firstBookShelf) return;
                MainUIList.FirstFloorBookShelf.firstBookShelf = true;
                MainUIList.FirstFloorBookShelf.NoneFirstBookShelfUI.sprite =
                    MainUIList.FirstFloorBookShelf.ActiveFirstBookShelfSprite;
                // GameManager.instance.UpgradeBookShelf(0);
            }
            else
            {
                if (MainUIList.FirstFloorBookShelf.secondBookShelf) return;

                MainUIList.FirstFloorBookShelf.secondBookShelf = true;
                MainUIList.FirstFloorBookShelf.NoneSecondBookShelfUI.sprite =
                    MainUIList.FirstFloorBookShelf.ActiveSecondBookShelfSprite;
                // GameManager.instance.UpgradeBookShelf(1);
            }
        }
        else
        {
            if (index == 1)
            {
                if (MainUIList.SecondFloorBookShelf.firstBookShelf) return;

                MainUIList.SecondFloorBookShelf.firstBookShelf = true;
                MainUIList.SecondFloorBookShelf.NoneFirstBookShelfUI.sprite =
                    MainUIList.SecondFloorBookShelf.ActiveFirstBookShelfSprite;
                // GameManager.instance.UpgradeBookShelf(2);
            }
            else
            {
                if (MainUIList.SecondFloorBookShelf.secondBookShelf) return;

                MainUIList.SecondFloorBookShelf.secondBookShelf = true;
                MainUIList.SecondFloorBookShelf.NoneSecondBookShelfUI.sprite =
                    MainUIList.SecondFloorBookShelf.ActiveSecondBookShelfSprite;
                // GameManager.instance.UpgradeBookShelf(3);
            }
        }

        GameManager.instance.AddMoney(-3000);
    }

    public int getBookShelfInfo()
    {
        int count = 0;
        if (MainUIList.FirstFloorBookShelf.firstBookShelf) count+=1;
        if (MainUIList.FirstFloorBookShelf.secondBookShelf) count+=1;
        if (MainUIList.SecondFloorBookShelf.firstBookShelf) count+=1;
        if (MainUIList.SecondFloorBookShelf.secondBookShelf) count+=1;
        return count;
    }
    
    public void resetBookShelf()
    {
        MainUIList.FirstFloorBookShelf.firstBookShelf = false;
        MainUIList.FirstFloorBookShelf.secondBookShelf = false;
        MainUIList.SecondFloorBookShelf.firstBookShelf = false;
        MainUIList.SecondFloorBookShelf.secondBookShelf = false;
        MainUIList.FirstFloorBookShelf.NoneFirstBookShelfUI.sprite =
            MainUIList.FirstFloorBookShelf.NoneFirstBookShelfSprite;
        MainUIList.FirstFloorBookShelf.NoneSecondBookShelfUI.sprite =
            MainUIList.FirstFloorBookShelf.NoneSecondBookShelfSprite;
        MainUIList.SecondFloorBookShelf.NoneFirstBookShelfUI.sprite =
            MainUIList.SecondFloorBookShelf.NoneFirstBookShelfSprite;
        MainUIList.SecondFloorBookShelf.NoneSecondBookShelfUI.sprite =
            MainUIList.SecondFloorBookShelf.NoneSecondBookShelfSprite;
    }
    
    
    //메인 이벤트 리스트 관리 함수
    public void OpenEventList()
    {
        MainUIList.EventList.SetActive(true);
        MainUIList.OpenEventBtn.SetActive(false);
    }
    public void CloseEventList()
    {
        MainUIList.EventList.SetActive(false);
        MainUIList.OpenEventBtn.SetActive(true);
    }

    //다음날로
    public void NextDayEvent()
    {
        resetBookShelf();
        GoFirstFloor();
        isNight = false;
        MainUIList.DoneUI.SetActive(false);
        MainUIList.FinishButton.SetActive(false);
        MainUIList.OpenEventBtn.SetActive(true);
        MainUIList.BackgroundImage.sprite = MainUIList.DayImage;
        if (GameManager.instance.Date < 1) SetNoneUI(false);
        else SetNoneUI(true);
    }
}