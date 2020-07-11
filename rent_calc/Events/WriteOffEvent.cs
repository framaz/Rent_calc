using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace rent_calc.Events
{
    [Serializable]
    public class WriteOffEvent : Event
    {
        public int moneyGot;
        public double penalty;
        public override bool ToRow(IRow row)
        {
            base.ToRow(row);
            row.CreateCell(1).SetCellValue(moneyGot);
            return true;
        }
        public WriteOffEvent(DateTime newDate, int money, double penaltyPercent) : base("writeOffEvent", newDate)
        {
            moneyGot = money;
            penalty = penaltyPercent;
        }
        public override string ToString()
        {
            return base.ToString() + "Снятие средств с арендатора " + moneyGot + "р.";
        }
        public override void Apply(Person person)
        {
            int realMoney = moneyGot;
            if (person.overpayment > 0)
            {
                if (realMoney < person.overpayment)
                {
                    person.overpayment -= realMoney;
                    return;
                }
                else
                {
                    realMoney -= person.overpayment;
                    person.overpayment = 0;
                }
            }
            person.months.Add(new MonthBill(date, realMoney, penalty));
        }
    }
}
