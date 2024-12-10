using HarvestValley.Graphics;
using HarvestValley.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.System
{
    public class MoneyManager
    {
        private int moneyAmount { get; set; }
        public MoneyManager(int _moneyAmount)
        {
            this.moneyAmount = _moneyAmount;
        }


        public void Add(int _amount)
        {
            moneyAmount += _amount;
        }

        public void Remove(int _amount)
        {
            moneyAmount -= _amount;
        }

        public int getCurrent()
        {
            return moneyAmount;
        }
    }
}
