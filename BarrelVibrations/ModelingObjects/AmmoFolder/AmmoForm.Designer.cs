namespace BarrelVibrations.ModelingObjects.AmmoFolder
{
    partial class AmmoForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MissilePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PowderChargePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AmmoListBox = new System.Windows.Forms.ListBox();
            this.DeleteAmmoButton = new System.Windows.Forms.Button();
            this.AddAmmoButton = new System.Windows.Forms.Button();
            this.AmmoNameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.MeshMissileButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MeshMissileButton);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Controls.Add(this.DeleteAmmoButton);
            this.splitContainer1.Panel2.Controls.Add(this.AddAmmoButton);
            this.splitContainer1.Panel2.Controls.Add(this.AmmoNameLabel);
            this.splitContainer1.Panel2.Controls.Add(this.NameTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(1074, 605);
            this.splitContainer1.SplitterDistance = 412;
            this.splitContainer1.SplitterWidth = 12;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(412, 605);
            this.splitContainer2.SplitterDistance = 299;
            this.splitContainer2.SplitterWidth = 12;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.MissilePropertyGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 299);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Снаряд";
            // 
            // MissilePropertyGrid
            // 
            this.MissilePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MissilePropertyGrid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MissilePropertyGrid.Location = new System.Drawing.Point(3, 22);
            this.MissilePropertyGrid.Name = "MissilePropertyGrid";
            this.MissilePropertyGrid.Size = new System.Drawing.Size(406, 274);
            this.MissilePropertyGrid.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PowderChargePropertyGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 294);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Пороховой заряд";
            // 
            // PowderChargePropertyGrid
            // 
            this.PowderChargePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PowderChargePropertyGrid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PowderChargePropertyGrid.Location = new System.Drawing.Point(3, 22);
            this.PowderChargePropertyGrid.Name = "PowderChargePropertyGrid";
            this.PowderChargePropertyGrid.Size = new System.Drawing.Size(406, 269);
            this.PowderChargePropertyGrid.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.AmmoListBox);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox3.Location = new System.Drawing.Point(10, 12);
            this.groupBox3.MinimumSize = new System.Drawing.Size(0, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(604, 552);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Боеприпасы";
            // 
            // AmmoListBox
            // 
            this.AmmoListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AmmoListBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AmmoListBox.FormattingEnabled = true;
            this.AmmoListBox.ItemHeight = 19;
            this.AmmoListBox.Location = new System.Drawing.Point(3, 22);
            this.AmmoListBox.Name = "AmmoListBox";
            this.AmmoListBox.Size = new System.Drawing.Size(598, 527);
            this.AmmoListBox.TabIndex = 0;
            this.AmmoListBox.SelectedIndexChanged += new System.EventHandler(this.AmmoListBox_SelectedIndexChanged);
            // 
            // DeleteAmmoButton
            // 
            this.DeleteAmmoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteAmmoButton.Location = new System.Drawing.Point(332, 570);
            this.DeleteAmmoButton.Name = "DeleteAmmoButton";
            this.DeleteAmmoButton.Size = new System.Drawing.Size(88, 23);
            this.DeleteAmmoButton.TabIndex = 4;
            this.DeleteAmmoButton.Text = "Удалить";
            this.DeleteAmmoButton.UseVisualStyleBackColor = true;
            this.DeleteAmmoButton.Click += new System.EventHandler(this.DeleteAmmoButton_Click);
            // 
            // AddAmmoButton
            // 
            this.AddAmmoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddAmmoButton.Location = new System.Drawing.Point(238, 570);
            this.AddAmmoButton.Name = "AddAmmoButton";
            this.AddAmmoButton.Size = new System.Drawing.Size(88, 23);
            this.AddAmmoButton.TabIndex = 3;
            this.AddAmmoButton.Text = "Добавить";
            this.AddAmmoButton.UseVisualStyleBackColor = true;
            this.AddAmmoButton.Click += new System.EventHandler(this.AddAmmoButton_Click);
            // 
            // AmmoNameLabel
            // 
            this.AmmoNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AmmoNameLabel.AutoSize = true;
            this.AmmoNameLabel.Location = new System.Drawing.Point(10, 573);
            this.AmmoNameLabel.Name = "AmmoNameLabel";
            this.AmmoNameLabel.Size = new System.Drawing.Size(62, 15);
            this.AmmoNameLabel.TabIndex = 2;
            this.AmmoNameLabel.Text = "Название:";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NameTextBox.Location = new System.Drawing.Point(78, 570);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(154, 23);
            this.NameTextBox.TabIndex = 1;
            this.NameTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.NameTextBox_PreviewKeyDown);
            // 
            // MeshMissileButton
            // 
            this.MeshMissileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MeshMissileButton.Location = new System.Drawing.Point(426, 570);
            this.MeshMissileButton.Name = "MeshMissileButton";
            this.MeshMissileButton.Size = new System.Drawing.Size(187, 23);
            this.MeshMissileButton.TabIndex = 6;
            this.MeshMissileButton.Text = "Рассчитать физику снаряда";
            this.MeshMissileButton.UseVisualStyleBackColor = true;
            this.MeshMissileButton.Click += new System.EventHandler(this.MeshMissileButton_Click);
            // 
            // AmmoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 605);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(1090, 400);
            this.Name = "AmmoForm";
            this.Text = "Боеприпасы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AmmoForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private PropertyGrid MissilePropertyGrid;
        private PropertyGrid PowderChargePropertyGrid;
        private Button DeleteAmmoButton;
        private Button AddAmmoButton;
        private Label AmmoNameLabel;
        private TextBox NameTextBox;
        private ListBox AmmoListBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button MeshMissileButton;
    }
}