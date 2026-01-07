using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void MostButtons_Click(object sender, EventArgs e)
        {
            textBox1.Text += ((Button)sender).Tag.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void txtCalculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys
            if (char.IsControl(e.KeyChar))
                return;

            // Digits
            if (char.IsDigit(e.KeyChar))
                return;

            // Division
            if (e.KeyChar == '/')
            {
                e.Handled = true;
                InsertOperator(" ÷ ");
                return;
            }

            // Multiplication
            if (e.KeyChar == '*')
            {
                e.Handled = true;
                InsertOperator(" × ");
                return;
            }

            // + - %
            if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '%')
            {
                e.Handled = true;
                InsertOperator($" {e.KeyChar} ");
                return;
            }

            // Block everything else
            e.Handled = true;
        }

        private void InsertOperator(string op)
        {
            int pos = textBox1.SelectionStart;
            textBox1.Text = textBox1.Text.Insert(pos, op);
            textBox1.SelectionStart = pos + op.Length;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                textBox1.SelectionStart = textBox1.Text.Length;
            }

            textBox1.Focus(); // optional but recommended
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    return;

                string expression = textBox1.Text
                    .Replace('×', '*')
                    .Replace('÷', '/')
                    .Replace("−", "-")   // Unicode minus safeguard
                    .Trim();

                DataTable table = new DataTable();
                object result = table.Compute(expression, "");

                textBox1.Text = result.ToString();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            catch
            {
                MessageBox.Show("Invalid expression", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
