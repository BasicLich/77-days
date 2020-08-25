using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Very early testing script
public class Testing : MonoBehaviour {
    public Inventory inventory;

    public void AddGold() {
        inventory.AddGold(1);
    }
    
    public void AddFood() {
        inventory.AddItem(GameManager.instance.foodItem, 1);
    }

    public void DoActivity() {
        GameManager.instance.StartNight();
        int rolls = Random.Range(0, 5);
        for (int i = 0; i < rolls; i++) {
            AddGold();
        }
        rolls = Random.Range(0, 5);
        for (int i = 0; i < rolls; i++) {
            AddFood();
        }
    }
}
