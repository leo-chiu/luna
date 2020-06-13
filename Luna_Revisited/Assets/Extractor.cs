using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : Tool
{
    public void Start()
    {
        layer = LayerMask.GetMask("Deposit");
    }

    public override void Cast()
    {
        base.Cast();
        if (focus == null) return;

        if(focus.GetComponent<Deposit>() != null)
            focus.GetComponent<Deposit>().Interact();
    }
}
