﻿using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class XRCheckbox : MonoBehaviour
{
    //private XRSpriteButton checkButton;
    //private TextMeshPro tmpCheckText;

    //public Sprite checkSprite;
    //public Sprite uncheckSprite;

    //public Color checkColor = Color.green;
    //public Color uncheckColor = Color.red;

    //public bool isChecked = false;
    //public bool showText = true;

    //[ShowIf("showText")]
    //public GameObject textPanel;
    //[ShowIf("showText")]
    //public string checkText;

    //[Serializable]
    //public class CheckboxEvent : UnityEvent<bool> { }

    //public CheckboxEvent onChageState;
    //public UnityEvent onCheck;
    //public UnityEvent onUncheck;

    //private void OnValidate()
    //{
    //    if (checkButton == null)
    //        checkButton = GetComponentInChildren<XRSpriteButton>();

    //    ConfigureCheckText();

    //    SetCheckboxSprite();
    //    SetCheckboxColor();
    //}

    //private void Awake()
    //{
    //    if (checkButton == null)
    //        checkButton = GetComponentInChildren<XRSpriteButton>();

    //    if (checkButton == null)
    //    {
    //        Debug.LogError("This component needs a XRSpriteButton To works.");
    //        return;
    //    }

    //    //add checkbox unity event on xrButton clickDown event
    //    checkButton.onClickDown.AddListener(OnchangeStateFunction);

    //    XRMeshFeedback xrFeedback = checkButton.GetComponentInChildren<XRMeshFeedback>();
    //    xrFeedback.onProximityAreaExit.AddListener(SetCheckboxColor);
    //}

    //private void OnchangeStateFunction()
    //{
    //    isChecked = !isChecked;

    //    if (isChecked)
    //        onCheck?.Invoke();
    //    else
    //        onUncheck?.Invoke();

    //    SetCheckboxSprite();
    //    SetCheckboxColor();

    //    onChageState?.Invoke(isChecked);
    //}


    //private void ConfigureCheckText()
    //{
    //    if (showText)
    //    {
    //        if (tmpCheckText == null)
    //            tmpCheckText = textPanel.GetComponentInChildren<TextMeshPro>();

    //        tmpCheckText.text = checkText;
    //    }

    //    textPanel.SetActive(showText);
    //}

    //private void SetCheckboxSprite()
    //{
    //    if (checkSprite == null && uncheckSprite == null)
    //        return;

    //    if (isChecked)
    //        checkButton.SetSprite(checkSprite);
    //    else
    //        checkButton.SetSprite(uncheckSprite);
    //}

    //private void SetCheckboxColor()
    //{
    //    if (checkButton.buttonTransform.GetComponent<MeshRenderer>().sharedMaterial != null)
    //        checkButton.buttonTransform.GetComponent<MeshRenderer>().sharedMaterial.color = isChecked ? checkColor : uncheckColor;
    //}

    //public void SetText(string value)
    //{
    //    tmpCheckText.text = value;
    //}
}
