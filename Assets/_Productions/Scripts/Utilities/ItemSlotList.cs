using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotList : MonoBehaviour
{
    [SerializeField]
    private List<Image> items = new();

    public void SetList(Sprite[] datas)
    {
        items.ForEach(item => item.gameObject.SetActive(false));
        
        for (int i = 0; i < datas.Length; i++)
        {
            if (i < items.Count)
            {
                items[i].gameObject.SetActive(true);
                items[i].sprite = datas[i];
            }
            else
            {
                var item = Instantiate(items[0], transform);
                item.sprite = datas[i];
            }
        }
    }
}
