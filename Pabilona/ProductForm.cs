using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pabilona
{
    public partial class ProductForm : Form
    {
        private List<string> savedBarcodes = new List<string>();
        public ProductForm()
        {
            InitializeComponent();

            // Category options
            cmbCategory.Items.Add("Electronics");
            cmbCategory.Items.Add("Food & Beverages");
            cmbCategory.Items.Add("Clothing");
            cmbCategory.Items.Add("Accessories");
            cmbCategory.Items.Add("Household");

            // Automatically calculate the price
            txtCost.TextChanged += CalculatePrice;
            txtMarkup.TextChanged += CalculatePrice;

            // Check barcode availability
            txtBarcode.TextChanged += txtBarcode_TextChanged;

            // Button events
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnCancel.Click += btnCancel_Click;

            // Price is automatically calculated
            txtPrice.ReadOnly = true;
        }


        private void CalculatePrice(object sender, EventArgs e)
        {
            decimal cost;
            decimal markup;

            if (decimal.TryParse(txtCost.Text, out cost) &&
                decimal.TryParse(txtMarkup.Text, out markup))
            {
                if (cost > 0 && markup >= 0)
                {
                    decimal price = cost + (cost * markup / 100);

                    txtPrice.Text = price.ToString("0.00");
                }
                else
                {
                    txtPrice.Clear();
                }
            }
            else
            {
                txtPrice.Clear();
            }
        }
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim();

            if (barcode == "")
            {
                lblBarcodeStatus.Text = "Barcode is required.";
            }
            else if (savedBarcodes.Contains(barcode))
            {
                lblBarcodeStatus.Text = "Barcode already exists.";
            }
            else
            {
                lblBarcodeStatus.Text = "Barcode is available.";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim();
            string name = txtName.Text.Trim();

            // Validate barcode
            if (barcode == "")
            {
                MessageBox.Show("Please enter a barcode.");
                txtBarcode.Focus();
                return;
            }

            // Validate name
            if (name == "")
            {
                MessageBox.Show("Please enter the product name.");
                txtName.Focus();
                return;
            }

            // Validate category
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.");
                cmbCategory.Focus();
                return;
            }

            // Validate origin
            if (!rbLocal.Checked && !rbAbroad.Checked)
            {
                MessageBox.Show("Please select the product origin.");
                return;
            }

            // Validate cost
            decimal cost;

            if (!decimal.TryParse(txtCost.Text, out cost) || cost <= 0)
            {
                MessageBox.Show("Please enter a valid cost.");
                txtCost.Focus();
                return;
            }

            // Validate markup
            decimal markup;

            if (!decimal.TryParse(txtMarkup.Text, out markup) || markup < 0)
            {
                MessageBox.Show("Please enter a valid markup percentage.");
                txtMarkup.Focus();
                return;
            }

            // Check duplicate barcode
            if (savedBarcodes.Contains(barcode))
            {
                MessageBox.Show("Barcode already exists.");
                txtBarcode.Focus();
                return;
            }

            // Calculate price
            decimal price = cost + (cost * markup / 100);

            // Save barcode temporarily
            savedBarcodes.Add(barcode);

            MessageBox.Show(
                "Product successfully saved!\n\n" +
                "Product Name: " + name + "\n" +
                "Category: " + cmbCategory.Text + "\n" +
                "Origin: " + (rbLocal.Checked ? "Local" : "Abroad") + "\n" +
                "Cost: " + cost.ToString("0.00") + "\n" +
                "Price: " + price.ToString("0.00") + "\n" +
                "Markup: " + markup.ToString("0.00") + "%",
                "Product Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ClearFields();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all fields?","Confirm Clear", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearFields();
            }
        }

        // CLEAR ALL INPUTS
        private void ClearFields()
        {
            txtBarcode.Clear();
            txtName.Clear();
            txtDescription.Clear();

            cmbCategory.SelectedIndex = -1;

            rbLocal.Checked = false;
            rbAbroad.Checked = false;

            chkActive.Checked = false;

            txtCost.Clear();
            txtPrice.Clear();
            txtMarkup.Clear();

            lblBarcodeStatus.Text = "Barcode is available.";

            txtBarcode.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Are you sure you want to exit?","Confirm Exit",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtMarkup_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
