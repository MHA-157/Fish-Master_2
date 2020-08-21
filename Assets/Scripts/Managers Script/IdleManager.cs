using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleManager : MonoBehaviour
{
    [HideInInspector]
    
    public int length;
    [HideInInspector]
    
    public int strength;
    [HideInInspector]
   
    public int offlineEarnings;
    [HideInInspector]
   
    public int lengthCost;
    [HideInInspector]
    
    public int strengthCost;
    [HideInInspector]
    
    public int offlineEarningsCost;
    [HideInInspector]
   
    public int wallet;
   
    [HideInInspector]
    public int totalGain;

    private int[] costs = new int[]
    {
        120, 151, 197, 250, 325, 456, 777, 550, 450, 700, 900, 1000, 1300, 1600, 2000,
    };

    public static IdleManager instance;
    
    
    void Awake()
    {
        if (IdleManager.instance)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        else
        {
            IdleManager.instance = this;
        }

        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        lengthCost =  costs[-length / 10 - 3];
        strengthCost = costs[strength - 3];
        offlineEarningsCost = costs[offlineEarnings - 3];
        wallet = PlayerPrefs.GetInt("Wallet", 0);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            DateTime  now = DateTime.Now;
            PlayerPrefs.SetString("Date", String.Empty);
            MonoBehaviour.print(now.ToString());
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date", String.Empty);
            if (@string != String.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineEarnings + 1.0);
                ScreensManager.instance.ChangeScreen(Screens.RETURN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLength()
    {
        length -= 10;
        wallet -= lengthCost;
        lengthCost =  costs[-length / 10 - 3];
        PlayerPrefs.SetInt("Length", -length);
        PlayerPrefs.SetInt("Wallet" , wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }
    
    public void BuyStrength()
    {
        strength++;
        wallet -= strengthCost;
        strengthCost =  costs[strength - 3];
        PlayerPrefs.SetInt("Strength", strength);
        PlayerPrefs.SetInt("Wallet" , wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }
    
    public void BuyOfflineEarnings()
    {
        offlineEarnings++;
        wallet -= offlineEarningsCost;
        offlineEarningsCost =  costs[offlineEarnings - 3];
        PlayerPrefs.SetInt("Offline", offlineEarnings);
        PlayerPrefs.SetInt("Wallet" , wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectMoney()
    {
        wallet += totalGain;
        PlayerPrefs.SetInt("wallet", wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    
}
