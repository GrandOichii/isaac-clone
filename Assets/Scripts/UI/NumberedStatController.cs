using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberedStatController : MonoBehaviour
{
    public GameObject sprite;
    public TMP_Text text;


    public string watching;

    public void OnNumberedStatChanged(string what, int newValue) {
        if (what != watching) return;

        text.text = newValue.ToString();
    }
}
