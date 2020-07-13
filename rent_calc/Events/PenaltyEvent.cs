using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rent_calc.Logics;

namespace rent_calc.Events
{
    [Serializable]
    public class PenaltyEvent : Event
    {
        public PenaltyEvent(DateTime newDate) : base("penaltyEvent", newDate)
        {
        }
        public override void Apply(Person person)
        {
            foreach (MonthBill month in person.months)
            {
                month.penalty += month.debt * month.penaltyPercentage;
            }
        }
    }
}
