using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData 
{

    public string PlayerName;
    public PlayerType Type;
    public Color PlayerColor;
    public bool IsGhost;

    public PlayerData(string playerName, PlayerType type, Color color, bool isGhost)
    {
        PlayerName = playerName;
        Type = type;
        PlayerColor = color;
        IsGhost = isGhost;
    }
}

