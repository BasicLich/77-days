using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    // TODO // really should use a proper graph. at least it's constant time
    public List<LocationNode> locations;
    private Dictionary<Location, LocationNode> _nodeDict = new Dictionary<Location, LocationNode>();
    [Tooltip("Index of start location in locations list.")]
    public int startLocationIndex;
    [HideInInspector] public Location currLocation;
    [SerializeField] private float _slideSpeed = 0;
    private bool _canMove;

    private void Awake() {
        InitializeLocationNodes();
        currLocation = locations[startLocationIndex].location;
    }

    private void Start() {
        GameManager.instance.player.transform.position = currLocation.stopPoint.transform.position;
    }

    private void InitializeLocationNodes() {
        foreach (LocationNode locationNode in locations) {
            locationNode.InitializeDictionary();
            _nodeDict.Add(locationNode.location, locationNode);
        }
    }

    // Show destinations and costs and allow movement, or the opposite, depending on allow param
    public void SetAllowMovement(bool allow) {
        if (allow) {
            _canMove = true;
            // foreach (LocationNode locationNode in locations) {
            //     if (locationNode.location != currLocation && locationNode.location.DestinationAvailable()) {
            //         locationNode.location.ShowCost(currLocation);
            //     }
            // }
            UIManager.instance.SetNighttimeOptions(true);
        } else {
            _canMove = false;
            UIManager.instance.SetNighttimeOptions(false);
            // foreach (LocationNode locationNode in locations) {
            //     locationNode.location.HideCost();
            // }
        }
        UpdateDestinations();
    }

    public void UpdateDestinations() {
        foreach (LocationNode locationNode in locations) {
            if (_canMove && locationNode.location != currLocation && locationNode.location.DestinationAvailable()) {
                // TODO: some kinda animation?
                locationNode.location.ShowCost(currLocation);
            } else {
                locationNode.location.HideCost();
            }
        }
    }

    public int GetTravelCost(Location origin, Location destination) {
        return _nodeDict[origin].GetTravelCost(destination);
    }

    public IEnumerator MoveToLocation(Location destination) {
        if (!_canMove) {
            yield break;
        }
        if (currLocation == destination) {
            Debug.Log("Invalid destination: player is already at " + destination.locationType);
            yield break;
        }
        if (!destination.DestinationAvailable()) {
            Debug.Log("Invalid destination: " + destination.locationType + " is not available");
            yield break;
        }
        int travelCost = GetTravelCost(currLocation, destination);
        if (travelCost > GameManager.instance.player.inventory.GetQuantity(GameManager.instance.foodItem)) {
            Debug.Log("Invalid destination: player cannot afford the trip");
            UIManager.instance.ShowTextPopup(Constants.INSUFFICIENT_FOOD, Input.mousePosition);
            yield break;
        }

        // Pay cost
        GameManager.instance.player.inventory.AddItem(GameManager.instance.foodItem, -travelCost);

        currLocation = destination;
        SetAllowMovement(false);
        yield return StartCoroutine(Slide(GameManager.instance.player.transform, destination.stopPoint.transform));
        GameManager.instance.TriggerNextDay();
    }

    public void TeleportToRandomLocation() {
        Location destination = GetRandomOtherLocation();
        currLocation = destination;
        GameManager.instance.player.transform.position = destination.stopPoint.transform.position;
    }

    private Location GetRandomOtherLocation() {
        Location destination = currLocation;
        int attempts = 0;
        while ((destination == currLocation || destination.locationType == LocationType.HUT) && attempts < Constants.instance.loopLimit) {
            destination = locations[Random.Range(0, locations.Count)].location;
            attempts++;
        }
        return destination;
    }

    private IEnumerator Slide(Transform transform, Transform destination) {
        float elapsedTime = 0;
        float t = 0;
        Vector3 start = transform.position;
        Vector3 end = destination.position;
        float slideDuration = Vector3.Distance(start, end) / _slideSpeed;
        while (transform.position != destination.position) {
            elapsedTime += Time.deltaTime;
            t = Mathf.SmoothStep(0, 1, elapsedTime / slideDuration);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
}
