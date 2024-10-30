using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        CreateItemButton(Item.GetSprite(Item.ItemType.SpikeBat), "Spike Bat", Item.GetCost(Item.ItemType.SpikeBat), 0);
        CreateItemButton(Item.GetSprite(Item.ItemType.Warhammer), "Warhammer", Item.GetCost(Item.ItemType.Warhammer), 1);
        CreateItemButton(Item.GetSprite(Item.ItemType.Pistol), "Pistol", Item.GetCost(Item.ItemType.Pistol), 2);
        CreateItemButton(Item.GetSprite(Item.ItemType.Shotgun), "Pistol", Item.GetCost(Item.ItemType.Shotgun), 3);
        CreateItemButton(Item.GetSprite(Item.ItemType.Life), "Life Up", Item.GetCost(Item.ItemType.Life), 4);
        CreateItemButton(Item.GetSprite(Item.ItemType.DmgUp), "Damage Up", Item.GetCost(Item.ItemType.DmgUp), 5);
        CreateItemButton(Item.GetSprite(Item.ItemType.Stamina), "Stamina Up", Item.GetCost(Item.ItemType.Stamina), 6);
        CreateItemButton(Item.GetSprite(Item.ItemType.ShotSpeed), "Shot Speed Up", Item.GetCost(Item.ItemType.ShotSpeed), 7);
    }
    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        
        float shopItemHeight = 30f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
    }
}
