using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDEF6
{
    public partial class Form1 : Form
    {
        int index=-1;
        private readonly string[] HeaderNames = new string[4] { "Name", "Value", "Date", "Operation" };
        public void UpdateDGV()
        {
            using (DBContext db = new DBContext())
            {
                var ds = db.Operation.Select(o => new { o.Name, OperationValue = o.Value, dateOfOperation = o.OperationTime, typeOfOperation = o.OperationType.Name });
                dataGridView1.DataSource = ds.ToList();
                for (int i = 0; i < 4; i++)
                    dataGridView1.Columns[i].HeaderText = HeaderNames[i];
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridView1.AutoGenerateColumns = true;
            UpdateDGV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(index==-1)
            {
                MessageBox.Show("Choose operation for edit!");
            }
            else
            {
                using (DBContext db = new DBContext())
                {
                    var operation = db.Operation.AsEnumerable().ElementAt(index);
                    try
                    {
                        operation.Value = Convert.ToDecimal(textBox2.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Enter correct value of operation!");
                    }
                    operation.Name = textBox1.Text;
                    operation.OperationType = db.OperationType.First(o=>o.Name==comboBox1.Text);
                    db.SaveChanges();
                    MessageBox.Show("OK!");
                    UpdateDGV();
                }
            }
        }

   

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            this.Hide();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            using (DBContext db = new DBContext())
            {
                index = e.RowIndex;
                var operation = db.Operation.AsEnumerable().ElementAt(index);
                textBox1.Text = operation.Name;
                textBox2.Text = operation.Value.ToString();
                comboBox1.Text = operation.OperationType.Name;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (index == -1)
            {
                MessageBox.Show("Choose operation for edit!");
            }
            else
            {
                using (DBContext db = new DBContext())
                {
                    var operation = db.Operation.AsEnumerable().ElementAt(index);
                    db.Operation.Remove(operation);
                    index = -1;
                    db.SaveChanges();
                    UpdateDGV();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    comboBox1.SelectedIndex = -1;
                    comboBox1.Text = "";
                    MessageBox.Show("OK!");
                }
            }
        }
    }
}
