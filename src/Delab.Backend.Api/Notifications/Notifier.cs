
namespace Delab.Backend.Api.Notifications;

public class Notifier : INotifier
{
    private ICollection<Notification> _notifications;

    public Notifier()
    {
        _notifications = [];
    }

    public ICollection<Notification> GetNotifications()
    {
        return _notifications;
    }

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public bool HasNotification()
    {
        return _notifications.Any();
    }
}
