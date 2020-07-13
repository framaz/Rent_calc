namespace rent_calc.NewObjectForms
{
    partial class NewPerson
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startTermMonthCalendar = new System.Windows.Forms.MonthCalendar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rentSumUpDown = new System.Windows.Forms.NumericUpDown();
            this.paymentDayUpDown = new System.Windows.Forms.NumericUpDown();
            this.penaltyUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.endTermMonthCalendar = new System.Windows.Forms.MonthCalendar();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rentSumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentDayUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penaltyUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // addressDescriptionTextBox2
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(7, 84);
            this.descriptionTextBox.Name = "addressDescriptionTextBox2";
            this.descriptionTextBox.Size = new System.Drawing.Size(402, 20);
            this.descriptionTextBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Доп. Информация";
            // 
            // textBox1
            // 
            this.nameTextBox.Location = new System.Drawing.Point(7, 46);
            this.nameTextBox.Name = "textBox1";
            this.nameTextBox.Size = new System.Drawing.Size(402, 20);
            this.nameTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ФИО";
            // 
            // eventDateMonthCalendar
            // 
            this.startTermMonthCalendar.Location = new System.Drawing.Point(7, 174);
            this.startTermMonthCalendar.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.startTermMonthCalendar.MaxSelectionCount = 1;
            this.startTermMonthCalendar.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.startTermMonthCalendar.Name = "eventDateMonthCalendar";
            this.startTermMonthCalendar.ShowToday = false;
            this.startTermMonthCalendar.ShowTodayCircle = false;
            this.startTermMonthCalendar.TabIndex = 9;
            this.startTermMonthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.endTermMonthCalendar_DateChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Дата заключения";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(184, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Сумма оплаты";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(184, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Дата оплаты (номер дня в месяце)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Пеня (В десятичных долях)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(114, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Информация по договору";
            // 
            // paymentSumUpDown
            // 
            this.rentSumUpDown.InterceptArrowKeys = false;
            this.rentSumUpDown.Location = new System.Drawing.Point(187, 191);
            this.rentSumUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.rentSumUpDown.Name = "paymentSumUpDown";
            this.rentSumUpDown.Size = new System.Drawing.Size(248, 20);
            this.rentSumUpDown.TabIndex = 20;
            // 
            // payDateUpDown
            // 
            this.paymentDayUpDown.Location = new System.Drawing.Point(187, 228);
            this.paymentDayUpDown.Maximum = new decimal(new int[] {
            28,
            0,
            0,
            0});
            this.paymentDayUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.paymentDayUpDown.Name = "payDateUpDown";
            this.paymentDayUpDown.Size = new System.Drawing.Size(248, 20);
            this.paymentDayUpDown.TabIndex = 21;
            this.paymentDayUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // newTermsPenaltyUpDown
            // 
            this.penaltyUpDown.DecimalPlaces = 5;
            this.penaltyUpDown.Location = new System.Drawing.Point(187, 267);
            this.penaltyUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.penaltyUpDown.Name = "newTermsPenaltyUpDown";
            this.penaltyUpDown.Size = new System.Drawing.Size(248, 20);
            this.penaltyUpDown.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(418, 152);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 164);
            this.label10.TabIndex = 23;
            this.label10.Text = "         ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "label11";
            // 
            // termsEndMonthCalendar
            // 
            this.endTermMonthCalendar.Location = new System.Drawing.Point(7, 362);
            this.endTermMonthCalendar.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.endTermMonthCalendar.MaxSelectionCount = 1;
            this.endTermMonthCalendar.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.endTermMonthCalendar.Name = "termsEndMonthCalendar";
            this.endTermMonthCalendar.ShowToday = false;
            this.endTermMonthCalendar.ShowTodayCircle = false;
            this.endTermMonthCalendar.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Дата завершения договора";
            // 
            // NewPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 579);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.endTermMonthCalendar);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.penaltyUpDown);
            this.Controls.Add(this.paymentDayUpDown);
            this.Controls.Add(this.rentSumUpDown);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.startTermMonthCalendar);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "NewPerson";
            this.Text = "NewPerson";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.nameTextBox, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.descriptionTextBox, 0);
            this.Controls.SetChildIndex(this.startTermMonthCalendar, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.rentSumUpDown, 0);
            this.Controls.SetChildIndex(this.paymentDayUpDown, 0);
            this.Controls.SetChildIndex(this.penaltyUpDown, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.endTermMonthCalendar, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            ((System.ComponentModel.ISupportInitialize)(this.rentSumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paymentDayUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penaltyUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MonthCalendar startTermMonthCalendar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown rentSumUpDown;
        private System.Windows.Forms.NumericUpDown paymentDayUpDown;
        private System.Windows.Forms.NumericUpDown penaltyUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.MonthCalendar endTermMonthCalendar;
        private System.Windows.Forms.Label label4;
    }
}