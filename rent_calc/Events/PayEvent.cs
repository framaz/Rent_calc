using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using rent_calc.Logics;

namespace rent_calc.Events
{
    [Serializable]
    public class PayEvent : Event
    {
        public int moneyPaid;
        public override bool ToRow(IRow row)
        {
            base.ToRow(row);
            row.CreateCell(2).SetCellValue(moneyPaid);
            return true;
        }
        public PayEvent(DateTime newDate, int money) : base("payEvent", newDate)
        {
            moneyPaid = money;
        }
        public override string ToString()
        {
            return base.ToString() + "Оплачено " + moneyPaid + "р.";
        }
        public override void Apply(Person person)
        {
            int curMoney = moneyPaid + person.overpayment;
            while (curMoney > 0 && person.months.Count > 0)
            {
                if (curMoney > person.months[0].debt)
                {
                    curMoney -= (int)Math.Round(person.months[0].debt);
                    person.months[0].debt = 0;
                }
                else
                {
                    person.months[0].debt -= curMoney;
                    curMoney = 0;
                }
                if (curMoney > person.months[0].penalty)
                {
                    curMoney -= (int)Math.Round(person.months[0].penalty);
                    person.months[0].penalty = 0;
                }
                else
                {
                    person.months[0].penalty -= curMoney;
                    curMoney = 0;
                }
                if (person.months[0].debt == 0 && person.months[0].penalty == 0)
                    person.months.RemoveAt(0);
            }
            if (curMoney > 0)
                person.overpayment = curMoney;
        }
    }
}
