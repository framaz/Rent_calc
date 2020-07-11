using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rent_calc.Events
{
    [Serializable]
    public class CustomWriteOff : WriteOffEvent
    {
        public string commentary;
        public CustomWriteOff(DateTime newDate, int money, double penaltyPercent, string newCommentary) : base(newDate, money, penaltyPercent)
        {
            commentary = newCommentary;
        }
        public override string ToString()
        {
            return date.ToString().Substring(0, 11) + ":" + "Переменная часть: " + moneyGot + "р.";
        }
    }
}
