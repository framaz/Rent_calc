using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rent_calc.Logics;

namespace rent_calc.Events
{
    [Serializable]
    public class LeaveEvent : Event
    {
        public LeaveEvent(DateTime newDate) : base("leaveEvent", newDate)
        {

        }
        public override string ToString()
        {
            return base.ToString() + "Арендатор покинул помещение";
        }
        public override void Apply(Person person)
        {
            person.status = 0;
        }
    }
}
