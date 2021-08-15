using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : Window
{

    [Tooltip("Interactable items in vendor window")]
    [SerializeField] private VendorButton[] vendorButtons;
    [Tooltip("Vendor window page number")]
    [SerializeField] private Text vendorPageNo;
    [Tooltip("Vendor window previous and next buttons")]
    [SerializeField] private GameObject prevBtn, nextBtn;
    private Vendor _vendor;
    private List<List<VendorItems>> _vendorPages = new List<List<VendorItems>>();
    private int _pageIndex = 0;



    /// <summary>
    /// Create vendor window pages
    /// </summary>
    /// <param name="vendorItems">Added items</param>
    public void CreatePages(VendorItems[] vendorItems)
    {
        _vendorPages.Clear();
        List<VendorItems> tmpPage = new List<VendorItems>();
        for (int i = 0; i < vendorItems.Length; i++)
        {
            tmpPage.Add(vendorItems[i]);
            if (tmpPage.Count == 10 || i == vendorItems.Length - 1)
            {
                _vendorPages.Add(tmpPage);
                tmpPage = new List<VendorItems>();
            }
        }
        AddItemsToVendor();
    }

    /// <summary>
    /// Add all items to vendor display
    /// </summary>
    public void AddItemsToVendor()
    {
        vendorPageNo.text = _pageIndex + 1 + " / " + _vendorPages.Count;
        prevBtn.SetActive(_pageIndex > 0);
        nextBtn.SetActive(_vendorPages.Count > 1 && _pageIndex < _vendorPages.Count - 1);
        if (_vendorPages.Count > 0)
        {
            for (int i = 0; i < _vendorPages[_pageIndex].Count; i++)
            {
                if (_vendorPages[_pageIndex][i] != null)
                {
                    vendorButtons[i].AddItemToVendorPage(_vendorPages[_pageIndex][i]);
                }
            }
        }
    }

    /// <summary>
    /// Vendor window Next button
    /// </summary>
    public void VendorWNextPage()
    {
        if (_pageIndex < _vendorPages.Count - 1)
        {
            _pageIndex++;
            RefreshPage();
            AddItemsToVendor();
        }
    }

    /// <summary>
    /// Vendor window Previous button
    /// </summary>
    public void VendorWPrevPage()
    {
        if (_pageIndex > 0)
        {
            _pageIndex--;
            RefreshPage();
            AddItemsToVendor();
        }
    }

    /// <summary>
    /// Refresh vendor window
    /// </summary>
    public void RefreshPage()
    {
        foreach (VendorButton btn in vendorButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public override void OpenWindow(NPC npc)
    {
        CreatePages((npc as Vendor).Items);
        base.OpenWindow(npc);
    }
}
