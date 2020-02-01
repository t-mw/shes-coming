using InControl;

public class MenuActions : PlayerActionSet
{
    public PlayerAction AdvanceMenu;

    public MenuActions()
    {
        AdvanceMenu = CreatePlayerAction("Advance Menu");
    }
}