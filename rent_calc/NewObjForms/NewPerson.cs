using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using rent_calc.Logics;

namespace rent_calc.NewObjectForms
{
    public partial class NewPerson : AbstractNew
    {
        public NewPerson() : base()
        {
            InitializeComponent();
        }
        public NewPerson(string address) : base()
        {
            InitializeComponent();
            label11.Text = address;
            endTermMonthCalendar.MinDate = startTermMonthCalendar.SelectionStart;
        }
        protected override GenericNewHelper getContents()
        {
            if (nameTextBox.Text.Length < 3)
                return new ErrorHelper("", "Введие ФИО более 3 букв");
            if (endTermMonthCalendar.SelectionStart <= startTermMonthCalendar.SelectionStart)
                return new ErrorHelper("", "Договор оканчивается раньше, чем начинается");
            return new NewPersonHelper(
                nameTextBox.Text,
                descriptionTextBox.Text,
                startTermMonthCalendar.SelectionStart,
                endTermMonthCalendar.SelectionStart,
                (int)rentSumUpDown.Value, 
                (int)paymentDayUpDown.Value,
                (double)penaltyUpDown.Value);
        }
        private void endTermMonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            //    DateTime date = eventDateMonthCalendar.SelectionStart;
            //     label4.Text = date.ToString().Substring(0,11);
            endTermMonthCalendar.MinDate = startTermMonthCalendar.SelectionStart;
        }
    }
    public class NewPersonHelper : GenericNewHelper
    {
        public Terms terms;
        public DateTime endDate;
        public NewPersonHelper(
            string name, 
            string description,
            DateTime newDate, 
            DateTime newEndDate,
            int newRent, 
            int day, 
            double newPenalty) : base(name, description)
        {
            endDate = newEndDate;
            DateTime payDay = new DateTime(newDate.Year, newDate.Month, day);
            if (payDay < newDate)
                payDay = payDay.AddMonths(1);
            terms = new Terms(newRent, newDate, payDay, newPenalty);
        }
    }
}
