public static class GameState 
{
    // Modes:
    // easy | normal | hard
    // why not enums? see https://forum.unity.com/threads/ability-to-add-enum-argument-to-button-functions.270817/page-3
    private static string difficulty = "easy";

    public static string GameMode
    {
        get {
            return difficulty;
        }

        set {
            difficulty = value;
        }
    }
}