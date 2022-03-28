using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDEF6
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (DBContext db = new DBContext())
            {
                foreach (Control c in Controls)
                {
                    if (c is TextBox textBox && textBox.Text == "" || c is ComboBox comboBox && comboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("Incorrect data!");
                        return;
                    }
                }
                decimal value = 0;
                try
                {
                    value = Convert.ToDecimal(textBox2.Text);
                }
                catch
                {
                    MessageBox.Show("Enter valid decimal value of Operation value!");
                }

                var operationType = db.OperationType.First(o => o.Name == comboBox1.Text);

                var operation = new Operation
                {
                    Name = textBox1.Text,
                    Value = value,
                    OperationType = operationType,
                    OperationTime = DateTime.Now,
                };
                db.Operation.Add(operation);
                db.SaveChanges();
                MessageBox.Show("OK!");
                Form1 f = new Form1();
                f.Show();
                Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

        }
    }
}
