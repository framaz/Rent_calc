using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rent_calc
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
            monthCalendar2.MinDate = monthCalendar1.SelectionStart;
        }
        protected override GenericNewHelper getContents()
        {
            if (textBox1.Text.Length < 3)
                return new ErrorHelper("", "Введие ФИО более 3 букв");
            if (monthCalendar2.SelectionStart<= monthCalendar1.SelectionStart)
                return new ErrorHelper("", "Договор оканчивается раньше, чем начинается");
            return new NewPersonHelper(textBox1.Text, textBox2.Text,monthCalendar1.SelectionStart, monthCalendar2.SelectionStart, (int)numericUpDown1.Value,(int)numericUpDown2.Value,(double)numericUpDown3.Value);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            //    DateTime date = monthCalendar1.SelectionStart;
            //     label4.Text = date.ToString().Substring(0,11);
            monthCalendar2.MinDate = monthCalendar1.SelectionStart;
        }

        private void NewPerson_Load(object sender, EventArgs e)
        {

        }
    }
    public class NewPersonHelper : GenericNewHelper
    {
        public Terms terms;
        public DateTime endDate;
        public NewPersonHelper(string name, string description,DateTime newDate, DateTime newEndDate, int newRent,int day,double newPenalty) : base(name, description)
        {
            endDate = newEndDate;
            DateTime payDay = new DateTime(newDate.Year, newDate.Month, day);
            if (payDay < newDate)
                payDay = payDay.AddMonths(1);
            terms = new Terms(newRent, newDate, payDay, newPenalty);
        }
    }
}
