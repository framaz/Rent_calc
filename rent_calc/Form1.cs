using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

using rent_calc.Events;
namespace rent_calc
{
    public partial class Form1 : Form
    {
        public delegate void CreateNew(GenericNewHelper helper);
        public CreateNew myDelegate;
        private BindingList<Address> addresses;
        int changeCurrentAddressInfoButton_state = 0;
        public Form1()
        {
            InitializeComponent();
            myDelegate = new CreateNew(createNewItem);
            addresses = new BindingList<Address>();
            addressListBox.DataSource = addresses;
            saveFileDialog1.Filter = "Save|*.dap";
            openFileDialog1.Filter = "Load|*.dap";
            changeCurrentAddressInfoButton_state = 0;
        }
        public Form1(string fileName)
        {
            InitializeComponent();
            myDelegate = new CreateNew(createNewItem);
            addresses = new BindingList<Address>();
            addressListBox.DataSource = addresses;
            saveFileDialog1.Filter = "Save|*.dap";
            openFileDialog1.Filter = "Load|*.dap";

            if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                openFileDialog1.FileName = fileName;
                if (openFileDialog1.FileName != "")
                {
                    using (FileStream fs = (FileStream)openFileDialog1.OpenFile())
                    {
                        BindingList<Address> newAddresses = (BindingList<Address>)formatter.Deserialize(fs);
                        addresses = newAddresses;
                        addressListBox.DataSource = addresses;
                    }
                }
            }
            changeCurrentAddressInfoButton_state = 0;
        }
        private void addressListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnclickAllButtons();
            Address whatAddress = (Address)addressListBox.SelectedItem;
            currentAddressNameTextBox.Text = "";
            currentAddressDescriptionTextBox.Text = "";
            if (whatAddress == null)
                return;

