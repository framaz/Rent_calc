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
namespace rent_calc
{

    public partial class Form1 : Form
    {
        public GenericNewHelper newSmth;
        public delegate void CreateNew();
        public CreateNew myDelegate;
        private BindingList<Address> addresses;
        public Form1()
        {
            InitializeComponent();
            myDelegate = new CreateNew(createNewItem);
            addresses = new BindingList<Address>();
            listBox1.DataSource = addresses;
            saveFileDialog1.Filter = "Save|*.dap";
            openFileDialog1.Filter = "Load|*.dap";
            button4_state = 0;
          
            
        }
        public Form1(string fileName)
        {
            InitializeComponent();
            myDelegate = new CreateNew(createNewItem);
            addresses = new BindingList<Address>();
            listBox1.DataSource = addresses;
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
                        listBox1.DataSource = addresses;
                    }
                }
            }
            button4_state = 0;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnclickAllButtons();
            Address whatAddress = (Address)listBox1.SelectedItem;
            textBox1.Text = "";
            richTextBox1.Text = "";
            if (whatAddress == null)
                return;
            listBox2.DataSource = whatAddress.people;
            if (whatAddress.people.Count > 0)
            {
                listBox2.SetSelected(0, true);
                listBox3.DataSource = whatAddress.people[0].events;
            }
            else
            {
                listBox3.DataSource = null;
                listBox3.Items.Clear();
                textBox2.Text = "";
                richTextBox2.Text = "";
            }
            textBox1.Text = whatAddress.name;
            richTextBox1.Text = whatAddress.description;

        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnclickAllButtons();
            Person who = (Person)listBox2.SelectedItem;
            if (who == null)
            {
                textBox2.Text = "";
                richTextBox2.Text = "";
                return;
            }
            who.generateWithdrawEvents(DateTime.Today);
            listBox3.DataSource = who.events;
            textBox2.Text = who.personName;
            richTextBox2.Text = who.description;
            Recount();
        }

        private void Recount()
        {
            Person who = (Person)listBox2.SelectedItem;
            textBox8.Text = "";
            textBox9.Text = "";
            if (who == null)
            {
                textBox2.Text = "";
                richTextBox2.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                return;
            }
            Report report = who.Simulate(DateTime.Now);
            textBox4.Text = report.totalDepth.ToString();
            textBox5.Text = report.totalPenalty.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewRoom test = new NewRoom();
            test.Owner = this;
            test.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                NewPerson test = new NewPerson(((Address)listBox1.SelectedItem).name);
                test.Owner = this;
                test.Show();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

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
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnclickAllButtons();
            changeSelectedEvent();
        }
        private void changeSelectedEvent()
        {
            groupBoxContract.Visible = false;
            groupBoxContract.Enabled = false;
            groupBoxLeave.Visible = false;
            groupBoxLeave.Enabled = false;
            groupBoxPayment.Visible = false;
            groupBoxPayment.Enabled = false;
            groupBoxCustomWriteOff.Visible = false;
            groupBoxCustomWriteOff.Enabled = false;
            textBox3.Text = "";
            textBox7.Text = "";
            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null || listBox3.SelectedItem == null)
                return;
            Event curEvent = (Event)listBox3.SelectedItem;
            textBox3.Text = curEvent.date.ToString().Substring(0, 11);
            if (typeof(PayEvent) == curEvent.GetType())
            {
                PayEvent trueEvent = (PayEvent)curEvent;
                groupBoxPayment.Visible = true;
                groupBoxPayment.Enabled = true;
                numericUpDown1.Value = trueEvent.moneyPaid;
            }
            if (typeof(LeaveEvent) == curEvent.GetType())
            {
                //  PayEvent trueEvent = (PayEvent)curEvent;
                //  groupBoxLeave.Visible = true;
                //  groupBoxLeave.Enabled = true;
                // numericUpDown1.Value = trueEvent.moneyPaid;
            }
            if (typeof(CustomWriteOff) == curEvent.GetType())
            {
                CustomWriteOff trueEvent = (CustomWriteOff)curEvent;
                groupBoxCustomWriteOff.Visible = true;
                groupBoxCustomWriteOff.Enabled = true;
                numericUpDown6.Value = trueEvent.moneyGot;
                richTextBox3.Text = trueEvent.commentary;
                // numericUpDown1.Value = trueEvent.moneyPaid;
            }
            if (typeof(ContractChangeEvent) == curEvent.GetType())
            {
                ContractChangeEvent trueEvent = (ContractChangeEvent)curEvent;
                groupBoxContract.Visible = true;
                groupBoxContract.Enabled = true;
                Terms terms = trueEvent.terms;
                numericUpDown4.Value = terms.payment;
                numericUpDown2.Value = terms.dayOfPayment.Day;
                numericUpDown3.Value = (decimal)terms.penalty;
                textBox6.Text = trueEvent.endDate.ToString().Substring(0, 11);
            }
            Person person = (Person)listBox2.SelectedItem;
            int pointer;
            for (pointer = person.events.Count - 1; ; pointer--)
            {
                if (person.events[pointer].GetType() == typeof(ContractChangeEvent))
                {
                    if (((ContractChangeEvent)person.events[pointer]).date <= DateTime.Now)
                        break;
                }
            }

            textBox7.Text = ((ContractChangeEvent)person.events[pointer]).endDate.ToString().Substring(0, 11);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить помещение " + ((Address)(listBox1.SelectedItem)).ToString(), "Удаление помещения", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
                return;
            addresses.Remove((Address)listBox1.SelectedItem);
            listBox1.SelectedItem = null;
            listBox2.DataSource = null;
            listBox3.DataSource = null;
            Recount();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null || listBox1.SelectedItem == null)
                return;
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить арендатора " + ((Person)(listBox2.SelectedItem)).ToString(), "Удаление арендатора", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
                return;
            ((Address)(listBox1.SelectedItem)).people.Remove((Person)listBox2.SelectedItem);
            listBox2.SelectedItem = null;
            listBox3.DataSource = null;
            Recount();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null || listBox1.SelectedItem == null || listBox3.SelectedItem == null)
                return;
            if (listBox3.SelectedIndex == 0)
            {
                MessageBox.Show("Заключение первого договора нельзя удалить");
                return;
            }
            if (listBox3.SelectedItem.GetType() == typeof(WriteOffEvent))
            {
                MessageBox.Show("Автоматически сгенерированные события удалить нельзя");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить событие: \n" + ((Event)(listBox3.SelectedItem)).ToString(), "Удаление события", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
                return;
            ((Person)(listBox2.SelectedItem)).events.Remove((Event)listBox3.SelectedItem);
            listBox3.SelectedItem = null;
            Recount();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (DateTime.Now-then>new TimeSpan(60,0,0,0,0))
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

        public void createNewItem() 
        {
            if(newSmth.GetType()==typeof(NewRoomHelper))
            {
                NewRoomHelper room = (NewRoomHelper)newSmth;
                Address newAddress = new Address(room.name, room.description);
                addresses.Add(newAddress);
                listBox1.SelectedItem = newAddress;
                listBox2.DataSource = newAddress.people;
                textBox1.Text = newAddress.name;
                richTextBox1.Text = newAddress.description;
                textBox2.Text = "";
                richTextBox2.Text = "";
                //listBox1.Items.Add(newAddress);
            }
            if(newSmth.GetType()==typeof(NewPersonHelper))
            {
                NewPersonHelper personHelper = (NewPersonHelper)newSmth;
                Person person = new Person(personHelper.name,personHelper.description,personHelper.terms.dateOfAcceptance);
                ((Address)listBox1.SelectedItem).people.Add(person);
                person.addEvent(new ContractChangeEvent(personHelper.terms.dateOfAcceptance,personHelper.endDate, personHelper.terms));
                person.generateWithdrawEvents(DateTime.Now);
                listBox2.SelectedItem = person;
                listBox3.DataSource = person.events;
                textBox2.Text = person.personName;
                richTextBox2.Text = person.description;
            }
            if(newSmth.GetType()==typeof(NewEventHelper))
            {
                Person person = (Person)listBox2.SelectedItem;
                if (person.addEvent(((NewEventHelper)newSmth).myEvent) == 1)
                    MessageBox.Show("Нельзя совершать действия до заклю" +
                        "чения первого договора");
                if(((NewEventHelper)newSmth).myEvent.GetType()==typeof(CustomWriteOff))
                {
                    int i;
                    for (i = person.events.Count - 1; i >= 0 && (person.events[i].GetType() != typeof(ContractChangeEvent) || person.events[i].date > ((NewEventHelper)newSmth).myEvent.date); i--)
                        ;
                    ((CustomWriteOff)((NewEventHelper)newSmth).myEvent).penalty = ((ContractChangeEvent)person.events[i]).terms.penalty;
                }
                listBox3.SelectedItem = ((NewEventHelper)newSmth).myEvent;
            }
            Recount();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                NewEvent test = new NewEvent(((Address)listBox1.SelectedItem).name, ((Person)listBox2.SelectedItem).ToString());
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
                    listBox1.DataSource = addresses;
                }
            }
            
        }
        int button4_state=0;
        private void UnclickAllButtons()
        {
            button4_state = 0;
            button5_state = 0;
            button4.Text = "Изменить";
            button5.Text = "Изменить";
            textBox1.Enabled = false;
            richTextBox1.Enabled = false;
            textBox2.Enabled = false;
            richTextBox2.Enabled = false;
            if (listBox1.SelectedItem != null)
            {
                Address address = (Address)listBox1.SelectedItem;
                textBox1.Text = address.name ;
                richTextBox1.Text = address.description;
            }
            if (listBox2.SelectedItem != null)
            {
                Person person = (Person)listBox2.SelectedItem;
                textBox2.Text = person.personName;
                richTextBox2.Text = person.description;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(button4_state == 0)
            {
                UnclickAllButtons();
                if (listBox1.SelectedItem == null)
                    return;
                button4.Text = "Готово";
                textBox1.Enabled = true;
                richTextBox1.Enabled = true;
                button4_state = 1;
            }
            else
            {
                if (listBox1.SelectedItem == null)
                    return;
                Address address = (Address)listBox1.SelectedItem;
                button4.Text = "Изменить";
                address.name = textBox1.Text;
                address.description =richTextBox1.Text;
                button4_state = 0;
                textBox1.Enabled = false;
                richTextBox1.Enabled = false;
                listBox1.DataSource = null;
                listBox1.DataSource = addresses;
                listBox1.SelectedItem = address;
            }
        }
        int button5_state = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            if (button5_state == 0)
            {
                UnclickAllButtons();
                if (listBox2.SelectedItem == null)
                    return;
                button5.Text = "Готово";
                textBox2.Enabled = true;
                richTextBox2.Enabled = true;
                button5_state = 1;
            }
            else
            {
                if (listBox2.SelectedItem == null)
                    return;
                Person person = (Person)listBox2.SelectedItem;
                button5.Text = "Изменить";
                person.personName = textBox2.Text;
                person.description = richTextBox2.Text;
                button5_state = 0;
                textBox2.Enabled = false;
                richTextBox2.Enabled = false;
                listBox2.DataSource = null;
                listBox2.DataSource = ((Address)listBox1.SelectedItem).people;
                listBox2.SelectedItem = person;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem == null)
                return;
            Person person = (Person)listBox2.SelectedItem;
            Report report = person.Simulate(monthCalendar1.SelectionStart);
            textBox9.Text = report.totalDepth.ToString();
            textBox8.Text = report.totalPenalty.ToString();
            person.generateWithdrawEvents(DateTime.Now);
        }

        private void button10_Click(object sender, EventArgs e)
        {

            if (listBox2.SelectedItem == null)
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
                foreach(Event curEvent in ((Person)listBox2.SelectedItem).events.Where(x=> { return x.date <= monthCalendar1.SelectionStart; }))
                {
                    if(curEvent.ToRow(row))
                    {
                        curRow++;
                        row = sheet.CreateRow(curRow); 
                    }
                }
                Person person = (Person)listBox2.SelectedItem;
                Report report = person.Simulate(monthCalendar1.SelectionStart);
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                row.CreateCell(0).SetCellValue("Итого на " + monthCalendar1.SelectionStart.ToString().Substring(0, 11));
                row.CreateCell(1).SetCellValue("Долг:");
                row.CreateCell(2).SetCellValue(report.totalDepth);
                row.CreateCell(3).SetCellValue("Пеня:");
                row.CreateCell(4).SetCellValue(report.totalPenalty);
                row.CreateCell(5).SetCellValue("Долг+пеня:");
                row.CreateCell(6).SetCellValue(report.totalDepth+report.totalPenalty);
                for(int i=0;i<7;i++)
                    sheet.AutoSizeColumn(i);
                FileStream sw = File.Create(saveFileDialog2.FileName);
                workbook.Write(sw);
                sw.Close();

            }

        }

        private void saveFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            
        }
    }

    [Serializable]
    public class MonthBill : Object
    {
        public DateTime date;
        public double debt;
        public double penalty;
        public double penaltyPercentage;
        public MonthBill(DateTime newDate,double newDebt,double newPenaltyPercentage)
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
        public string personName,description;
        public Terms terms;
        public List<MonthBill> months;
        public BindingList<Event> events;
        public double penalty;
        public int addEvent(Event newEvent)
        {
            //TODO переделать
            int pointer=0;
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
            int pointer=0;
            for(int i=0;i<events.Count;i++)
            {
                Event curEvent = events[i];
                if (curEvent.GetType() == typeof(WriteOffEvent))
                {
                    events.Remove(curEvent);
                    i--;
                }
            }
            while(nextPaymentDate<=target)
            {
                bool isPayed = false;
                for(; pointer < events.Count&&events[pointer].date<=nextPaymentDate;pointer++)
                {
                    if(events[pointer].getType() == "leaveEvent")
                    {
                        events[pointer].Apply(this);
                    }
                    if (events[pointer].getType() == "paymentChangeEvent")
                        events[pointer].Apply(this);
                    if (events[pointer].getType() == "contractChangeEvent")
                    {
                        events[pointer].Apply(this);
                        nextPaymentDate = new DateTime(terms.dateOfAcceptance.Year, terms.dateOfAcceptance.Month, terms.dayOfPayment.Day);
                        if (nextPaymentDate < terms.dateOfAcceptance)
                            nextPaymentDate=nextPaymentDate.AddMonths(1);
                    }
                    
                }
                if (!isPayed&&status==1)
                {
                    events.Insert(pointer, new WriteOffEvent(nextPaymentDate, terms.payment,terms.penalty));
                    pointer++;
                }
                nextPaymentDate=nextPaymentDate.AddMonths(1);
            }
        }
        private DateTime start, finish;
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
            if(currentDate>target)
            {
                report.stringReport = "Неверная дата";
                return report;
            }
            List<Event> eventHistory = new List<Event>();
            months.Clear();
            int pointer=0;
            
            for(;currentDate.Date<=target; currentDate=currentDate.AddDays(1))
            {
                PenaltyEvent penaltyEvent = new PenaltyEvent(currentDate);
                penaltyEvent.Apply(this);
                eventHistory.Add(penaltyEvent);
                for(;pointer<events.Count&&events[pointer].date<=currentDate;pointer++)
                {
                    eventHistory.Add(events[pointer]);
                    events[pointer].Apply(this);
                }
            }
            foreach(MonthBill bill in months)
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
    public abstract class Event : Object
    {
        protected string type;
        public DateTime date;
        public DateTime getDate()
        {
            return date;
        }
        public string getType()
        {
            return type;
        }
        public Event(string newType, DateTime newDate)
        {
            type = newType;
            date = newDate;
        }
        abstract public void Apply(Person person);
        public virtual bool ToRow(IRow row)
        {
            row.CreateCell(0).SetCellValue(date.ToString().Substring(0,11));
            return false;
        }
        public override string ToString()
        {
            return date.ToString().Substring(0, 10) + ": ";
        }
    }
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
        public PayEvent(DateTime newDate,int money) : base("payEvent", newDate)
        {
            moneyPaid = money;
        }
        public override string ToString()
        {
            return base.ToString()+"Оплачено " + moneyPaid+"р."; 
        }
        public override void Apply(Person person)
        {
            int curMoney = moneyPaid+person.overpayment;
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
        public WriteOffEvent(DateTime newDate, int money,double penaltyPercent) : base("writeOffEvent", newDate)
        {
            moneyGot = money;
            penalty = penaltyPercent;
        }
        public override string ToString()
        {
            return base.ToString()+"Снятие средств с арендатора "+moneyGot+"р.";
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
            person.months.Add(new MonthBill(date, realMoney,penalty));
        }
    }
    [Serializable]
    public class CustomWriteOff : WriteOffEvent
    {
        public string commentary;
        public CustomWriteOff(DateTime newDate, int money, double penaltyPercent,string newCommentary) : base(newDate,money,penaltyPercent)
        {
            commentary = newCommentary;
        }
        public override string ToString()
        {
            return date.ToString().Substring(0,11)+":"+ "Переменная часть: " + moneyGot+"р.";
        }
    }
    [Serializable]
    public class PenaltyEvent : Event
    {
        public PenaltyEvent(DateTime newDate) : base("penaltyEvent", newDate)
        {
        }
        public override void Apply(Person person)
        {
            foreach(MonthBill month in person.months)
            {
                month.penalty +=month.debt * month.penaltyPercentage;
            }
        }
    }
    [Serializable]
    public class LeaveEvent : Event
    {
        public LeaveEvent(DateTime newDate) : base("leaveEvent", newDate)
        {

        }
        public override string ToString()
        {
            return base.ToString()+"Арендатор покинул помещение";
        }
        public override void Apply(Person person)
        {
            person.status = 0;
        }
    }
    [Serializable]
    public class PaymentChangeEvent : Event
    {
        public int payment;
        public PaymentChangeEvent(DateTime newDate,int newPayment) : base("paymentChangeEvent", newDate)
        {
            payment = newPayment;
        }
        public override string ToString()
        {
            return base.ToString() + "Изменение оплаты, теперь "+payment+"р.";
        }
        public override void Apply(Person person)
        {
            person.terms.payment = payment;
        }
    }
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
            person.terms = new Terms(terms.payment,terms.dateOfAcceptance,terms.dayOfPayment,terms.penalty);
            person.status = 1;
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
        public Terms(int newPayment,DateTime newDateOfAcceptance,DateTime newDayOfPayment,double newPenalty)
        {
            payment = newPayment;
            dateOfAcceptance = newDateOfAcceptance;
            dayOfPayment = newDayOfPayment;
            penalty = newPenalty;
        }
        public Terms(int newPayment, DateTime newDateOfAcceptance, DateTime newDayOfPayment, double newPenalty,DateTime newDateOfEnd)
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
        public Address(string newName,string newDescription)
        {
            name = newName;
            description = newDescription;
            people = new BindingList<Person>();
        }
    }

}
