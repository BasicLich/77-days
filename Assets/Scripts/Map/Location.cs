using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationType {
    TOWN,
    FOREST,
    LAKE,
    HUT,
    CRASH_SITE
}

public class Location : MonoBehaviour {
    private Map map;

    [Tooltip("Transform marking the position of where the player goes if they move to this destination.")]
    public Transform stopPoint;
    public LocationType locationType;
    public GameObject locationCanvas;
    public DestinationCanvas destinationCanvas;

    protected virtual void Start() {
        map = GameManager.instance.map;
    }

    public IEnumerator TryMoveHere() {
        yield return StartCoroutine(map.MoveToLocation(this));
    }

    public virtual bool DestinationAvailable() {
        return true;
    }

    public virtual void PerformActivity() {
        OpenCanvas(true);
    }

    public virtual void EndActivity() {
        OpenCanvas(false);
        GameManager.instance.StartNight();
    }

    public virtual bool CanRemain() {
        return true;
    }

    public void OpenCanvas(bool open) {
        locationCanvas.SetActive(open);
    }

    public virtual void RefreshLocation() {
        
    }

    // Show the destination canvas with the updated cost
    public void ShowCost(Location originLocation) {
        int travelCost = GameManager.instance.map.GetTravelCost(originLocation, this);
        destinationCanvas.SetCost(travelCost);
        bool canAfford = GameManager.instance.player.inventory.GetQuantity(GameManager.instance.foodItem) >= travelCost;
        destinationCanvas.SetEnabled(canAfford);
        destinationCanvas.gameObject.SetActive(true);
    }

    public void HideCost() {
        destinationCanvas.gameObject.SetActive(false);
    }

    private void OnMouseDown() {
        StartCoroutine(TryMoveHere());
    }
}
