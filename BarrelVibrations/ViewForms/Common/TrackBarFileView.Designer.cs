namespace BarrelVibrations.ViewForms.Common
{
    partial class TrackBarFileView
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.RunAnimationButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AnimationDurationNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.AnimationIntervalsNumeric = new System.Windows.Forms.NumericUpDown();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeTrackBar = new System.Windows.Forms.TrackBar();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CopyButton = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationDurationNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationIntervalsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.RunAnimationButton);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.AnimationDurationNumeric);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.AnimationIntervalsNumeric);
            this.groupBox.Controls.Add(this.timeLabel);
            this.groupBox.Controls.Add(this.timeTrackBar);
            this.groupBox.Controls.Add(this.listBox);
            this.groupBox.Controls.Add(this.SaveButton);
            this.groupBox.Controls.Add(this.CopyButton);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(800, 450);
            this.groupBox.TabIndex = 4;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Название";
            // 
            // RunAnimationButton
            // 
            this.RunAnimationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RunAnimationButton.Location = new System.Drawing.Point(676, 216);
            this.RunAnimationButton.Name = "RunAnimationButton";
            this.RunAnimationButton.Size = new System.Drawing.Size(112, 34);
            this.RunAnimationButton.TabIndex = 11;
            this.RunAnimationButton.Text = "Анимировать";
            this.RunAnimationButton.UseVisualStyleBackColor = true;
            this.RunAnimationButton.Click += new System.EventHandler(this.RunAnimationButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(676, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 38);
            this.label2.TabIndex = 10;
            this.label2.Text = "Время\r\nанимации, сек:";
            // 
            // AnimationDurationNumeric
            // 
            this.AnimationDurationNumeric.Location = new System.Drawing.Point(676, 294);
            this.AnimationDurationNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.AnimationDurationNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.AnimationDurationNumeric.Name = "AnimationDurationNumeric";
            this.AnimationDurationNumeric.Size = new System.Drawing.Size(112, 26);
            this.AnimationDurationNumeric.TabIndex = 9;
            this.AnimationDurationNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(676, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Интервалов:";
            // 
            // AnimationIntervalsNumeric
            // 
            this.AnimationIntervalsNumeric.Location = new System.Drawing.Point(676, 345);
            this.AnimationIntervalsNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.AnimationIntervalsNumeric.Name = "AnimationIntervalsNumeric";
            this.AnimationIntervalsNumeric.Size = new System.Drawing.Size(112, 26);
            this.AnimationIntervalsNumeric.TabIndex = 7;
            this.AnimationIntervalsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // timeLabel
            // 
            this.timeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(676, 393);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(49, 19);
            this.timeLabel.TabIndex = 6;
            this.timeLabel.Text = "t: 0 мс";
            // 
            // timeTrackBar
            // 
            this.timeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeTrackBar.Location = new System.Drawing.Point(6, 393);
            this.timeTrackBar.Name = "timeTrackBar";
            this.timeTrackBar.Size = new System.Drawing.Size(664, 45);
            this.timeTrackBar.TabIndex = 5;
            this.timeTrackBar.ValueChanged += new System.EventHandler(this.timeTrackBar_ValueChanged);
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 19;
            this.listBox.Location = new System.Drawing.Point(6, 25);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(664, 346);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(676, 65);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 34);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Сохранить";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyButton.Location = new System.Drawing.Point(676, 25);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(112, 34);
            this.CopyButton.TabIndex = 2;
            this.CopyButton.Text = "Скопировать";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // TrackBarFileView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox);
            this.Name = "TrackBarFileView";
            this.Text = "TrackbarView";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationDurationNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationIntervalsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected GroupBox groupBox;
        protected Label timeLabel;
        protected TrackBar timeTrackBar;
        protected ListBox listBox;
        private Button SaveButton;
        private Button CopyButton;
        private Button RunAnimationButton;
        private Label label2;
        private NumericUpDown AnimationDurationNumeric;
        private Label label1;
        private NumericUpDown AnimationIntervalsNumeric;
    }
}