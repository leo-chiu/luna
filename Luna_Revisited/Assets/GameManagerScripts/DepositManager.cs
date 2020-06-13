using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositManager : MonoBehaviour
{
    public static DepositManager instance { set; get; }

    public Dictionary<int, ResourceDeposit> all_deposits;

    public ResourceDeposit default_deposit;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

            all_deposits = new Dictionary<int, ResourceDeposit>();

            ResourceDeposit[] deposit_assets = Resources.LoadAll<ResourceDeposit>("DepositAssets/Deposits");

            foreach (ResourceDeposit resource in deposit_assets)
            {
                if (!all_deposits.ContainsKey(resource.deposit_id))
                    all_deposits.Add(resource.deposit_id, resource);
            }
        }
    }

    public void Start()
    {
        //printDeposits();
    }

    public void printDeposits()
    {
        foreach(KeyValuePair<int, ResourceDeposit> s in all_deposits)
        {
            Debug.Log(s.Value.deposit_name + " will drop:");
            foreach (DepositResourceStack resource in s.Value.resources)
                Debug.Log(resource.count + " " + resource.item);
        }
    }

}
