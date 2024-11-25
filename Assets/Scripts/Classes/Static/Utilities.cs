using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    #region Colors
    private const string RED_COLOR = "Red";
    private const string BLUE_COLOR = "Blue";
    private const string YELLOW_COLOR = "Yellow";
    private const string GREEN_COLOR = "Green";
    private const string PURPLE_COLOR = "Purple";
    private const string ORANGE_COLOR = "Orange";

    private const string BLACK_COLOR = "Black";
    private const string WHITE_COLOR = "White";
    private const string GRAY_COLOR = "Gray";
    private const string GOLD_COLOR = "Gold";
    private const string SILVER_COLOR = "Silver";
    private const string CRIMSON_COLOR = "Crimson";
    #endregion

    public static PlayerColor GetPlayerColorByName(string name)
    {
        switch (name)
        {
            case RED_COLOR:
            default:
                return PlayerColor.Red;
            case BLUE_COLOR:
                return PlayerColor.Blue;
            case YELLOW_COLOR:
                return PlayerColor.Yellow;
            case GREEN_COLOR:
                return PlayerColor.Green;
            case PURPLE_COLOR:
                return PlayerColor.Purple;
            case ORANGE_COLOR:
                return PlayerColor.Orange;
        }
    }

    public static BulletColor GetBulletColorByName(string name)
    {
        switch (name)
        {
            case BLACK_COLOR:
            default:
                return BulletColor.Black;
            case WHITE_COLOR:
                return BulletColor.White;
            case GRAY_COLOR:
                return BulletColor.Gray;
            case GOLD_COLOR:
                return BulletColor.Gold;
            case SILVER_COLOR:
                return BulletColor.Silver;
            case CRIMSON_COLOR:
                return BulletColor.Crimson;
        }
    }
}
