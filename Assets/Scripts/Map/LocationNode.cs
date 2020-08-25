using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Edge {
    public Location location;
    public int cost;

    public Edge(Location location, int cost) {
        this.location = location;
        this.cost = cost;
    }
}

[System.Serializable]
public class LocationNode {
    public Location location;
    public List<Edge> edges;
    private Dictionary<Location, int> costDict = new Dictionary<Location, int>();

    public void InitializeDictionary() {
        foreach (Edge edge in edges) {
            costDict.Add(edge.location, edge.cost);
        }
    }

    public int GetTravelCost(Location destination) {
        if (!costDict.ContainsKey(destination)) {
            Debug.LogWarning(location + " has malformed travel costDict");
            return 0;
        }
        return costDict[destination];
    }
}
