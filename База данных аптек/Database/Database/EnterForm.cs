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
    public partial class EnterForm : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../mainBase.accdb");
        int numberSellers;
        public EnterForm()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0){
                textBox1.Enabled = false;
                textBox3.Enabled = false;
            }
            else{
                textBox3.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        int successLogin(string login, string password, int type){
            string _login, _password;
            int number, group;
           
            
            OleDbCommand cmd = new OleDbCommand("Select * From Пользователи", conn); //создаем команду
            using (OleDbDataReader dr = cmd.ExecuteReader()){ //выполняем
                while (dr.Read()){ //читаем результат
                    _login = dr.GetValue(1).ToString();
                    _password = dr.GetValue(2).ToString();
                    group = System.Convert.ToInt32(dr.GetValue(4).ToString());
                    number = System.Convert.ToInt32(dr.GetValue(5).ToString());
                    numberSellers = System.Convert.ToInt32(dr.GetValue(6).ToString());
                    if (login == _login){
                        if (type == group)
                            if (password == _password)
                                return number;
                            else{
                                MessageBox.Show("Неправильный пароль!");
                                return -1;
                            }
                        else{
                            MessageBox.Show("Нет прав доступа!");
                            return -1;
                        }
                    }
                }
            }
            MessageBox.Show("Пользователь не найден!");
            return -1;
        }

        private void button1_Click(object sender, EventArgs e){
            if (textBox1.Text != "" || comboBox1.SelectedIndex == 0){
                if (textBox3.Text != "" || comboBox1.SelectedIndex == 0){
                    if (comboBox1.SelectedIndex == 0){
                        conn.Close();
                        QuestForm quest = new QuestForm();
                        quest.ShowDialog();
                    }
                    if (comboBox1.SelectedIndex == 1){
                        if (successLogin(textBox1.Text, textBox3.Text, 0) >= 0){
                            conn.Close();
                            AdminForm admin = new AdminForm();
                            admin.AdminLogin = textBox1.Text;
                            admin.ShowDialog();
                        }
                    }
                    if (comboBox1.SelectedIndex == 2){
                        int branch = successLogin(textBox1.Text, textBox3.Text, 1);
                        if (branch > 0){
                            conn.Close();
                            SellerForm seller = new SellerForm();
                            seller.sellerLogin = textBox1.Text;
                            seller.numberBranche = branch;
                            seller.numberSellers = numberSellers;
                            seller.ShowDialog();
                        }
                    }
                }
                else
                    MessageBox.Show("Введите пароль!");
            }
            else
                MessageBox.Show("Введите логин!");
        }

        private void EnterForm_Load(object sender, EventArgs e)
        {
            conn.Open();
        }
    }
}
