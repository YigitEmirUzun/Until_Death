using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(4,10)]
    public string[] sentences;
}
