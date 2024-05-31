using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public GameObject NameUI;
    public TextMeshProUGUI Sentence;

    public Image ModalImage;
    public Image BackgroundImage;
    public Sprite NormalBackgroundImage;
    public Image CharacterImage;
    public Image CharacterWearImage;
    public GameObject DialogueInfoUI;
    public GameObject DialogueUI;
    public GameObject SelectOptions;
    public List<GameObject> Options;

    private DialogEntry Character;
    private Queue<DialogueInfo> DialogueInfo = new Queue<DialogueInfo>();

    private Queue<DialogEntry> selectDialogEntries = new Queue<DialogEntry>();
    private DialogueInfo selectDialogueInfo;

    private bool isRun = false;
    private bool isSelect = false;

    public static DialogueManager Instance {
        get {
            return instance;
        }
    }
    private static DialogueManager instance;


    public void Awake()
    {
        ModalImage.gameObject.SetActive(false);
        DialogueUI.SetActive(false);
        SelectOptions.SetActive(false);
        foreach (var gameObject in Options)
        {
            gameObject.SetActive(false);
        }
        if (instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
    }
    

    public void Init(Dialogue info)
    {
        DialogueUI.SetActive(true);
        foreach (var information in info.dialogueInfos)
        {
            DialogueInfo.Enqueue(information);
        }
        Begin();
    }

    public void Begin()
    {
        isRun = false;
        Next();
    }

    public void SetDialogueImage(DialogEntry dialogEntry)
    {
        
        if (dialogEntry.Name == "" || dialogEntry.Name == null) NameUI.SetActive(false);
        else
        {
            Name.text = dialogEntry.Name;
            NameUI.SetActive(true);
        }
        //옷, 배경 입히기
        if (dialogEntry.backgroundImage != null)
            BackgroundImage.sprite = dialogEntry.backgroundImage;
        else
            BackgroundImage.sprite = NormalBackgroundImage;
        if (dialogEntry.characterObject != null)
        {
            CharacterImage.gameObject.SetActive(true);
            CharacterImage.sprite = dialogEntry.characterObject;
        }
        else
            CharacterImage.gameObject.SetActive(false);

        if (dialogEntry.characterWearObject != null)
        {
            CharacterWearImage.gameObject.SetActive(true);
            CharacterWearImage.sprite = dialogEntry.characterWearObject;
        }
        else
            CharacterWearImage.gameObject.SetActive(false);
    }

    public void Next()
    {
        if (isRun) return;
        isRun = true;
        if (!isSelect)
        {
            if (DialogueInfo.Count == 0 && selectDialogEntries.Count == 0) StartCoroutine(End());
            else
            {
                DialogueInfo dialogueInfo = DialogueInfo.Dequeue();
                if (dialogueInfo.ImageModal != null)
                {
                    isRun = false;
                    ModalImage.gameObject.SetActive(true);
                    ModalImage.sprite = dialogueInfo.ImageModal;
                }
                if (dialogueInfo.dialogEntry.isSpecialEvent)
                {
                    StartCoroutine(viewSpecialEvent(dialogueInfo.dialogEntry));
                }

                else
                {
                    if (dialogueInfo.isSelect) Select(dialogueInfo);
                    SetDialogueImage(dialogueInfo.dialogEntry);
                    StartCoroutine(viewSentence(dialogueInfo.dialogEntry.sentences));
                }
            }
        }
        else
        {
            if (selectDialogEntries.Count == 0 && DialogueInfo.Count == 0) StartCoroutine(End());
            else if (selectDialogEntries.Count == 0 && DialogueInfo.Count != 0)
            {
                isSelect = false;
                DialogEntry dialogEntry = new DialogEntry();
                dialogEntry.Name = "";
                SetDialogueImage(dialogEntry);
                StartCoroutine(viewSentence("..."));
            }
            else
            {
                DialogEntry dialogEntry = selectDialogEntries.Dequeue();
                Name.text = dialogEntry.Name;
                //옷, 배경 입히기
                SetDialogueImage(dialogEntry);
                StartCoroutine(viewSentence(dialogEntry.sentences));
            }
        }
    }

    public void OnClickImageModal()
    {
        ModalImage.gameObject.SetActive(false);
        isRun = false;
        Next();
    }

    IEnumerator viewSpecialEvent(DialogEntry dialogEntry)
    {
        MySceneManager.Instance.StartFadeIn();
        yield return new WaitForSeconds(1.0f);
        DialogueInfoUI.gameObject.SetActive(false);
        CharacterImage.gameObject.SetActive(false);
        CharacterWearImage.gameObject.SetActive(false);
        BackgroundImage.sprite = dialogEntry.backgroundImage;
        MySceneManager.Instance.StartFadeOut();
        yield return new WaitForSeconds(4.0f);
        SetDialogueImage(dialogEntry);
        DialogueInfoUI.gameObject.SetActive(true);
        isRun = false;
        Next();
    }
    
    IEnumerator viewSentence(string sentences)
    {
        for (int i = 1; i < sentences.Length + 1; i++)
        {
            yield return new WaitForSeconds(0.015f);
            Sentence.text = sentences.Substring(0, i);
        }
        isRun = false;
    }

    IEnumerator End()
    {
        MySceneManager.Instance.StartFadeIn();
        yield return new WaitForSeconds(1.0f);
        Sentence.text = string.Empty;
        DialogueUI.SetActive(false);
        SelectOptions.SetActive(false);
        foreach (var gameObject in Options)
        {
            gameObject.SetActive(false);
        }
        Debug.Log("END");
        MainUIManager.instance.DoneEvent();
        MySceneManager.Instance.StartFadeOut();

    }
    void Select(DialogueInfo dialogueInfo)
    {
        selectDialogueInfo = dialogueInfo;
        foreach (var information in selectDialogueInfo.dialogSelectInfo)
        {
            information.SelectInfo.SelectUI.SetActive(true);
            information.SelectInfo.TextUI.text = information.SelectInfo.title;
        }
        SelectOptions.SetActive(true);
    }

    public void onSelect(int index)
    {
        foreach (var information in selectDialogueInfo.dialogSelectInfo)
        {
            if (information.SelectInfo.index == index)
            {
                if (information.istngml) GameManager.instance.Addtngmllikeability(information.likeability);
                else GameManager.instance.AddgPwllikeability(information.likeability);
                SelectOptions.SetActive(false);
                foreach (var selectinfo in information.SelectInfo.Dialog)
                {
                    Debug.Log(selectinfo.Name);
                    selectDialogEntries.Enqueue(selectinfo);
                    isSelect = true;
                }
                Next();
            }
        }   
    }
    
}

