using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rent_calc.Logics;

namespace rent_calc.Events
{
    [Serializable]
    public class PaymentChangeEvent : Event
    {
        public int payment;
        public PaymentChangeEvent(DateTime newDate, int newPayment) : base("paymentChangeEvent", newDate)
        {
            payment = newPayment;
        }
        public override string ToString()
        {
            return base.ToString() + "Изменение оплаты, теперь " + payment + "р.";
        }
        public override void Apply(Person person)
        {
            person.terms.payment = payment;
        }
    }
}
