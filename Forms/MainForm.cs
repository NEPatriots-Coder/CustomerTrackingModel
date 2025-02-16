using System;
using System.Drawing;
using System.Windows.Forms;
using CRM.Desktop.Models;

namespace CRM.Desktop.Forms
{
    public class MainForm : Form
    {
        private readonly DataGridView dgvPurchaseOrders;
        private readonly Button btnNewPO;
        private readonly Button btnEditPO;
        private readonly Button btnDeletePO;
        private readonly Button btnRefresh;
        private readonly Panel buttonPanel;

        public MainForm()
        {
            dgvPurchaseOrders = new DataGridView();
            btnNewPO = new Button();
            btnEditPO = new Button();
            btnDeletePO = new Button();
            btnRefresh = new Button();
            buttonPanel = new Panel();

            InitializeForm();
            InitializeComponents();
            WireUpEvents();
            LoadInitialData();
        }

        private void InitializeForm()
        {
            Text = "Purchase Order Management";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponents()
        {
            InitializeButtonPanel();
            InitializeButtons();
            InitializeGrid();
            
            Controls.Add(dgvPurchaseOrders);
            Controls.Add(buttonPanel);
            buttonPanel.BringToFront();
        }

        private void InitializeButtonPanel()
        {
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 40;
            buttonPanel.BackColor = Color.WhiteSmoke;
        }

        private void InitializeButtons()
        {
            ConfigureButton(btnNewPO, "New PO", 10);
            ConfigureButton(btnEditPO, "Edit PO", 120);
            ConfigureButton(btnDeletePO, "Delete PO", 230);
            ConfigureButton(btnRefresh, "Refresh", 340);

            buttonPanel.Controls.AddRange(new Control[] 
            { 
                btnNewPO, 
                btnEditPO, 
                btnDeletePO, 
                btnRefresh 
            });
        }

        private static void ConfigureButton(Button button, string text, int x)
        {
            button.Text = text;
            button.Location = new Point(x, 5);
            button.Size = new Size(100, 30);
            button.BackColor = Color.White;
        }

        private void InitializeGrid()
        {
            dgvPurchaseOrders.Dock = DockStyle.Fill;
            dgvPurchaseOrders.BackgroundColor = Color.White;
            dgvPurchaseOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPurchaseOrders.RowHeadersVisible = false;
            dgvPurchaseOrders.AllowUserToAddRows = false;
            dgvPurchaseOrders.ReadOnly = true;
            dgvPurchaseOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvPurchaseOrders.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "PONumber", HeaderText = "PO Number", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "OrderDate", HeaderText = "Order Date", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "SupplierName", HeaderText = "Supplier", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "TotalAmount", HeaderText = "Total Amount", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 100 }
            });
        }

        private void WireUpEvents()
        {
            btnNewPO.Click += BtnNewPO_Click;
            btnEditPO.Click += BtnEditPO_Click;
            btnDeletePO.Click += BtnDeletePO_Click;
            btnRefresh.Click += BtnRefresh_Click;
            dgvPurchaseOrders.CellDoubleClick += (s, e) => BtnEditPO_Click(s, e);
        }

        private void LoadInitialData()
        {
            dgvPurchaseOrders.Rows.Clear();
            // TODO: Replace with actual data loading from a service
            dgvPurchaseOrders.Rows.Add("PO-2024-001", DateTime.Now, "Test Supplier", 1500.00m, "Open");
            dgvPurchaseOrders.Rows.Add("PO-2024-002", DateTime.Now.AddDays(-1), "Another Supplier", 2500.00m, "Pending");
        }

        private void BtnNewPO_Click(object? sender, EventArgs e)
        {
            using var poForm = new PODetailForm();
            if (poForm.ShowDialog() == DialogResult.OK)
            {
                RefreshGrid();
            }
        }

        private void BtnEditPO_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.CurrentRow != null)
            {
                var po = GetSelectedPurchaseOrder();
                if (po != null)
                {
                    using var poForm = new PODetailForm(po);
                    if (poForm.ShowDialog() == DialogResult.OK)
                    {
                        RefreshGrid();
                    }
                }
            }
        }

        private void BtnDeletePO_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.CurrentRow == null) return;

            if (MessageBox.Show(
                "Are you sure you want to delete this Purchase Order?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // TODO: Implement delete functionality using a service
                RefreshGrid();
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e) => RefreshGrid();

        private void RefreshGrid()
        {
            // TODO: Implement actual data refresh from a service
            LoadInitialData();
        }

        private PurchaseOrder? GetSelectedPurchaseOrder()
        {
            if (dgvPurchaseOrders.CurrentRow == null) return null;

            try
            {
                return new PurchaseOrder
                {
                    PONumber = dgvPurchaseOrders.CurrentRow.Cells["PONumber"].Value.ToString()!,
                    SupplierName = dgvPurchaseOrders.CurrentRow.Cells["SupplierName"].Value.ToString()!,
                    OrderDate = DateTime.Parse(dgvPurchaseOrders.CurrentRow.Cells["OrderDate"].Value.ToString()!),
                    Status = dgvPurchaseOrders.CurrentRow.Cells["Status"].Value.ToString()!,
                    TotalAmount = decimal.Parse(dgvPurchaseOrders.CurrentRow.Cells["TotalAmount"].Value.ToString()!)
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading Purchase Order data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}