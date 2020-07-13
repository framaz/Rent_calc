using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using rent_calc.Events;

namespace rent_calc.NewObjectForms
{
    public partial class NewEvent : AbstractNew
    {
        GroupBox[] groupBoxes;
        public NewEvent(string address, string person) : base()
        {
            InitializeComponent();
            addressLabel.Text = address;
            personLabel.Text = person;
            eventTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            groupBoxes = new GroupBox[] {
                paymentGroupBox,
                newTermsGroupBox,
                leaveGroupBox,
                newPaymentSetGroupBox,
                customWriteOffGroupBox };
            eventTypeComboBox.SelectedIndex = 0;
            termsEndMonthCalendar.MinDate = eventDateMonthCalendar.SelectionStart;
        }
        protected override GenericNewHelper getContents()
        {
            //     if (textBox1.Text.Length < 3)
            //       return new ErrorHelper("", "Введие адрес более 3 букв");
            //    return new NewRoomHelper(textBox1.Text, addressDescriptionTextBox2.Text);
            Event myEvent;
            switch (eventTypeComboBox.SelectedIndex)
            {
                case 0:
                    myEvent = new PayEvent(eventDateMonthCalendar.SelectionStart, (int)paymentSumUpDown.Value);
                    break;
                case 1:
                    DateTime newDate = eventDateMonthCalendar.SelectionStart;
                    DateTime endDate = termsEndMonthCalendar.SelectionStart;
                    DateTime payDay = new DateTime(newDate.Year, newDate.Month, (int)payDateUpDown.Value);
                    if (payDay < newDate)
                        payDay = payDay.AddMonths(1);
                    myEvent = new ContractChangeEvent(
                        newDate, 
                        endDate, 
                        new Terms((int)newTermsPaymentUpDown.Value, 
                            newDate,
                            payDay, 
                            (double)newTermsPenaltyUpDown.Value));
                    break;
                case 2:
                    myEvent = new LeaveEvent(eventDateMonthCalendar.SelectionStart);
                    break;
                case 3:
                    myEvent = new PaymentChangeEvent(eventDateMonthCalendar.SelectionStart, (int)newPaymentUpDown.Value);
                    break;
                case 4:
                    myEvent = new CustomWriteOff(
                        eventDateMonthCalendar.SelectionStart, 
                        (int)customWriteOffSumUpDown.Value,
                        0,
                        customWriteOffCommentTextBox.Text);
                    break;
                default:
                    myEvent = new LeaveEvent(eventDateMonthCalendar.SelectionStart);
                    break;
            }
            return new NewEventHelper(myEvent);
        }

        private void eventTypeComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GroupBox i in groupBoxes)
            {
                i.Visible = false;
                i.Enabled = false;
            }
            groupBoxes[eventTypeComboBox.SelectedIndex].Visible = true;
            groupBoxes[eventTypeComboBox.SelectedIndex].Enabled = true;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            termsEndMonthCalendar.MinDate = eventDateMonthCalendar.SelectionStart;
        }
    }
    public class NewEventHelper : GenericNewHelper
    {
        public Event myEvent;
        public NewEventHelper(Event newEvent) : base("", "")
        {
            myEvent = newEvent;
        }
    }
}
