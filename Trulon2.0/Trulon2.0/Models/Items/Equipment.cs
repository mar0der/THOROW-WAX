﻿using GameEngine.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Models.Items
{
    public abstract class Equipment : Item
    {
        protected Equipment(string name, Texture2D image, Rectangle bounds, EquipmentSlots slot)
            : base(name, image, bounds)
        {
            this.Slot = slot;
        }

        public EquipmentSlots Slot { get; set; }
    }
}
