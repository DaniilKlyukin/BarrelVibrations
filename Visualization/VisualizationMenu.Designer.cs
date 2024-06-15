
namespace Visualization
{
    partial class VisualizationMenu
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
            this.timeMomentTrackBar = new System.Windows.Forms.TrackBar();
            this.timeMomentLabel = new System.Windows.Forms.Label();
            this.startVisualizationButton = new System.Windows.Forms.Button();
            this.stopVisualizationButton = new System.Windows.Forms.Button();
            this.OpenVideoFolderButton = new System.Windows.Forms.Button();
            this.MoveToStartAnimation = new System.Windows.Forms.Button();
            this.FirstTimeMomentButton = new System.Windows.Forms.Button();
            this.LastTimeMomentButton = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.timeMomentTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // timeMomentTrackBar
            // 
            this.timeMomentTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeMomentTrackBar.Location = new System.Drawing.Point(12, 12);
            this.timeMomentTrackBar.Name = "timeMomentTrackBar";
            this.timeMomentTrackBar.Size = new System.Drawing.Size(669, 45);
            this.timeMomentTrackBar.TabIndex = 0;
            this.timeMomentTrackBar.ValueChanged += new System.EventHandler(this.timeMomentTrackBar_ValueChanged);
            // 
            // timeMomentLabel
            // 
            this.timeMomentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeMomentLabel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.timeMomentLabel.Location = new System.Drawing.Point(12, 60);
            this.timeMomentLabel.Name = "timeMomentLabel";
            this.timeMomentLabel.Size = new System.Drawing.Size(669, 21);
            this.timeMomentLabel.TabIndex = 1;
            this.timeMomentLabel.Text = "Момент времени";
            this.timeMomentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startVisualizationButton
            // 
            this.startVisualizationButton.BackgroundImage = Resource.start;
            this.startVisualizationButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.startVisualizationButton.Location = new System.Drawing.Point(12, 84);
            this.startVisualizationButton.Name = "startVisualizationButton";
            this.startVisualizationButton.Size = new System.Drawing.Size(50, 50);
            this.startVisualizationButton.TabIndex = 2;
            this.startVisualizationButton.UseVisualStyleBackColor = false;
            this.startVisualizationButton.Click += new System.EventHandler(this.startVisualizationButton_Click);
            // 
            // stopVisualizationButton
            // 
            this.stopVisualizationButton.BackgroundImage = Resource.pause;
            this.stopVisualizationButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.stopVisualizationButton.Location = new System.Drawing.Point(68, 84);
            this.stopVisualizationButton.Name = "stopVisualizationButton";
            this.stopVisualizationButton.Size = new System.Drawing.Size(50, 50);
            this.stopVisualizationButton.TabIndex = 3;
            this.stopVisualizationButton.UseVisualStyleBackColor = true;
            this.stopVisualizationButton.Click += new System.EventHandler(this.stopVisualizationButton_Click);
            // 
            // OpenVideoFolderButton
            // 
            this.OpenVideoFolderButton.BackgroundImage = Resource.folder;
            this.OpenVideoFolderButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OpenVideoFolderButton.Location = new System.Drawing.Point(68, 196);
            this.OpenVideoFolderButton.Name = "OpenVideoFolderButton";
            this.OpenVideoFolderButton.Size = new System.Drawing.Size(50, 50);
            this.OpenVideoFolderButton.TabIndex = 10;
            this.OpenVideoFolderButton.UseVisualStyleBackColor = true;
            // 
            // MoveToStartAnimation
            // 
            this.MoveToStartAnimation.BackColor = System.Drawing.SystemColors.Control;
            this.MoveToStartAnimation.BackgroundImage = Resource.refresh;
            this.MoveToStartAnimation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveToStartAnimation.Location = new System.Drawing.Point(12, 196);
            this.MoveToStartAnimation.Name = "MoveToStartAnimation";
            this.MoveToStartAnimation.Size = new System.Drawing.Size(50, 50);
            this.MoveToStartAnimation.TabIndex = 11;
            this.MoveToStartAnimation.UseVisualStyleBackColor = false;
            this.MoveToStartAnimation.Click += new System.EventHandler(this.MoveToStartAnimation_Click);
            // 
            // FirstTimeMomentButton
            // 
            this.FirstTimeMomentButton.BackColor = System.Drawing.SystemColors.Control;
            this.FirstTimeMomentButton.BackgroundImage = Resource.to_start;
            this.FirstTimeMomentButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FirstTimeMomentButton.Location = new System.Drawing.Point(12, 140);
            this.FirstTimeMomentButton.Name = "FirstTimeMomentButton";
            this.FirstTimeMomentButton.Size = new System.Drawing.Size(50, 50);
            this.FirstTimeMomentButton.TabIndex = 13;
            this.FirstTimeMomentButton.UseVisualStyleBackColor = false;
            this.FirstTimeMomentButton.Click += new System.EventHandler(this.FirstTimeMomentButton_Click);
            // 
            // LastTimeMomentButton
            // 
            this.LastTimeMomentButton.BackColor = System.Drawing.SystemColors.Control;
            this.LastTimeMomentButton.BackgroundImage = Resource.to_end;
            this.LastTimeMomentButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LastTimeMomentButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LastTimeMomentButton.Location = new System.Drawing.Point(68, 140);
            this.LastTimeMomentButton.Name = "LastTimeMomentButton";
            this.LastTimeMomentButton.Size = new System.Drawing.Size(50, 50);
            this.LastTimeMomentButton.TabIndex = 12;
            this.LastTimeMomentButton.UseVisualStyleBackColor = false;
            this.LastTimeMomentButton.Click += new System.EventHandler(this.LastTimeMomentButton_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(124, 84);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(557, 294);
            this.propertyGrid.TabIndex = 14;
            // 
            // VisualizationMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 390);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.FirstTimeMomentButton);
            this.Controls.Add(this.LastTimeMomentButton);
            this.Controls.Add(this.MoveToStartAnimation);
            this.Controls.Add(this.OpenVideoFolderButton);
            this.Controls.Add(this.stopVisualizationButton);
            this.Controls.Add(this.startVisualizationButton);
            this.Controls.Add(this.timeMomentLabel);
            this.Controls.Add(this.timeMomentTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "VisualizationMenu";
            this.Text = "Параметры визуализации";
            ((System.ComponentModel.ISupportInitialize)(this.timeMomentTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar timeMomentTrackBar;
        private System.Windows.Forms.Label timeMomentLabel;
        private System.Windows.Forms.Button startVisualizationButton;
        private System.Windows.Forms.Button stopVisualizationButton;
        private System.Windows.Forms.Button OpenVideoFolderButton;
        private System.Windows.Forms.Button MoveToStartAnimation;
        private System.Windows.Forms.Button FirstTimeMomentButton;
        private System.Windows.Forms.Button LastTimeMomentButton;
        private System.Windows.Forms.PropertyGrid propertyGrid;
    }
}