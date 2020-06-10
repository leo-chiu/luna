using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public int item_id;
    public Item item;
    public int count;

    public float time_to_expire;

    private SpriteRenderer sprite;

    private float originalY;

    public GameObject shadow;

    public Sprite[] shadow_sprites;

    private AudioSource pick_up_sound_effect;

    public void Awake()
    {
        active = true;
        pick_up_sound_effect = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ItemDropEventSystem.instance.ItemPickup(this.GetInstanceID());
        }
    }

    public void FixedUpdate()
    {
        if (active)
        {
            if (!pick_up_sound_effect.isPlaying)
            {
                pick_up_sound_effect.enabled = false;
            }
            time_to_expire -= Time.deltaTime;
            if (time_to_expire <= 0)
            {
                active = false;
                StartCoroutine(RemoveDrop());
            }
            transform.position = new Vector3(transform.position.x, originalY + ((float)Mathf.Sin(Time.time) * ItemManager.instance.bobbing_speed), 0);

            int sprite_index;

            float y_ratio = Mathf.InverseLerp(originalY, originalY + 1 * ItemManager.instance.bobbing_speed, transform.position.y);

            sprite_index = Mathf.Clamp((int)(shadow_sprites.Length * y_ratio), 0, shadow_sprites.Length-1);

            transform.parent.GetComponent<SpriteRenderer>().sprite = shadow_sprites[sprite_index];
        }
    }

    public IEnumerator Fade_Destroy()
    {
        Color color = sprite.color;
        for (float i = 1f; i >= 0f; i-=.04f)
        {
            color.a = i;
            sprite.color = color;
            transform.parent.gameObject.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.001f);
        }
    }

    public void Start()
    {
        ItemDropEventSystem.instance.onItemPickup += Pickup;

        time_to_expire = TimeManager.instance.ItemExpire;
        originalY = transform.position.y;

        // items will be spawned in as the default item with a count of 0, as established in the prefab
        primeItem(item_id, count);
    }

    public void primeItem(int item_id, int count)
    { 
        // this function is called to change the item drop's item and count
        if (ItemManager.instance.all_items.ContainsKey(item_id))
        {
            item = ItemManager.instance.all_items[item_id];
            sprite.sprite = item.sprite;
            this.count = count;
            activateDrop();
        }
    }

    public void activateDrop()
    {
        transform.parent.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        ItemDropEventSystem.instance.onItemPickup -= Pickup;
    }

    public override void Interact()
    {
        if (active)
            ItemDropEventSystem.instance.ItemPickup(this.GetInstanceID());
    }

    public void Pickup(int item_instance_id)
    {
        if (this.GetInstanceID() != item_instance_id) return;

        int beforeCount = count;

        Inventory.instance.Loot(this);
        
        if (count == 0)
        {
            active = false;

            if (AudioManager.instance.current_item_pickup_sounds < AudioManager.instance.max_item_pickup_sounds_at_once)
            {
                pick_up_sound_effect.enabled = true;
                AudioManager.instance.current_item_pickup_sounds++;
            }
            StartCoroutine(RemoveDrop());
        }
        else if (Mathf.Abs(beforeCount - count) > 0)
        {
            if (AudioManager.instance.current_item_pickup_sounds < AudioManager.instance.max_item_pickup_sounds_at_once)
            {
                pick_up_sound_effect.enabled = true;
                AudioManager.instance.current_item_pickup_sounds++;
            }
            Debug.Log("Could not loot all");
            has_interacted = false;
        }
    }

    public IEnumerator RemoveDrop()
    {
        yield return StartCoroutine(Fade_Destroy());
        yield return new WaitWhile(() => pick_up_sound_effect.isPlaying);

        if(AudioManager.instance.current_item_pickup_sounds > 0)
            AudioManager.instance.current_item_pickup_sounds--;

        transform.parent.gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
    }
}
