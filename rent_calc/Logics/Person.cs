using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using rent_calc.Events;

namespace rent_calc.Logics
{
    [Serializable]
    public class Person : Object
    {
        public string personName, description;
        public Terms terms;
        public List<MonthBill> months;
        public BindingList<Event> events;
        public double penalty;
        public int addEvent(Event newEvent)
        {
            //TODO переделать
            int pointer = 0;
            if (events.Count == 0)
            {
                events.Add(newEvent);
                return 0;
            }
            for (; pointer < events.Count && events[pointer].date <= newEvent.date; pointer++)
                ;
            if (pointer == 0)
                return 1;
            events.Insert(pointer, newEvent);
            return 0;
        }
        public void generateWithdrawEvents(DateTime target)
        {
            DateTime nextPaymentDate = start;
            int pointer = 0;
            for (int i = 0; i < events.Count; i++)
            {
                Event curEvent = events[i];
                if (curEvent.GetType() == typeof(WriteOffEvent))
                {
                    events.Remove(curEvent);
                    i--;
                }
            }
            while (nextPaymentDate <= target)
            {
                bool isPayed = false;
                for (; pointer < events.Count && events[pointer].date <= nextPaymentDate; pointer++)
                {
                    if (events[pointer].getType() == "leaveEvent")
                    {
                        events[pointer].Apply(this);
                    }
                    if (events[pointer].getType() == "paymentChangeEvent")
                        events[pointer].Apply(this);
                    if (events[pointer].getType() == "contractChangeEvent")
                    {
                        events[pointer].Apply(this);
                        nextPaymentDate = new DateTime(
                            terms.dateOfAcceptance.Year,
                            terms.dateOfAcceptance.Month,
                            terms.dayOfPayment.Day);
                        if (nextPaymentDate < terms.dateOfAcceptance)
                            nextPaymentDate = nextPaymentDate.AddMonths(1);
                    }

                }
                if (!isPayed && status == 1)
                {
                    events.Insert(pointer, new WriteOffEvent(nextPaymentDate, terms.payment, terms.penalty));
                    pointer++;
                }
                nextPaymentDate = nextPaymentDate.AddMonths(1);
            }
        }
        private DateTime start;
        public int overpayment;
        public int status;

        public Person(string newName, string newDescription, DateTime newStart)
        {
            description = newDescription;
            status = 1;
            personName = newName;
            start = newStart;
            months = new List<MonthBill>();
            events = new BindingList<Event>();
        }
        public override string ToString()
        {
            return personName;
        }
        public Report Simulate(DateTime target)
        {
            DateTime currentDate = start;
            Report report = new Report();
            overpayment = 0;
            generateWithdrawEvents(target);
            if (currentDate > target)
            {
                report.stringReport = "Неверная дата";
                return report;
            }
            List<Event> eventHistory = new List<Event>();
            months.Clear();
            int pointer = 0;

            for (; currentDate.Date <= target; currentDate = currentDate.AddDays(1))
            {
                PenaltyEvent penaltyEvent = new PenaltyEvent(currentDate);
                penaltyEvent.Apply(this);
                eventHistory.Add(penaltyEvent);
                for (; pointer < events.Count && events[pointer].date <= currentDate; pointer++)
                {
                    eventHistory.Add(events[pointer]);
                    events[pointer].Apply(this);
                }
            }
            foreach (MonthBill bill in months)
            {
                report.totalDepth += bill.debt;
                report.totalPenalty += bill.penalty;
            }
            if (report.totalDepth == 0 && report.totalPenalty == 0)
                report.totalDepth = -overpayment;
            return report;
        }
    }
}
