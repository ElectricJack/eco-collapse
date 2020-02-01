﻿namespace EntitySystem
{
    class Poisonable : BaseComponent, ISteppable
    {
        private int poisonVal = 0;

        public void OnPoison(int potency)
        {
            poisonVal += potency;
        }

        public void Step()
        {
            if (poisonVal > 0)
            {
                entity.deathAge -= poisonVal;
                poisonVal--;
            }
        }
    }
}

