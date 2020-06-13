using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deposit", menuName = "Deposit")] [System.Serializable]
public class ResourceDeposit : ScriptableObject
{
    public string deposit_name;
    public int deposit_id;

    public int max_health;

    public List<DepositResourceStack> resources;

    public void addToDeposit(Item item, int count)
    {
        resources.Add(new DepositResourceStack(item, count));
    }
}
