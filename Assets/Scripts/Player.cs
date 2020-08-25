using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [HideInInspector] public PlayerBackground playerBackground;
    private int _health;
    private int _maxHealth;
    [HideInInspector] public Inventory inventory;
    public SpriteRenderer spriteRenderer;

    private void Awake() {
        inventory = GetComponent<Inventory>();
    }

    public void SetSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

    public void SetHealth(int value) {
        _health = value;
        UIManager.instance.UpdateHealthDisplay(value);
    }

    public int GetHealth() {
        return _health;
    }

    /*
     * Set canTriggerLose to true if the game should be ended if health reaches 0.
     */
    public void AddHealth(int value, bool showPopup = true, bool canTriggerLose = true) {
        if (value != 0) {
            _health += value;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            UIManager.instance.UpdateHealthDisplay(_health);
            if (showPopup) {
                UIManager.instance.ShowHealthPopup(value);
            }
            if (canTriggerLose && _health <= 0) {
                GameManager.instance.EndGame(GameEndType.NO_HEALTH);
            }
        }
    }
    
    public int GetMaxHealth() {
        return _maxHealth;
    }

    public void SetMaxHealth(int value) {
        _maxHealth = value;
    }

    public void AddMaxHealth(int value) {
        if (value != 0) {
            _maxHealth += value;
            _maxHealth = Mathf.Max(_maxHealth, 0);
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            UIManager.instance.UpdateHealthDisplay(_health);
            // TODO: UI manager popup
            if (_maxHealth <= 0) {
                // This shouldn't be possible, but
                GameManager.instance.EndGame(GameEndType.NO_HEALTH);
            }
        }
    }

    public int GetMissingHealth() {
        return _maxHealth - _health;
    }
    
    private bool CanPayCost(Item item, int cost) {
        // Special case: can't pay health/max health cost that would put you below 1 health or 1 max health
        if (item == GameManager.instance.healthItem || item == GameManager.instance.maxHealthItem) {
            return inventory.GetQuantity(item) > cost;
        }
        return inventory.GetQuantity(item) >= cost;
    }
    
    // Returns true iff player can afford the costs in the given list. Note that the quantity values should be positive
    public bool CanPayCost(List<CardEffect> cost) {
        foreach (CardEffect cardEffect in cost) {
            if (!CanPayCost(cardEffect.item, cardEffect.quantity)) {
                return false;
            }
        }
        return true;
    }

    public bool CanApplyEffects(List<CardEffect> get, List<CardEffect> cost) {
        bool canPayCost = CanPayCost(cost);
        bool hasSpace = inventory.HasSpaceForItems(get, cost);
        bool effectsWithinLimit = inventory.EffectsWithinLimit(get, cost);
        return canPayCost && hasSpace && effectsWithinLimit;
    }

    public bool CanApplyEffects(CardEffectsSet cardEffectsSet) {
        List<CardEffect> get = cardEffectsSet.GetReward();
        List<CardEffect> cost = cardEffectsSet.GetCost();
        return CanApplyEffects(get, cost);
    }
}
