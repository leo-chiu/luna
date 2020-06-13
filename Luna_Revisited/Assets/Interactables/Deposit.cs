using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : Interactable
{
    // stores the deposit information
    public int deposit_id;
    public ResourceDeposit resource;
    public int deposit_health;
    public int deposit_max_health;
    public PlayerStats stats;

    public void Start()
    {
        if (DepositManager.instance.all_deposits.ContainsKey(deposit_id))
        {
            resource = DepositManager.instance.all_deposits[deposit_id];
        }
        else
        {
            resource = DepositManager.instance.default_deposit;

        }
        // when this deposit is spawned in, it should have full durability, as it hasn't been extracted yet
        deposit_max_health = resource.max_health;
        deposit_health = deposit_max_health;
    }

    public override void Interact()
    {
        base.Interact();
        stats = player.GetComponent<PlayerStats>();
        mineDeposit();
    }

    public void mineDeposit()
    {
        deposit_health -= stats.mining.getValue();
        StartCoroutine(Shake(.2f, .04f));
        if(deposit_health <= 0)
        {
            StartCoroutine(extract());
        }
        stats.resetCooldown();
        has_interacted = false;
        isFocus = false;
    }

    public IEnumerator extract()
    {
        float y = .1f;
        foreach (DepositResourceStack stack in resource.resources)
        {
            GameObject drop_template = (GameObject)Resources.Load("Prefabs/Item/Loot", typeof(GameObject));
            // refer to the enemy drop table and roll dice 
            float x = Random.Range(-1f, 1f) * ItemManager.instance.scatter_factor;
            y += .1f;
            Vector2 drop_location = new Vector2(transform.position.x + x, transform.position.y + y);
            ItemPickup drop = Instantiate(drop_template, drop_location, Quaternion.identity, null).GetComponentInChildren<ItemPickup>();
            drop.primeItem(stack.item.item_id, stack.count);
        }
        yield return StartCoroutine(Fade());
        Destroy(gameObject);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y + y, 0f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = orignalPosition;
    }

    public IEnumerator Fade()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        Color color = GetComponent<SpriteRenderer>().color;
        for(float alpha = color.a; alpha > 0; alpha -= .02f)
        {
            color.a = alpha;
            GetComponent<SpriteRenderer>().color = color;
            yield return 0;
        }
    }

}
