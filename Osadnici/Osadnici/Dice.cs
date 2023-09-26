using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Dice
    {
        public int Number { get; set; }
        private Random randomGenerator;

        public Dice()
        {
            this.randomGenerator = new Random();
        }

        public void Roll()
        {
            int firstDice = randomGenerator.Next(1, 7);
            int secondDice = randomGenerator.Next(1, 7);
            this.Number = firstDice + secondDice;
        }
    }
}
