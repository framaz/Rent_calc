using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rent_calc.Logics;

namespace rent_calc.Events
{
    [Serializable]
    public class ContractChangeEvent : Event
    {
        public Terms terms;
        public DateTime endDate;
        public ContractChangeEvent(DateTime newDate, DateTime newEndDate, Terms newTerms) : base("contractChangeEvent", newDate)
        {
            endDate = newEndDate;
            terms = newTerms;
        }
        public override string ToString()
        {
            return base.ToString() + "Заключен договор аренды";
        }
        public override void Apply(Person person)
        {
            person.terms = new Terms(terms.payment, terms.dateOfAcceptance, terms.dayOfPayment, terms.penalty);
            person.status = 1;
        }
    }
}
