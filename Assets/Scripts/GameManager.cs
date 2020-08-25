using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus {
    MENU,
    PLAYING,
    END_SCREEN
}

public enum GameEndType {
    // Winning endings
    SHIP_ESCAPE,
    MAGIC_MISSILE,
    // Losing endings
    NO_HEALTH,
    STARVATION,
    DEBT,
    THEY_ARRIVE
}

public class GameManager : Singleton<GameManager> {
    
    [Header("References")]
    public Player player = null;
    private PlayerBackground _playerBackground;
    public Market market;
    public Map map;
    [SerializeField] private LightController _lightController = null;

    [Header("Key Items")]
    public Item goldItem;
    public Item healthItem;
    public Item maxHealthItem;
    public Item dayItem;
    public Item foodItem;
    public Item vendingLicence;
    public Item inventoryUpgradeItem;
    public Item cauldronItem;

    [Header("Game Parameters")]
    [Tooltip("The final day. After the night of this day, the player will lose.")]
    public int finalDay;
    public int maximumDebt;
    [Tooltip("The health restored when eating a full meal.")]
    public int mealHeal;
    
    [Header("Flags")]
    public bool FLAG_TOWN_TUTORIAL_SEEN = false;
    public bool FLAG_GATHERING_TUTORIAL_SEEN = false;
    public bool FLAG_NIGHT_TUTORIAL_SEEN = false;
    public bool FLAG_HUNGRY_TRAVELLER_SEEN = false;
    public bool FLAG_HUNGRY_TRAVELLER_FED = false;
    public bool FLAG_GRATEFUL_TRAVELLER_SELECTED = false;

    [Space]
    public int day = 1;
    public GameStatus gameStatus;
    [Tooltip("The ship's starting integrity, as a percentage.")]
    public int shipIntegrity = 50;
    [Tooltip("The progress made with each repair.")]
    public int shipRepairIncrement = 5;
    [Tooltip("The progress lost with each dismantle.")]
    public int shipRepairDecrement = 20;
    public bool eatAtNight = true;
    public bool noTimeLimit = false;
    
    [Header("Debug")]
    public DebugSettings debug;

    private void Start() {
        if (debug.debugOn) {
            debug.ApplyDebug();
        } else {
            debug.DisableDebug();
        }
        
        gameStatus = GameStatus.MENU;

        // Initialize market by opening canvas. TODO?: make less jammy
        map.currLocation.PerformActivity();
    }

    public void StartGame() {
        // TODO // Jam code: scroll to top now that market is initialized and the layout is rebuilt
        market.ScrollToTop();

        _playerBackground = player.playerBackground;
        gameStatus = GameStatus.PLAYING;
        StartDay();
    }

    private void StartDay() {
        UIManager.instance.SetMainMenu(false);
        UIManager.instance.UpdateDayDisplay(day);
        _lightController.RaiseSun();
        map.currLocation.PerformActivity();
    }
    
    public void StartNight() {
        _lightController.SetSun();
        map.SetAllowMovement(true);
        UIManager.instance.tutorials.ShowNighttimeTutorial();
    }

    public void AllowMovement() {
        map.SetAllowMovement(true);
    }

    public void TriggerNextDay() {
        StartCoroutine(TransistionToNextDay());
    }

    private IEnumerator TransistionToNextDay() {
        map.SetAllowMovement(false);
        UIManager.instance.SetNighttimeOptions(false);
        day++;
        // Check for game over
        if (!TryTimeoutGame()) {
            yield return StartCoroutine(PayExpenses());
            StartDay();
        }
    }

    public void AddDays(int quantity) {
        day += quantity;
        UIManager.instance.UpdateDayDisplay(day);
        TryTimeoutGame();
    }

    // Returns true if days has passed the day limit and ends the game if so
    private bool TryTimeoutGame() {
        if (day > finalDay && !noTimeLimit) {
            EndGame(GameEndType.THEY_ARRIVE);
            return true;
        }
        return false;
    }

    public void SkipGathering() {
        ((GatheringLocation) map.currLocation).ProcessGather();
    }

    private IEnumerator PayExpenses() {
        yield return new WaitForSeconds(0.25f);
        
        // Process items that have an effect at the end of the night
        Dictionary<NightEffectItem, int> nightEffectItems = player.inventory.GetNightEffectItems();
        int itemsProcessed = 0;
        foreach (KeyValuePair<NightEffectItem, int> item in nightEffectItems) {
            for (int i = 0; i < item.Value; i++) {
                item.Key.TryPerformNightEffect();
                if (itemsProcessed != nightEffectItems.Count && i != item.Value - 1) {
                    // Apply wait if this item is not the last item of the last item type
                    yield return new WaitForSeconds(0.3f);
                }
            }
            itemsProcessed++;
        }

        // Check if the player has lost from debt
        if (player.inventory.GetGold() < -maximumDebt) {
            EndGame(GameEndType.DEBT);
        }

        yield return new WaitForSeconds(0.5f);

        int expense = _playerBackground.foodExpense;
        if (expense > 0) {
            int foodCount = player.inventory.GetQuantity(foodItem);
            int foodToEat = eatAtNight ? Mathf.Min(foodCount, expense) : 0;
            // TODO: delay food expense a bit to separate it more from travel cost anim maybe?
            if (foodToEat >= expense) {
                player.inventory.AddItem(foodItem, -expense);
                int healAmount = mealHeal;
                if (player.inventory.HasItem(cauldronItem)) {
                    healAmount += cauldronItem.effectPower;
                }
                player.AddHealth(healAmount);
            } else {
                // Impose food penalty
                int difference = expense - foodToEat;
                player.inventory.AddItem(foodItem, -(expense - difference));
                player.AddHealth(-difference, true, false);
                if (player.GetHealth() <= 0) {
                    EndGame(GameEndType.STARVATION);
                    yield break;
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
    }

    public void OnInventoryChanged() {
        map.UpdateDestinations();
        map.currLocation.RefreshLocation();
    }

    public int CalculateShipIntegrityChange(bool repair) {
        return shipIntegrity + (repair ? shipRepairIncrement : -shipRepairDecrement);
    }

    public void AddShipIntegrity(bool repair) {
        shipIntegrity = CalculateShipIntegrityChange(repair);
    }

    public bool ShipFixed() {
        return shipIntegrity >= 100;
    }

    public void EndGame(GameEndType gameEndType) {
        if (gameStatus == GameStatus.PLAYING) {
            gameStatus = GameStatus.END_SCREEN;
            UIManager.instance.ShowEndScreen(gameEndType);
            /*
            switch (gameEndType) {
                case (GameEndType.SHIP_ESCAPE):
                    Debug.Log("GAME WIN: ship escape");
                    break;
                case (GameEndType.MAGIC_MISSILE):
                    Debug.Log("GAME WIN: magic missile");
                    break;
                case (GameEndType.NO_HEALTH):
                    Debug.Log("GAME OVER: you ran out of health");
                    break;
                case (GameEndType.STARVATION):
                    Debug.Log("GAME OVER: you ran out of food");
                    break;
                case (GameEndType.DEBT):
                    Debug.Log("GAME OVER: debt");
                    break;
                default:
                    Debug.LogWarning("Unhandled GameEndType");
                    break;
            }
            */
        }
    }

    public void RestartGame() {
        if (gameStatus == GameStatus.END_SCREEN) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
