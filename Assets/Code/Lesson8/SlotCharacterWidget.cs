using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    public Button SlotButton { get => _button; }
    public bool IsFilled { get; private set; } = false;
    public string Name => _nameLabel.text;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _emptySlot;
    
    [SerializeField]
    private GameObject _infoCharacterSlot;

    [SerializeField]
    private TMP_Text _nameLabel;
    
    [SerializeField]
    private TMP_Text _levelLabel;
    
    [SerializeField]
    private TMP_Text _goldLabel;

    public void SetName(string name)
    {
        _nameLabel.text = $"Name: {name}";
    }

    public void SetStats(string level, string gold)
    {
        _levelLabel.text = $"Level: {level}";
        _goldLabel.text = $"Gold: {gold}";
        _infoCharacterSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowInfoCharacterSlot(string name, string level, string gold)
    {
        IsFilled = true;

        _nameLabel.text = $"Name: {name}";
        _levelLabel.text = $"Level: {level}";
        _goldLabel.text = $"Gold: {gold}";

        _infoCharacterSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        IsFilled = false;
        _infoCharacterSlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}
