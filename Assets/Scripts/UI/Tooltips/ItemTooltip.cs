using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : BasicTooltip {
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sellText;
    public TextMeshProUGUI description;
    [SerializeField, Tooltip("The ContentSizeFitters at the lowest levels of this object.")]
    private ContentSizeFitter[] _contentSizeFitterLeaves = null;
    
    public void SetTooltip(Item item) {
        if (!item.resourceItem) {
            nameText.transform.parent.gameObject.SetActive(true);
            sellText.transform.parent.gameObject.SetActive(true);
            nameText.text = item.itemName;
            sellText.text = "sells for " + item.sellValue;
        } else {
            nameText.transform.parent.gameObject.SetActive(false);
            sellText.transform.parent.gameObject.SetActive(false);
        }
        description.text = item.BuildTooltipDescription();
        Rebuild();
    }

    public void Rebuild() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_background);
        Canvas.ForceUpdateCanvases();
        _background.GetComponent<VerticalLayoutGroup>().enabled = false;
        _background.GetComponent<VerticalLayoutGroup>().enabled = true;
        foreach(ContentSizeFitter contentSizeFitter in _contentSizeFitterLeaves) {
            contentSizeFitter.SetLayoutVertical();
        }
        FollowCursor();
    }
}
