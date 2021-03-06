﻿namespace Trulon.Models.Entities.NPCs.Enemies
{
    using Microsoft.Xna.Framework;
    using global::Trulon.Enums;

    public class Ogre : Enemy
    {
        #region Constants
        private const Names DefaultName = Names.Ogre;
        private const int DefaultAttackPoints = 9;
        private const int DefaultDefensePoints = 10;
        private const int DefaultSpeedPoints = 3;
        private const int DefaultHealthPoints = 90;
        private const int DefaultAttackRadius = 40;
        private const int DefaultLevel = 4;
        private const int DefaultExperienceReward = 90;
        private const int DefaultCoinsReward = 60;
        private const int DefaultWidth = 96;
        private const int DefaultHeight = 128;
        #endregion

        public Ogre(int x, int y)
        {
            this.Name = DefaultName;
            this.BaseAttack = DefaultAttackPoints;
            this.BaseDefense = DefaultDefensePoints;
            this.BaseSpeed = DefaultSpeedPoints;
            this.BaseHealth = DefaultHealthPoints;
            this.Level = DefaultLevel;
            this.ExperienceReward = DefaultExperienceReward;
            this.CoinsReward = DefaultCoinsReward;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
            this.Position = new Vector2(x, y);
            this.Bounds = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + Width, y + Height, 0));
            this.BaseAttackRadius = DefaultAttackRadius;
            this.IsAlive = true;
        }
    }
}
