namespace SCP079SoC.Models.Entities;

public class UserEntity
{
    public ulong Id { get; set; }

    public int Reputation { get; set; }

    public long UsedRepTime { get; set; }

    internal UserEntity()
    {
    }
    
    internal UserEntity(ulong id)
    {
        Id = id;
    }
}