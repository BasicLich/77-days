using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static string GetFormattedDays(int days) {
        if (days == 1) {
            return days + " day";
        } else {
            return days + " days";
        }
    }
}
