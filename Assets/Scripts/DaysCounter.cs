﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaysCounter : MonoBehaviour
{
    public static DaysCounter singleton { get; private set; }

    public static int CurrentDay;
    public bool AlarmAvailable;
    public bool DayEndAvailable;

    public List<int> messagesPerMinute = new List<int>();
    public List<int> delayBetweenMessages = new List<int>();

    public delegate void ChangeDayHandler();
    public event ChangeDayHandler DayChanged;

    public TMPro.TextMeshProUGUI day;
    public TMPro.TextMeshProUGUI days;
    public List<GameObject> switchOnDay = new List<GameObject>();
    public GameObject pelena;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        CurrentDay = 1;

        AlarmAvailable = true;
        DayEndAvailable = true;

        DayChanged += DayChange;
        DayChanged += PCMail.singleton.DayChange;
        DayChanged += PCPopUp.singleton.DayChange;
        DayChanged += PhoneMail.singleton.DayChange;
        DayChanged += PlayerBehavior.singleton.DayChange;
        DayChanged += MessageManager.singleton.DayChange;
        DayChanged += _Timer.singleton.DayEnd;
    }

    private void Update()
    {
        day.text = "Day: "+ CurrentDay.ToString();
        days.text = "Day: " + CurrentDay.ToString();
    }

    public void TriggerAlarm()
    {
        if (AlarmAvailable)
        {
            foreach(var obj in switchOnDay)
            {
                obj.SetActive(!obj.activeSelf);
            }
            pelena.SetActive(false);
            AlarmAvailable = false;
            _SoundManager.singleton.PlaySound(0);
        }
    }

    public void TriggerDayEnd()
    {
        if (DayEndAvailable)
        {
            foreach (var obj in switchOnDay)
            {
                obj.SetActive(!obj.activeSelf);
            }
            pelena.SetActive(true);
            DayEndAvailable = false;
            _SoundManager.singleton.PlaySound(6);
        }
    } 

    void DayChange()
    {
        CurrentDay += 1;
        AlarmAvailable = true; 
        DayEndAvailable = true;
    }

    public void TriggerDayChangeEvent()
    {
        Debug.Log($"new day {CurrentDay}!");
        DayChanged.Invoke();
    }
}
