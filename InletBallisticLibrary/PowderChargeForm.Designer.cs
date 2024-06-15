namespace InletBallisticLibrary
{
    partial class PowderChargeForm
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
            this.components = new System.ComponentModel.Container();
            this.powdersListBox = new System.Windows.Forms.ListBox();
            this.powdersListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPowderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.addTubePowderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.addGrainedPowderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePowderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.powdersPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.powdersSplitContainer = new System.Windows.Forms.SplitContainer();
            this.powdersListToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.powdersListContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.powdersSplitContainer)).BeginInit();
            this.powdersSplitContainer.Panel1.SuspendLayout();
            this.powdersSplitContainer.Panel2.SuspendLayout();
            this.powdersSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // powdersListBox
            // 
            this.powdersListBox.AllowDrop = true;
            this.powdersListBox.ContextMenuStrip = this.powdersListContextMenu;
            this.powdersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powdersListBox.FormattingEnabled = true;
            this.powdersListBox.ItemHeight = 15;
            this.powdersListBox.Location = new System.Drawing.Point(0, 0);
            this.powdersListBox.Name = "powdersListBox";
            this.powdersListBox.Size = new System.Drawing.Size(381, 479);
            this.powdersListBox.TabIndex = 0;
            this.powdersListToolTip.SetToolTip(this.powdersListBox, "Для добавления пороха нажмите ПКМ.\r\nЗажмите ctrl и нажимайте стрелки вверх или вн" +
        "из чтобы изменить порядок элементов.\r\n");
            this.powdersListBox.SelectedIndexChanged += new System.EventHandler(this.powdersListBox_SelectedIndexChanged);
            this.powdersListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.powdersListBox_KeyDown);
            // 
            // powdersListContextMenu
            // 
            this.powdersListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPowderButton,
            this.deletePowderButton});
            this.powdersListContextMenu.Name = "powdersListContextMenu";
            this.powdersListContextMenu.Size = new System.Drawing.Size(127, 48);
            // 
            // addPowderButton
            // 
            this.addPowderButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTubePowderButton,
            this.addGrainedPowderButton});
            this.addPowderButton.Name = "addPowderButton";
            this.addPowderButton.Size = new System.Drawing.Size(126, 22);
            this.addPowderButton.Text = "Добавить";
            // 
            // addTubePowderButton
            // 
            this.addTubePowderButton.Name = "addTubePowderButton";
            this.addTubePowderButton.Size = new System.Drawing.Size(180, 22);
            this.addTubePowderButton.Text = "Трубчатый";
            this.addTubePowderButton.Click += new System.EventHandler(this.addTubePowderButton_Click);
            // 
            // addGrainedPowderButton
            // 
            this.addGrainedPowderButton.Name = "addGrainedPowderButton";
            this.addGrainedPowderButton.Size = new System.Drawing.Size(180, 22);
            this.addGrainedPowderButton.Text = "Зернёный";
            this.addGrainedPowderButton.Click += new System.EventHandler(this.addGrainedPowderButton_Click);
            // 
            // deletePowderButton
            // 
            this.deletePowderButton.Name = "deletePowderButton";
            this.deletePowderButton.Size = new System.Drawing.Size(126, 22);
            this.deletePowderButton.Text = "Удалить";
            this.deletePowderButton.Click += new System.EventHandler(this.deletePowderButton_Click);
            // 
            // powdersPropertyGrid
            // 
            this.powdersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powdersPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.powdersPropertyGrid.Name = "powdersPropertyGrid";
            this.powdersPropertyGrid.Size = new System.Drawing.Size(359, 479);
            this.powdersPropertyGrid.TabIndex = 1;
            this.powdersPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.powdersPropertyGrid_PropertyValueChanged);
            // 
            // powdersSplitContainer
            // 
            this.powdersSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powdersSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.powdersSplitContainer.Name = "powdersSplitContainer";
            // 
            // powdersSplitContainer.Panel1
            // 
            this.powdersSplitContainer.Panel1.Controls.Add(this.powdersPropertyGrid);
            // 
            // powdersSplitContainer.Panel2
            // 
            this.powdersSplitContainer.Panel2.Controls.Add(this.powdersListBox);
            this.powdersSplitContainer.Size = new System.Drawing.Size(750, 479);
            this.powdersSplitContainer.SplitterDistance = 359;
            this.powdersSplitContainer.SplitterWidth = 10;
            this.powdersSplitContainer.TabIndex = 2;
            // 
            // powdersListToolTip
            // 
            this.powdersListToolTip.ToolTipTitle = "Подсказка";
            // 
            // PowderChargeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 479);
            this.Controls.Add(this.powdersSplitContainer);
            this.Name = "PowderChargeForm";
            this.Text = "Редактор порохового заряда";
            this.powdersListContextMenu.ResumeLayout(false);
            this.powdersSplitContainer.Panel1.ResumeLayout(false);
            this.powdersSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.powdersSplitContainer)).EndInit();
            this.powdersSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox powdersListBox;
        private PropertyGrid powdersPropertyGrid;
        private SplitContainer powdersSplitContainer;
        private ContextMenuStrip powdersListContextMenu;
        private ToolStripMenuItem addPowderButton;
        private ToolStripMenuItem deletePowderButton;
        private ToolStripMenuItem addTubePowderButton;
        private ToolStripMenuItem addGrainedPowderButton;
        private ToolTip powdersListToolTip;
    }
}