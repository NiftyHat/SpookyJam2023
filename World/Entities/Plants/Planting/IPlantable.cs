using System;
using SpookyBotanyGame.World.Entities.Farm.Tillable;

namespace SpookyBotanyGame.World.Entities.Plants.Planting;

public interface IPlantable
{
    public event Action OnDestroyed;
    public void SetSpot(TillableSpot spot);
}