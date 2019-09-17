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
    public partial class NewEvent : AbstractNew
    {
        GroupBox[] groupBoxes;
        public NewEvent(string address, string person):base()
        {
            InitializeComponent();
            label1.Text = address;
            label2.Text = person;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            groupBoxes = new GroupBox[] { groupBox1, groupBox2, groupBox3, groupBox4, groupBox5 };
            comboBox1.SelectedIndex = 0;
            monthCalendar2.MinDate = monthCalendar1.SelectionStart;
        }
        protected override GenericNewHelper getContents()
        {
            //     if (textBox1.Text.Length < 3)
            //       return new ErrorHelper("", "Введие адрес более 3 букв");
            //    return new NewRoomHelper(textBox1.Text, textBox2.Text);
            Event myEvent;
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    myEvent = new PayEvent(monthCalendar1.SelectionStart, (int)numericUpDown1.Value);
                    break;
                case 1:
                    DateTime newDate = monthCalendar1.SelectionStart;
                    DateTime endDate = monthCalendar2.SelectionStart;
                    DateTime payDay = new DateTime(newDate.Year, newDate.Month, (int)numericUpDown2.Value);
                    if (payDay < newDate)
                        payDay = payDay.AddMonths(1);
                    myEvent = new ContractChangeEvent(newDate, endDate, new Terms((int)numericUpDown4.Value, newDate, payDay, (double)numericUpDown3.Value));
                    break;
                case 2:
                    myEvent = new LeaveEvent(monthCalendar1.SelectionStart);
                    break;
                case 3:
                    myEvent = new PaymentChangeEvent(monthCalendar1.SelectionStart, (int)numericUpDown5.Value);
                    break;
                case 4:
                    myEvent = new CustomWriteOff(monthCalendar1.SelectionStart, (int)numericUpDown6.Value,0,richTextBox1.Text);
                    break;
                default:
                    myEvent = new LeaveEvent(monthCalendar1.SelectionStart);
                    break;
            }
            return new NewEventHelper(myEvent);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(GroupBox i in groupBoxes)
            {
                i.Visible = false;
                i.Enabled = false;
            }
            groupBoxes[comboBox1.SelectedIndex].Visible = true;
            groupBoxes[comboBox1.SelectedIndex].Enabled = true;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            monthCalendar2.MinDate = monthCalendar1.SelectionStart;
        }
    }
    public class NewEventHelper : GenericNewHelper
    {
        public Event myEvent;
        public NewEventHelper(Event newEvent):base("","")
        {
            myEvent = newEvent;
        }
    }
}
