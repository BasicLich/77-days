using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject {
    [TextArea(10, 10)] public string dialogue;

    public virtual string GetDialogue() {
        return dialogue;
    }
}
