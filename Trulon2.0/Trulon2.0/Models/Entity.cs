﻿namespace GameEngine.Models
{
    using System.Collections.Generic;

    public abstract class Entity : GameObject
    {
        protected Entity(
            string name,
            int attackPoints,
            int defencePoints,
            int speedPoints,
            int healthPoints,
            int level,
            List<Item> inventory)
            : base(name)
        {
            this.AttackPoints = attackPoints;
            this.DefencePoints = defencePoints;
            this.SpeedPoints = speedPoints;
            this.HealthPoints = healthPoints;
            this.Level = level;
            this.Inventory = inventory;
        }

        public int AttackPoints { get; set; }
        public int DefencePoints { get; set; }
        public int SpeedPoints { get; set; }
        public int HealthPoints { get; set; }
        public int Level { get; set; }
        public List<Item> Inventory { get; set; }

        protected abstract void Interact();

        protected abstract void Move();

        protected abstract void Die();


    }
}
