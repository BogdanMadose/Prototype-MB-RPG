using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow _instance;
    public static LootWindow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LootWindow>();
            }
            return _instance;
        }
    }

    [Tooltip("Loot game objects")]
    [SerializeField] private LootButton[] lootButtons;
    [Tooltip("Loot window page number")]
    [SerializeField] private Text pageNumber;
    [Tooltip("Loot window previous and next buttons")]
    [SerializeField] private GameObject prevBtn, nextBtn;
    private CanvasGroup _canvasGroup;
    private List<Item> _droppedLoot = new List<Item>();
    private List<List<Item>> _lootPages = new List<List<Item>>();
    private int _pageIndex = 0;
    [SerializeField] private Item[] items; // TODO: Remove after testing

    public bool IsOpened => _canvasGroup.alpha > 0;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Create the loot window pages
    /// </summary>
    /// <param name="items">List of items to be looted</param>
    public void CreatePages(List<Item> items)
    {
        if (!IsOpened)
        {
            List<Item> tmpPage = new List<Item>();
            _droppedLoot = items;
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
            OpenLootWindow();
        }
    }

    /// <summary>
    /// Add loot
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
    /// Refresh loot items
    /// </summary>
    public void RefreshPage()
    {
        foreach (LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Next loot page
    /// </summary>
    public void LootWNextPage()
    {
        if (_pageIndex < _lootPages.Count - 1)
        {
            _pageIndex++;
            RefreshPage();
            AddLoot();
        }
    }

    /// <summary>
    /// Previous loot page
    /// </summary>
    public void LootWPrevPage()
    {
        if (_pageIndex > 0)
        {
            _pageIndex--;
            RefreshPage();
            AddLoot();
        }
    }

    /// <summary>
    /// Transfer to bag
    /// </summary>
    /// <param name="item">Looted item</param>
    public void TakeLoot(Item item)
    {
        _lootPages[_pageIndex].Remove(item);
        _droppedLoot.Remove(item);
        if (_lootPages[_pageIndex].Count == 0)
        {
            _lootPages.Remove(_lootPages[_pageIndex]);
            if (_pageIndex == _lootPages.Count && _pageIndex > 0)
            {
                _pageIndex--;
            }
            AddLoot();
        }
    }

    /// <summary>
    /// Close loot window
    /// </summary>
    public void CloseLootWindow()
    {
        _lootPages.Clear();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        RefreshPage();
    }

    /// <summary>
    /// Open up loot windpow
    /// </summary>
    public void OpenLootWindow()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }
}
