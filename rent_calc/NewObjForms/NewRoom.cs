using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rent_calc.NewObjectForms
{
    public partial class NewRoom : AbstractNew
    {
        public NewRoom() : base()
        {
            InitializeComponent();
        }
        protected override GenericNewHelper getContents()
        {
            if (addressNameTextBox.Text.Length < 3)
                return new ErrorHelper("", "Введие адрес более 3 букв");
            return new NewRoomHelper(addressNameTextBox.Text, addressDescriptionTextBox2.Text);
        }
    }
    public class NewRoomHelper : GenericNewHelper
    {
        public NewRoomHelper(string name, string description) : base(name, description)
        {
        }
    }
}
