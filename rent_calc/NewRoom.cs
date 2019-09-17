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
    public partial class NewRoom : AbstractNew
    {
        public NewRoom():base()
        {
            InitializeComponent();
        }
        protected override GenericNewHelper getContents()
        {
            if (textBox1.Text.Length < 3)
                return new ErrorHelper("", "Введие адрес более 3 букв");
            return new NewRoomHelper(textBox1.Text,textBox2.Text);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    public class NewRoomHelper : GenericNewHelper
    {
        public NewRoomHelper(string name,string description):base(name,description)
        {
        }
    }
}
