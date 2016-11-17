using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Database
{
    public partial class SellerForm : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../mainBase.accdb");
        OleDbConnection connLocal;
        public string sellerLogin;
        public int numberBranche;
        public int numberSellers;
        public SellerForm()
        {
            InitializeComponent();
        }

        string getCommand(bool type)
        {
            bool check = false;
            string command = "Select * From Лекарства";
            if (type)
                return command;
            command += " where ";
            if (textBox1.Text != "")
            {
                command += " Название = '" + textBox1.Text + "'";
                check = true;
            }
            if (textBox2.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Производитель = '" + textBox2.Text + "'";
                check = true;
            }
            if (textBox3.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Описание = '" + textBox3.Text + "'";
                check = true;
            }
            if (textBox4.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Категория = '" + textBox4.Text + "'";
                check = true;
            }
            if (textBox5.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Форм-фактор = '" + textBox5.Text + "'";
                check = true;
            }
            if (textBox6.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Объем = " + textBox6.Text;
                check = true;
            }
            if (textBox7.Text != "")
                command += "and Цена = " + textBox7.Text;
            return command;
        }

        string getCommandById(string id)
        { 
            string command = "SELECT Лекарства.[Количество товара на складе], Лекарства.[Расположение товара на складе] "+
                " FROM Лекарства "+
                "where Лекарства.[Номер лекарства] = " + id+";";
            return command;
        }

        void load(bool type)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            conn.Open();
            connLocal.Open();
            OleDbCommand cmd = new OleDbCommand(getCommand(type), conn); //создаем команду
            OleDbCommand cmd2;
            OleDbDataReader dr2;
            comboBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            using (OleDbDataReader dr = cmd.ExecuteReader())
            { //выполняем
                while (dr.Read())
                { //читаем результат
                    dataGridView1.Rows.Add();
                    comboBox1.Items.Add(dr.GetValue(1).ToString());
                    comboBox2.Items.Add(dr.GetValue(1).ToString());
                    comboBox3.Items.Add(dr.GetValue(1).ToString());
                    dataGridView1[0, i].Value = dr.GetValue(1).ToString();
                    dataGridView1[1, i].Value = dr.GetValue(2).ToString();
                    dataGridView1[2, i].Value = dr.GetValue(3).ToString();
                    dataGridView1[3, i].Value = dr.GetValue(4).ToString();
                    dataGridView1[4, i].Value = dr.GetValue(5).ToString();
                    dataGridView1[5, i].Value = dr.GetValue(6).ToString();
                    dataGridView1[6, i].Value = dr.GetValue(7).ToString();
                    dataGridView1[7, i].Value = dr.GetValue(0).ToString();
                    cmd2 = new OleDbCommand(getCommandById(dr.GetValue(0).ToString()),connLocal);
                
                    dr2 = cmd2.ExecuteReader();
                    dr2.Read();
                    dataGridView1[8, i].Value = dr2.GetValue(0).ToString();
                    dataGridView1[9, i].Value = dr2.GetValue(1).ToString();
                    dataGridView1.Rows[i].ReadOnly = true;
                    i++;
                }
            }
            conn.Close();
            connLocal.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
                load(true);
            else
                load(false);
        }

        private void SellerForm_Load(object sender, EventArgs e)
        {
            connLocal= new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../localBase" + numberBranche + ".accdb");
            load(true);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = System.Convert.ToDecimal(dataGridView1[8,comboBox1.SelectedIndex].Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox8.Text = (System.Convert.ToInt32(numericUpDown1.Value) * System.Convert.ToInt32(dataGridView1[6, comboBox1.SelectedIndex].Value)).ToString();
        }

        string getDate()
        {
            return DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year;
        }
        string getTime()
        {
            return DateTime.Now.Hour + ":" + DateTime.Now.Minute;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string idProduct = dataGridView1[7, comboBox1.SelectedIndex].Value.ToString();
            int selled = System.Convert.ToInt32(numericUpDown1.Value);
            int newCount = System.Convert.ToInt32(numericUpDown1.Maximum) - selled;
            conn.Open();
            connLocal.Open();
            string commandSale = "INSERT INTO Продажи ([Номер товара], [Количество], [Дата], [Время], [Номер филиала], [Номер продавца] ) " +
            " Values ("+ idProduct+ ", "+ selled+",'"+getDate()+"','"+getTime()+"',"+numberBranche+","+numberSellers+");";
            string updateProduct = "Update Лекарства set [Количество товара на складе] = " + newCount + " where [Номер лекарства] = " + idProduct + ";";
            OleDbCommand cmd = new OleDbCommand(commandSale, conn); //создаем команду
            OleDbCommand cmd2 = new OleDbCommand(updateProduct,connLocal);
            cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery(); 
            connLocal.Close();
            conn.Close();
            load(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string idProduct = dataGridView1[7, comboBox2.SelectedIndex].Value.ToString();
            int count = System.Convert.ToInt32(dataGridView1[8, comboBox2.SelectedIndex].Value.ToString());
            int newCount = System.Convert.ToInt32(numericUpDown2.Value) + count;
            connLocal.Open();
            string updateProduct = "Update Лекарства set [Количество товара на складе] = " + newCount + ",[Расположение товара на складе] = '"+textBox9.Text+"'  where [Номер лекарства] = " + idProduct + ";";
            OleDbCommand cmd2 = new OleDbCommand(updateProduct, connLocal);
            cmd2.ExecuteNonQuery();
            connLocal.Close();
            load(true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string idProduct = dataGridView1[7, comboBox3.SelectedIndex].Value.ToString();
            int newCount = System.Convert.ToInt32(numericUpDown3.Value);
            connLocal.Open();
            string updateProduct = "Insert into [Закупки] ( [Номер лекарства],[Необходимое количество], [Дата составления]) Values ("+idProduct+","+newCount+",'"+getDate()+"')";
            OleDbCommand cmd2 = new OleDbCommand(updateProduct, connLocal);
            cmd2.ExecuteNonQuery();
            connLocal.Close();
            numericUpDown3.Value = 0;
            comboBox3.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox9.Text = dataGridView1[9, comboBox2.SelectedIndex].Value.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ListOrder list = new ListOrder();
            list.numberBranch = numberBranche;
            list.ShowDialog();
        }
    }
}