            personListBox.DataSource = whatAddress.people;
            if (whatAddress.people.Count > 0)
            {
                personListBox.SetSelected(0, true);
                eventListBox.DataSource = whatAddress.people[0].events;
            }
            else
            {
                eventListBox.DataSource = null;
                eventListBox.Items.Clear();
                currentPersonNameTextBox.Text = "";
                currentPersonDescriptionTextBox.Text = "";
            }
            currentAddressNameTextBox.Text = whatAddress.name;
            currentAddressDescriptionTextBox.Text = whatAddress.description;

        }
        private void personListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnclickAllButtons();
            Person who = (Person)personListBox.SelectedItem;
            if (who == null)
            {
                currentPersonNameTextBox.Text = "";
                currentPersonDescriptionTextBox.Text = "";
                return;
            }

            who.generateWithdrawEvents(DateTime.Today);
            eventListBox.DataSource = who.events;
            currentPersonNameTextBox.Text = who.personName;
            currentPersonDescriptionTextBox.Text = who.description;
            Recount();
        }
        private void Recount()
        {
            Person who = (Person)personListBox.SelectedItem;
            currentPenaltyTextBox.Text = "";
            currentDebtTextBox.Text = "";
            if (who == null)
            {
                currentPersonNameTextBox.Text = "";
                currentPersonDescriptionTextBox.Text = "";
                todayDebtTextBox.Text = "";
                todayPenaltyTextBox.Text = "";
                return;
            }

            Report report = who.Simulate(DateTime.Now);
            todayDebtTextBox.Text = report.totalDepth.ToString();
            todayPenaltyTextBox.Text = report.totalPenalty.ToString();
        }
        private void newAddressButton_Click(object sender, EventArgs e)
        {
            NewRoom test = new NewRoom();
            test.Owner = this;
            test.Show();
        }
        private void newPersonButton_Click(object sender, EventArgs e)
        {
            if (addressListBox.SelectedItem != null)
            {
                NewPerson test = new NewPerson(((Address)addressListBox.SelectedItem).name);
                test.Owner = this;
                test.Show();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Сохранить данные перед выходом?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    if (!Save())
                        e.Cancel = true;
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }
        }
        private void EventListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnclickAllButtons();
            SelectEvent();
        }
        private void SelectEvent()
        {
            groupBoxContract.Visible = false;
            groupBoxContract.Enabled = false;
            groupBoxLeave.Visible = false;
            groupBoxLeave.Enabled = false;
            groupBoxPayment.Visible = false;
            groupBoxPayment.Enabled = false;
            groupBoxCustomWriteOff.Visible = false;
            groupBoxCustomWriteOff.Enabled = false;
            currentEventDateTextBox.Text = "";
            lastTermEndDateTextBox.Text = "";

            if (addressListBox.SelectedItem == null || personListBox.SelectedItem == null || eventListBox.SelectedItem == null)
                return;

            Event curEvent = (Event)eventListBox.SelectedItem;
            currentEventDateTextBox.Text = curEvent.date.ToString().Substring(0, 11);
            switch (curEvent)
            {
                case PayEvent payEvent:
                    groupBoxPayment.Visible = true;
                    groupBoxPayment.Enabled = true;
                    numericUpDown1.Value = payEvent.moneyPaid;
                    break;
                case CustomWriteOff customWriteOff:
                    groupBoxCustomWriteOff.Visible = true;
                    groupBoxCustomWriteOff.Enabled = true;
                    numericUpDown6.Value = customWriteOff.moneyGot;
                    richTextBox3.Text = customWriteOff.commentary;
                    break;
                case ContractChangeEvent contractChangeEvent:
                    groupBoxContract.Visible = true;
                    groupBoxContract.Enabled = true;
                    Terms terms = contractChangeEvent.terms;
                    currentEventTermPaymentUpDown.Value = terms.payment;
                    currentEventTermPaymentDateUpDown.Value = terms.dayOfPayment.Day;
                    currentEventTermPenaltyUpDown.Value = (decimal)terms.penalty;
                    currentEventTermEndDateUpDown.Text = contractChangeEvent.endDate.ToString().Substring(0, 11);
                    break;
            }
            Person person = (Person)personListBox.SelectedItem;

            int pointer;
            for (pointer = person.events.Count - 1; ; pointer--)
            {
                if (person.events[pointer].GetType() == typeof(ContractChangeEvent))
                {
                    if (((ContractChangeEvent)person.events[pointer]).date <= DateTime.Now)
                        break;
                }
            }
            lastTermEndDateTextBox.Text = ((ContractChangeEvent)person.events[pointer]).endDate.ToString().Substring(0, 11);
        }
        private void deleteAddressButton_Click(object sender, EventArgs e)
        {
            if (addressListBox.SelectedItem == null)
                return;
            DialogResult dialogResult = MessageBox.Show(
                "Вы действительно хотите удалить помещение " + ((Address)(addressListBox.SelectedItem)).ToString(),
                "Удаление помещения", 
                MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
                return;

            addresses.Remove((Address)addressListBox.SelectedItem);
            addressListBox.SelectedItem = null;
            personListBox.DataSource = null;
            eventListBox.DataSource = null;
            Recount();
        }
        private void deletePersonButton_Click(object sender, EventArgs e)
        {
            if (personListBox.SelectedItem == null || addressListBox.SelectedItem == null)
                return;
            DialogResult dialogResult = MessageBox.Show(
                "Вы действительно хотите удалить арендатора " + ((Person)(personListBox.SelectedItem)).ToString(),
                "Удаление арендатора",
                MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
                return;

            ((Address)(addressListBox.SelectedItem)).people.Remove((Person)personListBox.SelectedItem);
            personListBox.SelectedItem = null;
            eventListBox.DataSource = null;
            Recount();
        }
        private void deleteEventButton_Click(object sender, EventArgs e)
        {
            if (personListBox.SelectedItem == null || addressListBox.SelectedItem == null || eventListBox.SelectedItem == null)
                return;
            if (eventListBox.SelectedIndex == 0)
            {
                MessageBox.Show("Заключение первого договора нельзя удалить");
                return;
            }
            if (eventListBox.SelectedItem.GetType() == typeof(WriteOffEvent))
            {
                MessageBox.Show("Автоматически сгенерированные события удалить нельзя");
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Вы действительно хотите удалить событие: \n" + ((Event)(eventListBox.SelectedItem)).ToString(),
                "Удаление события", 
                MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
                return;

            ((Person)(personListBox.SelectedItem)).events.Remove((Event)eventListBox.SelectedItem);
            eventListBox.SelectedItem = null;
            Recount();
        }
        private void СохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        private bool Save()
        {
            bool status = false;
            BinaryFormatter formatter = new BinaryFormatter();
            saveFileDialog1.ShowDialog();
            string saveFilePath = saveFileDialog1.FileName;
            if (saveFileDialog1.FileName != "")
            {
                using (FileStream fs = (FileStream)saveFileDialog1.OpenFile())
                {
                    formatter.Serialize(fs, addresses);
                    status = true;
                }
            }

            string md = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rent Calculator";
            if (Directory.Exists(md) == false)
            {
                Directory.CreateDirectory(md);
            }
            if (Directory.Exists(md + "\\All backups") == false)
            {
                Directory.CreateDirectory(md + "\\All backups");
            }
            saveFileDialog1.FileName = md + "\\All backups\\" + DateTime.Now.ToString("dd.MM.yyyy HH-mm") + ".dap";
            using (FileStream fs = (FileStream)saveFileDialog1.OpenFile())
            {
                formatter.Serialize(fs, addresses);
            }
            saveFileDialog1.FileName = md + "\\" + DateTime.Now.ToString("dd.MM.yyyy HH-mm") + ".dap";
            using (FileStream fs = (FileStream)saveFileDialog1.OpenFile())
            {
                formatter.Serialize(fs, addresses);
            }

            foreach (string str in Directory.GetFiles(md, "*.dap"))
            {
                try
                {
                    string fileName = str.Substring(1 + str.LastIndexOf("\\"));
                    DateTime then = Convert.ToDateTime(fileName.Substring(0, 11));
                    if (DateTime.Now - then > new TimeSpan(60, 0, 0, 0, 0))
                    {

                        File.Delete(str);
                    }
                }
                catch (Exception e)
                { }
            }
            saveFileDialog1.FileName = saveFilePath;
            return status;
        }
        public void createNewItem(GenericNewHelper newSmth)
        {
            switch (newSmth)
            {
                case NewRoomHelper room:
                    Address newAddress = new Address(room.name, room.description);
                    addresses.Add(newAddress);
                    addressListBox.SelectedItem = newAddress;
                    personListBox.DataSource = newAddress.people;
                    currentAddressNameTextBox.Text = newAddress.name;
                    currentAddressDescriptionTextBox.Text = newAddress.description;
                    currentPersonNameTextBox.Text = "";
                    currentPersonDescriptionTextBox.Text = "";
                    //addressListBox.Items.Add(newAddress);
                    break;
                case NewPersonHelper personHelper:
                    Person person = new Person(personHelper.name, personHelper.description, personHelper.terms.dateOfAcceptance);
                    ((Address)addressListBox.SelectedItem).people.Add(person);
                    person.addEvent(new ContractChangeEvent(personHelper.terms.dateOfAcceptance, personHelper.endDate, personHelper.terms));
                    person.generateWithdrawEvents(DateTime.Now);
                    personListBox.SelectedItem = person;
                    eventListBox.DataSource = person.events;
                    currentPersonNameTextBox.Text = person.personName;
                    currentPersonDescriptionTextBox.Text = person.description;
                    break;
                case NewEventHelper new_event:
                    person = (Person)personListBox.SelectedItem;
                    if (person.addEvent(((NewEventHelper)newSmth).myEvent) == 1)
                        MessageBox.Show("Нельзя совершать действия до заклю" +
                            "чения первого договора");
                    if (((NewEventHelper)newSmth).myEvent.GetType() == typeof(CustomWriteOff))
                    {
                        int i;
                        for (i = person.events.Count - 1;
                            i >= 0 &&
                            (person.events[i].GetType() != typeof(ContractChangeEvent) ||
                            person.events[i].date > ((NewEventHelper)newSmth).myEvent.date);
                            i--)
                            ;
                        ((CustomWriteOff)((NewEventHelper)newSmth).myEvent).penalty = ((ContractChangeEvent)person.events[i]).terms.penalty;
                    }
                    eventListBox.SelectedItem = ((NewEventHelper)newSmth).myEvent;
                    break;
            }
            Recount();
        }
        private void newEventButton_Click(object sender, EventArgs e)
        {
            if (personListBox.SelectedItem != null)
            {
                NewEvent test = new NewEvent(((Address)addressListBox.SelectedItem).name, ((Person)personListBox.SelectedItem).ToString());
                test.Owner = this;
                test.Show();
            }
        }
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                using (FileStream fs = (FileStream)openFileDialog1.OpenFile())
                {
                    BindingList<Address> newAddresses = (BindingList<Address>)formatter.Deserialize(fs);
                    addresses = newAddresses;
                    addressListBox.DataSource = addresses;
                }
            }

        }
        private void UnclickAllButtons()
        {
            changeCurrentAddressInfoButton_state = 0;
            changeCurrentPersonInfoButton_state = 0;
            changeCurrentAddressInfoButton.Text = "Изменить";
            changeCurrentPersonInfoButton.Text = "Изменить";
            currentAddressNameTextBox.Enabled = false;
            currentAddressDescriptionTextBox.Enabled = false;
            currentPersonNameTextBox.Enabled = false;
            currentPersonDescriptionTextBox.Enabled = false;

            if (addressListBox.SelectedItem != null)
            {
                Address address = (Address)addressListBox.SelectedItem;
                currentAddressNameTextBox.Text = address.name;
                currentAddressDescriptionTextBox.Text = address.description;
            }
            if (personListBox.SelectedItem != null)
            {
                Person person = (Person)personListBox.SelectedItem;
                currentPersonNameTextBox.Text = person.personName;
                currentPersonDescriptionTextBox.Text = person.description;
            }
        }
        private void changeCurrentAddressInfoButton_Click(object sender, EventArgs e)
        {
            if (changeCurrentAddressInfoButton_state == 0)
            {
                UnclickAllButtons();
                if (addressListBox.SelectedItem == null)
                    return;
                changeCurrentAddressInfoButton.Text = "Готово";
                currentAddressNameTextBox.Enabled = true;
                currentAddressDescriptionTextBox.Enabled = true;
                changeCurrentAddressInfoButton_state = 1;
            }
            else
            {
                if (addressListBox.SelectedItem == null)
                    return;
                Address address = (Address)addressListBox.SelectedItem;
                changeCurrentAddressInfoButton.Text = "Изменить";
                address.name = currentAddressNameTextBox.Text;
                address.description = currentAddressDescriptionTextBox.Text;
                changeCurrentAddressInfoButton_state = 0;
                currentAddressNameTextBox.Enabled = false;
                currentAddressDescriptionTextBox.Enabled = false;
                addressListBox.DataSource = null;
                addressListBox.DataSource = addresses;
                addressListBox.SelectedItem = address;
            }
        }
        int changeCurrentPersonInfoButton_state = 0;
        private void changeCurrentPersonInfoButton_Click(object sender, EventArgs e)
        {
            if (changeCurrentPersonInfoButton_state == 0)
            {
                UnclickAllButtons();
                if (personListBox.SelectedItem == null)
                    return;
                changeCurrentPersonInfoButton.Text = "Готово";
                currentPersonNameTextBox.Enabled = true;
                currentPersonDescriptionTextBox.Enabled = true;
                changeCurrentPersonInfoButton_state = 1;
            }
            else
            {
                if (personListBox.SelectedItem == null)
                    return;
                Person person = (Person)personListBox.SelectedItem;
                changeCurrentPersonInfoButton.Text = "Изменить";
                person.personName = currentPersonNameTextBox.Text;
                person.description = currentPersonDescriptionTextBox.Text;
                changeCurrentPersonInfoButton_state = 0;
                currentPersonNameTextBox.Enabled = false;
                currentPersonDescriptionTextBox.Enabled = false;
                personListBox.DataSource = null;
                personListBox.DataSource = ((Address)addressListBox.SelectedItem).people;
                personListBox.SelectedItem = person;
            }
        }
        private void countCurrentButton_Click(object sender, EventArgs e)
        {
            if (eventListBox.SelectedItem == null)
                return;
            Person person = (Person)personListBox.SelectedItem;
            Report report = person.Simulate(curDateCalendar.SelectionStart);
            currentDebtTextBox.Text = report.totalDepth.ToString();
            currentPenaltyTextBox.Text = report.totalPenalty.ToString();
            person.generateWithdrawEvents(DateTime.Now);
        }
        private void exportXLSButton_Click(object sender, EventArgs e)
        {

            if (personListBox.SelectedItem == null)
                return;
            saveFileDialog2.ShowDialog();
            if (saveFileDialog2.FileName != "")
            {
                IWorkbook workbook = new XSSFWorkbook();
                workbook.CreateSheet("Sheet A1");
                XSSFSheet sheet = workbook.GetSheetAt(0) as XSSFSheet;
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Дата");
                row.CreateCell(1).SetCellValue("Начислено");
                row.CreateCell(2).SetCellValue("Оплачено");
                int curRow = 1;
                row = sheet.CreateRow(curRow);
                foreach (Event curEvent in ((Person)personListBox.SelectedItem).events.Where(
                    x => { return x.date <= curDateCalendar.SelectionStart; }))
                {
                    if (curEvent.ToRow(row))
                    {
                        curRow++;
                        row = sheet.CreateRow(curRow);
                    }
                }
                Person person = (Person)personListBox.SelectedItem;
                Report report = person.Simulate(curDateCalendar.SelectionStart);
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                row.CreateCell(0).SetCellValue("Итого на " + curDateCalendar.SelectionStart.ToString().Substring(0, 11));
                row.CreateCell(1).SetCellValue("Долг:");
                row.CreateCell(2).SetCellValue(report.totalDepth);
                row.CreateCell(3).SetCellValue("Пеня:");
                row.CreateCell(4).SetCellValue(report.totalPenalty);
                row.CreateCell(5).SetCellValue("Долг+пеня:");
                row.CreateCell(6).SetCellValue(report.totalDepth + report.totalPenalty);
                for (int i = 0; i < 7; i++)
                    sheet.AutoSizeColumn(i);
                FileStream sw = File.Create(saveFileDialog2.FileName);
                workbook.Write(sw);
                sw.Close();

            }

        }
    }

    [Serializable]
    public class MonthBill : Object
    {
        public DateTime date;
        public double debt;
        public double penalty;
        public double penaltyPercentage;
        public MonthBill(DateTime newDate, double newDebt, double newPenaltyPercentage)
        {
            debt = newDebt;
            date = newDate;
            penaltyPercentage = newPenaltyPercentage;
        }
    }
    public struct Report
    {
        public double totalDepth;
        public double totalPenalty;
        public string stringReport;
    }
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

    [Serializable]
    public class Terms : Object
    {
        public int payment;
        public DateTime dateOfAcceptance;
        public DateTime dayOfPayment;
        public DateTime dateOfEnd;
        public double penalty;
        public Terms(int newPayment, DateTime newDateOfAcceptance, DateTime newDayOfPayment, double newPenalty)
        {
            payment = newPayment;
            dateOfAcceptance = newDateOfAcceptance;
            dayOfPayment = newDayOfPayment;
            penalty = newPenalty;
        }
        public Terms(int newPayment,
            DateTime newDateOfAcceptance,
            DateTime newDayOfPayment,
            double newPenalty,
            DateTime newDateOfEnd)
        {
            payment = newPayment;
            dateOfAcceptance = newDateOfAcceptance;
            dayOfPayment = newDayOfPayment;
            penalty = newPenalty;
            dateOfEnd = newDateOfEnd;
        }
    }
    [Serializable]
    public class Address : Object
    {
        public string name;
        public string description;
        public BindingList<Person> people;
        public string GetName()
        {
            return name;
        }
        public override string ToString()
        {
            return GetName();
        }
        public Address(string newName, string newDescription)
        {
            name = newName;
            description = newDescription;
            people = new BindingList<Person>();
        }
    }

}
