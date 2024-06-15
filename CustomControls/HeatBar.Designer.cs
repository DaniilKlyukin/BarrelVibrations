
namespace CustomControls
{
    partial class HeatBar
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.HeatMapPictureBox = new System.Windows.Forms.PictureBox();
            this.HeatMapLabelsPanel = new System.Windows.Forms.Panel();
            this.MeasurementUnitsLabel = new CustomControls.OrientedTextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.HeatMapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // HeatMapPictureBox
            // 
            this.HeatMapPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.HeatMapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.HeatMapPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.HeatMapPictureBox.Name = "HeatMapPictureBox";
            this.HeatMapPictureBox.Size = new System.Drawing.Size(63, 660);
            this.HeatMapPictureBox.TabIndex = 0;
            this.HeatMapPictureBox.TabStop = false;
            // 
            // HeatMapLabelsPanel
            // 
            this.HeatMapLabelsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.HeatMapLabelsPanel.Location = new System.Drawing.Point(63, 0);
            this.HeatMapLabelsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.HeatMapLabelsPanel.Name = "HeatMapLabelsPanel";
            this.HeatMapLabelsPanel.Size = new System.Drawing.Size(94, 660);
            this.HeatMapLabelsPanel.TabIndex = 1;
            // 
            // MeasurementUnitsLabel
            // 
            this.MeasurementUnitsLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.MeasurementUnitsLabel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MeasurementUnitsLabel.Location = new System.Drawing.Point(157, 0);
            this.MeasurementUnitsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.MeasurementUnitsLabel.Name = "MeasurementUnitsLabel";
            this.MeasurementUnitsLabel.RotationAngle = 270D;
            this.MeasurementUnitsLabel.Size = new System.Drawing.Size(33, 660);
            this.MeasurementUnitsLabel.TabIndex = 2;
            this.MeasurementUnitsLabel.TextDirection = CustomControls.Direction.Clockwise;
            this.MeasurementUnitsLabel.TextOrientation = CustomControls.Orientation.Rotate;
            // 
            // HeatBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HeatMapPictureBox);
            this.Controls.Add(this.HeatMapLabelsPanel);
            this.Controls.Add(this.MeasurementUnitsLabel);
            this.Name = "HeatBar";
            this.Size = new System.Drawing.Size(190, 660);
            this.DockChanged += new System.EventHandler(this.HeatBar_DockChanged);
            this.SizeChanged += new System.EventHandler(this.HeatBar_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.HeatMapPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox HeatMapPictureBox;
        private System.Windows.Forms.Panel HeatMapLabelsPanel;
        private OrientedTextLabel MeasurementUnitsLabel;
    }
}
