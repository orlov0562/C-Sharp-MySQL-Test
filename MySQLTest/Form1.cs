using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySQLTest
{
    public partial class Form1 : Form
    {
        private Sql _sql;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            _sql = new Sql();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            String query = "SELECT * FROM `test`";

            textBox1.AppendText(query + Environment.NewLine);
            try { 
                var results = _sql.Fetch(query);

                foreach (var res in results)
                {
                    listView1.Items.Add( new ListViewItem(new []{res["id"], res["text"]}));
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERR: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String query = "INSERT INTO `test` SET `text`='" + _sql.Esc(textBox2.Text) + "'";
            textBox1.AppendText(query + Environment.NewLine);
            try
            {
                _sql.Query(query);
                button1_Click(sender, e);
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERR: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String query = "INSERT INTO `test` SET `text`=?text";
            textBox1.AppendText(query + "| ?text:" + textBox2.Text + Environment.NewLine);

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("?text", textBox2.Text)    
            };
            try { 
                _sql.PreparedQuery(query, parameters);
                button1_Click(sender, e);
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERR: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                textBox1.AppendText("ERR: Select element first");
                MessageBox.Show("Select element first");
                return;
            }
            String selectedItemId = listView1.Items[listView1.SelectedIndices[0]].Text;
            String query = "DELETE FROM `test` WHERE `id`=" + _sql.Esc(selectedItemId);
            textBox1.AppendText(query + Environment.NewLine);
            try { 
                _sql.Query(query);
                button1_Click(sender, e);
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERR: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                textBox1.AppendText("ERR: Select element first" + Environment.NewLine);
                MessageBox.Show("Select element first");
                return;
            }
            String selectedItemId = listView1.Items[listView1.SelectedIndices[0]].Text;
            String query = "UPDATE `test` SET `text`='" + _sql.Esc(textBox2.Text) + "' WHERE `id`=" + _sql.Esc(selectedItemId);
            textBox1.AppendText(query + Environment.NewLine);
            try { 
                _sql.Query(query);
                button1_Click(sender, e);
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERR: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                textBox2.Text = listView1.Items[listView1.SelectedIndices[0]].SubItems[1].Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Try to connect.."+Environment.NewLine);
            try
            {
                if (_sql.Connect(textBox3.Text, int.Parse(textBox4.Text), textBox5.Text, textBox6.Text, textBox7.Text))
                {
                    textBox1.AppendText("Connected" + Environment.NewLine);
                    MessageBox.Show("Connected");
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText("ERR: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
