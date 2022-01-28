namespace WordlHelper
{
    partial class Form1
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
            this.guessBoxesPanel = new System.Windows.Forms.TableLayoutPanel();
            this.possibleWordsBox = new System.Windows.Forms.RichTextBox();
            this.analyzeButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.matchesCountLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // guessBoxesPanel
            // 
            this.guessBoxesPanel.ColumnCount = 1;
            this.guessBoxesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.guessBoxesPanel.Location = new System.Drawing.Point(40, 28);
            this.guessBoxesPanel.Name = "guessBoxesPanel";
            this.guessBoxesPanel.RowCount = 6;
            this.guessBoxesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.guessBoxesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.guessBoxesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.guessBoxesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.guessBoxesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.guessBoxesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.guessBoxesPanel.Size = new System.Drawing.Size(417, 453);
            this.guessBoxesPanel.TabIndex = 0;
            // 
            // possibleWordsBox
            // 
            this.possibleWordsBox.BulletIndent = 1;
            this.possibleWordsBox.DetectUrls = false;
            this.possibleWordsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.possibleWordsBox.Location = new System.Drawing.Point(557, 28);
            this.possibleWordsBox.Name = "possibleWordsBox";
            this.possibleWordsBox.ReadOnly = true;
            this.possibleWordsBox.Size = new System.Drawing.Size(243, 453);
            this.possibleWordsBox.TabIndex = 1;
            this.possibleWordsBox.Text = "";
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(322, 526);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(201, 50);
            this.analyzeButton.TabIndex = 2;
            this.analyzeButton.Text = "Analyze";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(322, 582);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(201, 27);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // matchesCountLabel
            // 
            this.matchesCountLabel.AutoSize = true;
            this.matchesCountLabel.Location = new System.Drawing.Point(559, 487);
            this.matchesCountLabel.Name = "matchesCountLabel";
            this.matchesCountLabel.Size = new System.Drawing.Size(54, 13);
            this.matchesCountLabel.TabIndex = 4;
            this.matchesCountLabel.Text = "Matches: ";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 553);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 582);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 6;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 623);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.matchesCountLabel);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.possibleWordsBox);
            this.Controls.Add(this.guessBoxesPanel);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel guessBoxesPanel;
        private System.Windows.Forms.RichTextBox possibleWordsBox;
        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label matchesCountLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
    }
}

