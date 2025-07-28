namespace Delab.Backend.Api.Notifications;

public interface INotifier
{
    bool HasNotification();
    ICollection<Notification> GetNotifications();
    void Handle(Notification notification);
}
