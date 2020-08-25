using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : Singleton<Constants> {
    [Tooltip("Maximum number of times a loop can iterate while attempting to randomly choose something.")]
    public int loopLimit;

    public int shiftClickQuantity = 10;
    public const string INSUFFICIENT_FOOD = "insufficient food";
    public const string CANNOT_AFFORD = "cannot afford";
    public const string NO_INV_SPACE = "not enough inventory space";
    public const string LIMIT_ONE = "limited to one";
    public const string LIMITED_ITEM = "limited item";
    public const string NEED_ITEM = "need {0}";

}