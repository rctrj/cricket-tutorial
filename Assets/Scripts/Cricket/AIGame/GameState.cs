namespace Cricket.AIGame
{
    [System.Serializable]
    public enum GameState
    {
        Idle,
        SettingAim,
        SettingPower,
        BallThrown,
        BallHit,
        InningsChange,
    }
}