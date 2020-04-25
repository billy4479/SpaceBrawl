
public interface IHealth
{
    void OnDeath(bool defeated);
    bool IsPlayer();

    object GetStats();
}