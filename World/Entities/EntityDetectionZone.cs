using System;
using System.Collections.Generic;
using Godot;

namespace SpookyBotanyGame.World.Entities;

[GlobalClass]
public partial class EntityDetectionZone : Area2D
{
    public abstract class Detector
    {
        public abstract void Add(GameEntity gameEntity);
        public abstract void Remove(GameEntity gameEntity);
        public abstract void Process();
        public abstract bool CanDetect(GameEntity gameEntity);
    }
    public class Detector<TEntity> : Detector where TEntity : GameEntity
    {
        private readonly HashSet<GameEntity> _items = new HashSet<GameEntity>();
        
        public Action<TEntity> OnEnter;
        public Action<TEntity> OnExit;
        public Action<TEntity> OnOverlap;

        public override void Add(GameEntity gameEntity)
        {
            OnEnter?.Invoke(gameEntity as TEntity);
            _items.Add(gameEntity);
        }

        public override void Remove(GameEntity gameEntity)
        {
            OnExit?.Invoke(gameEntity as TEntity);
            _items.Remove(gameEntity);
        }

        public override void Process()
        {
            foreach (var item in _items)
            {
                OnOverlap?.Invoke(item as TEntity);
            }
        }

        public override bool CanDetect(GameEntity gameEntity)
        {
            return gameEntity is TEntity;
        }
    }

    public bool IsEnabled { get; private set; } = true;
    
    private Detector _detector;
    
    [Export] public bool IsLogging { get; set; }

    public void SetEnabled(bool isEnabled)
    {
        if (IsEnabled != isEnabled)
        {
            IsEnabled = isEnabled;
        }
        //TODO - disable monitoring when enabled is false.
        Monitoring = IsEnabled;
    }

    public void Set(Detector detector)
    {
        _detector = detector;
    }

    public override void _Ready()
    {
        base._Ready();
        Monitoring = true;
        BodyEntered += HandleBodyEntered;
        BodyExited += HandleBodyExited;
    }

    public override void _Process(double delta)
    {
        _detector?.Process();
    }

    private void HandleBodyEntered(Node2D body)
    {
        if (body.IsInGroup("EntityProvider") && body.HasMethod("GetEntity"))
        {
            GameEntity gameEntity = body.Call("GetEntity").As<GameEntity>();
            if (gameEntity != null)
            {
                GD.Print(gameEntity.Name, " HandleBodyEntered");
            }
            
            if (_detector != null && _detector.CanDetect(gameEntity))
            {
                _detector.Add(gameEntity);
            }
        }
    }

    private void HandleBodyExited(Node2D body)
    {
        if (body.IsInGroup("EntityProvider") && body.HasMethod("GetEntity"))
        {
            GameEntity gameEntity = body.Call("GetEntity").As<GameEntity>();
            if (IsLogging && gameEntity != null)
            {
                GD.Print(gameEntity.Name, " HandleBodyExited");
            }
            if (_detector != null && _detector.CanDetect(gameEntity))
            {
                _detector.Remove(gameEntity);
            };
        }
    }
}