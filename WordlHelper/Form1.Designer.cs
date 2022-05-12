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
            this.analyzeButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.matchesCountLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.selectedWordLabel = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.showWordCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.wordSelectedLabel = new System.Windows.Forms.Label();
            this.testWordTextBox = new System.Windows.Forms.TextBox();
            this.possibleWordsList = new System.Windows.Forms.ListBox();
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
            this.matchesCountLabel.Size = new System.Drawing.Size(41, 13);
            this.matchesCountLabel.TabIndex = 4;
            this.matchesCountLabel.Text = "Count: ";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 553);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Visible = false;
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
            this.loadButton.Visible = false;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(167, 526);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(91, 35);
            this.playButton.TabIndex = 7;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // selectedWordLabel
            // 
            this.selectedWordLabel.AutoSize = true;
            this.selectedWordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedWordLabel.Location = new System.Drawing.Point(164, 589);
            this.selectedWordLabel.Name = "selectedWordLabel";
            this.selectedWordLabel.Size = new System.Drawing.Size(52, 16);
            this.selectedWordLabel.TabIndex = 8;
            this.selectedWordLabel.Text = "Word: ";
            this.selectedWordLabel.Visible = false;
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(751, 582);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(69, 26);
            this.testButton.TabIndex = 9;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Visible = false;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // showWordCheckBox
            // 
            this.showWordCheckBox.AutoSize = true;
            this.showWordCheckBox.Location = new System.Drawing.Point(167, 567);
            this.showWordCheckBox.Name = "showWordCheckBox";
            this.showWordCheckBox.Size = new System.Drawing.Size(82, 17);
            this.showWordCheckBox.TabIndex = 10;
            this.showWordCheckBox.Text = "Show Word";
            this.showWordCheckBox.UseVisualStyleBackColor = true;
            this.showWordCheckBox.CheckedChanged += new System.EventHandler(this.showWordCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(559, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Possible Words:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Guess Boxes (Double-Click to change state):";
            // 
            // wordSelectedLabel
            // 
            this.wordSelectedLabel.AutoSize = true;
            this.wordSelectedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.wordSelectedLabel.Location = new System.Drawing.Point(164, 510);
            this.wordSelectedLabel.Name = "wordSelectedLabel";
            this.wordSelectedLabel.Size = new System.Drawing.Size(119, 13);
            this.wordSelectedLabel.TabIndex = 13;
            this.wordSelectedLabel.Text = "Random word selected!";
            this.wordSelectedLabel.Visible = false;
            // 
            // testWordTextBox
            // 
            this.testWordTextBox.Location = new System.Drawing.Point(90, 534);
            this.testWordTextBox.Name = "testWordTextBox";
            this.testWordTextBox.Size = new System.Drawing.Size(71, 20);
            this.testWordTextBox.TabIndex = 14;
            this.testWordTextBox.Visible = false;
            // 
            // possibleWordsList
            // 
            this.possibleWordsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.possibleWordsList.FormattingEnabled = true;
            this.possibleWordsList.ItemHeight = 16;
            this.possibleWordsList.Location = new System.Drawing.Point(562, 27);
            this.possibleWordsList.Name = "possibleWordsList";
            this.possibleWordsList.Size = new System.Drawing.Size(195, 452);
            this.possibleWordsList.TabIndex = 15;
            this.possibleWordsList.DoubleClick += new System.EventHandler(this.possibleWordsList_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 623);
            this.Controls.Add(this.possibleWordsList);
            this.Controls.Add(this.testWordTextBox);
            this.Controls.Add(this.wordSelectedLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showWordCheckBox);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.selectedWordLabel);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.matchesCountLabel);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.guessBoxesPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wordl Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel guessBoxesPanel;
        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label matchesCountLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Label selectedWordLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.CheckBox showWordCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label wordSelectedLabel;
        private System.Windows.Forms.TextBox testWordTextBox;
        private System.Windows.Forms.ListBox possibleWordsList;
    }
}

