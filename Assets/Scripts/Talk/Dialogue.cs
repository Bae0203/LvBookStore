using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct SelectInfo
{
    public int index;
    public string title;
    public List<DialogEntry> Dialog;
    public GameObject SelectUI;
    public TextMeshProUGUI TextUI;
}

[Serializable]
public struct DialogSelectInfo
{
    public SelectInfo SelectInfo;
    public int likeability;
    public bool istngml;
}


[Serializable]
public struct DialogEntry
{
    public Sprite characterObject;
    public Sprite characterWearObject;
    public Sprite backgroundImage;
    public string Name;
    public string sentences;
    public bool isSpecialEvent;
}

[Serializable]
public struct DialogueInfo
{
    public bool isSelect;
    public DialogEntry dialogEntry;
    public List<DialogSelectInfo> dialogSelectInfo;
    public Sprite ImageModal;
}

[Serializable]
public class Dialogue
{
    public List<DialogueInfo> dialogueInfos;
}