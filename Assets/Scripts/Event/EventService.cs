public class EventService
{
    public EventController OnWinConfetti { get; private set; }
    public EventController OnRestart { get; private set; }
    public EventController OnConfetti { get; private set; }

    public EventService()
    {
        OnWinConfetti = new EventController();
        OnRestart = new EventController();
        OnConfetti = new EventController();
    }
}
