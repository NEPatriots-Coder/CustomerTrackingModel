using System;
using System.Windows.Forms;
using CRM.Desktop.Models;

namespace CRM.Desktop.Forms
{
    public class PODetailForm : Form
    {
        private readonly PurchaseOrder? _currentPO;
        private readonly TextBox txtPONumber;
        private readonly TextBox txtSupplier;
        private readonly DateTimePicker dtpOrderDate;
        private readonly DataGridView dgvLineItems;
        private readonly Button btnSave;
        private readonly Button btnCancel;
        private readonly Button btnAddLine;
        private readonly Button btnRemoveLine;

        public PODetailForm(PurchaseOrder? po = null)
        {
            _currentPO = po;
            txtPONumber = new TextBox();
            txtSupplier = new TextBox();
            dtpOrderDate = new DateTimePicker();
            dgvLineItems = new DataGridView();
            btnSave = new Button();
            btnCancel = new Button();
            btnAddLine = new Button();
            btnRemoveLine = new Button();

            InitializeComponents();
            if (po != null)
            {
                LoadPOData();
            }
        }

        private void InitializeComponents()
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.Text = _currentPO == null ? "New Purchase Order" : "Edit Purchase Order";

            // Initialize main controls
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            // Header panel for PO details
            var headerPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            // PO Number
            headerPanel.Controls.Add(new Label { Text = "PO Number:", Dock = DockStyle.Fill });
            txtPONumber.Dock = DockStyle.Fill;
            headerPanel.Controls.Add(txtPONumber);

            // Supplier
            headerPanel.Controls.Add(new Label { Text = "Supplier:", Dock = DockStyle.Fill });
            txtSupplier.Dock = DockStyle.Fill;
            headerPanel.Controls.Add(txtSupplier);

            // Order Date
            headerPanel.Controls.Add(new Label { Text = "Order Date:", Dock = DockStyle.Fill });
            dtpOrderDate.Dock = DockStyle.Fill;
            headerPanel.Controls.Add(dtpOrderDate);

            // Line Items Grid
            dgvLineItems.Dock = DockStyle.Fill;
            dgvLineItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLineItems.AllowUserToAddRows = true;
            dgvLineItems.AllowUserToDeleteRows = true;
            dgvLineItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "PartNumber", HeaderText = "Part Number" });
            dgvLineItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Description" });
            dgvLineItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Quantity" });
            dgvLineItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "UnitPrice", HeaderText = "Unit Price" });
            dgvLineItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalPrice", HeaderText = "Total Price", ReadOnly = true });

            // Buttons Panel
            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(5)
            };

            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;
            buttonsPanel.Controls.Add(btnSave);

            btnCancel.Text = "Cancel";
            btnCancel.Click += (s, e) => this.Close();
            buttonsPanel.Controls.Add(btnCancel);

            btnAddLine.Text = "Add Line";
            btnAddLine.Click += BtnAddLine_Click;
            buttonsPanel.Controls.Add(btnAddLine);

            btnRemoveLine.Text = "Remove Line";
            btnRemoveLine.Click += BtnRemoveLine_Click;
            buttonsPanel.Controls.Add(btnRemoveLine);

            mainPanel.Controls.Add(headerPanel);
            mainPanel.Controls.Add(dgvLineItems);
            mainPanel.Controls.Add(buttonsPanel);

            this.Controls.Add(mainPanel);
        }

        private void LoadPOData()
        {
            if (_currentPO == null) return;

            txtPONumber.Text = _currentPO.PONumber;
            txtSupplier.Text = _currentPO.SupplierName;
            dtpOrderDate.Value = _currentPO.OrderDate;

            foreach (var lineItem in _currentPO.LineItems)
            {
                dgvLineItems.Rows.Add(lineItem.PartNumber, lineItem.Description, lineItem.Quantity, lineItem.UnitPrice, lineItem.TotalPrice);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (_currentPO != null)
                {
                    _currentPO.PONumber = txtPONumber.Text;
                    _currentPO.SupplierName = txtSupplier.Text;
                    _currentPO.OrderDate = dtpOrderDate.Value;
                    _currentPO.LineItems.Clear();

                    foreach (DataGridViewRow row in dgvLineItems.Rows)
                    {
                        if (row.IsNewRow) continue;

                        var lineItem = new POLineItem
                        {
                            PartNumber = row.Cells["PartNumber"].Value?.ToString() ?? string.Empty,
                            Description = row.Cells["Description"].Value?.ToString(),
                            Quantity = int.Parse(row.Cells["Quantity"].Value?.ToString() ?? "0"),
                            UnitPrice = decimal.Parse(row.Cells["UnitPrice"].Value?.ToString() ?? "0")
                        };
                        _currentPO.LineItems.Add(lineItem);
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void BtnAddLine_Click(object? sender, EventArgs e)
        {
            dgvLineItems.Rows.Add();
        }

        private void BtnRemoveLine_Click(object? sender, EventArgs e)
        {
            if (dgvLineItems.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvLineItems.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        dgvLineItems.Rows.Remove(row);
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtPONumber.Text))
            {
                MessageBox.Show("PO Number is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSupplier.Text))
            {
                MessageBox.Show("Supplier is required.");
                return false;
            }

            return true;
        }
    }
}