using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    [SerializeField] private LootButton[] lootButtons;
    [SerializeField] private Text pageNumber;
    [SerializeField] private GameObject prevBtn, nextBtn;
    private List<List<Item>> _lootPages = new List<List<Item>>();
    private int _pageIndex = 0;
    [SerializeField] private Item[] items; // TODO: Remove after testing

    // Start is called before the first frame update
    void Start()
    {
        List<Item> tmp = new List<Item>();
        for (int i = 0; i < items.Length; i++)
        {
            tmp.Add(items[i]);
        }
        CreatePages(tmp);
    }

    /// <summary>
    /// Create the loot window
    /// </summary>
    /// <param name="items">List of items to be looted</param>
    public void CreatePages(List<Item> items)
    {
        List<Item> tmpPage = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            tmpPage.Add(items[i]);
            if (tmpPage.Count == 4 || i == items.Count - 1)
            {
                _lootPages.Add(tmpPage);
                tmpPage = new List<Item>();
            }
        }
        AddLoot();
    }

    /// <summary>
    /// Adds loot to the loot window
    /// </summary>
    private void AddLoot()
    {
        if (_lootPages.Count > 0)
        {
            pageNumber.text = _pageIndex + 1 + " / " + _lootPages.Count;
            prevBtn.SetActive(_pageIndex > 0);
            nextBtn.SetActive(_lootPages.Count > 1 && _pageIndex < _lootPages.Count - 1);
            for (int i = 0; i < _lootPages[_pageIndex].Count; i++)
            {
                if (_lootPages[_pageIndex][i] != null)
                {
                    lootButtons[i].Icon.sprite = _lootPages[_pageIndex][i].Icon;
                    lootButtons[i].Loot = _lootPages[_pageIndex][i];
                    lootButtons[i].gameObject.SetActive(true);
                    string title = string.Format("<color={0}>{1}</color>",
                        QualityColor.Colors[_lootPages[_pageIndex][i].Quality], _lootPages[_pageIndex][i].Title);
                    lootButtons[i].Title.text = title;
                }
            }
        }
    }

    /// <summary>
    /// Refreshes page
    /// </summary>
    public void RefreshPage()
    {
        foreach(LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Next button functionality
    /// </summary>
    public void NextPage()
    {
        if (_pageIndex < _lootPages.Count - 1)
        {
            _pageIndex++;
            RefreshPage();
            AddLoot();
        }
    }

    /// <summary>
    /// Previous button functionality
    /// </summary>
    public void PrevPage()
    {
        if ( _pageIndex > 0)
        {
            _pageIndex--;
            RefreshPage();
            AddLoot();
        }
    }
}
