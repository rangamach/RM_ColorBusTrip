public class EventService
{
    public EventController OnWinConfetti { get; private set; }

    public EventService()
    {
        OnWinConfetti = new EventController();
    }
}
