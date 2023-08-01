namespace meteogame
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pPlayer = new System.Windows.Forms.PictureBox();
            this.pGameover = new System.Windows.Forms.PictureBox();
            this.pTitle = new System.Windows.Forms.PictureBox();
            this.pEXP = new System.Windows.Forms.PictureBox();
            this.pMsg = new System.Windows.Forms.PictureBox();
            this.pBase = new System.Windows.Forms.PictureBox();
            this.pMeteor = new System.Windows.Forms.PictureBox();
            this.pBG = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGameover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pEXP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMsg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMeteor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBG)).BeginInit();
            this.SuspendLayout();
            // 
            // pPlayer
            // 
            this.pPlayer.Image = global::meteogame.Properties.Resources.p_player;
            this.pPlayer.Location = new System.Drawing.Point(573, 12);
            this.pPlayer.Name = "pPlayer";
            this.pPlayer.Size = new System.Drawing.Size(100, 50);
            this.pPlayer.TabIndex = 7;
            this.pPlayer.TabStop = false;
            // 
            // pGameover
            // 
            this.pGameover.Image = global::meteogame.Properties.Resources.p_gameover;
            this.pGameover.Location = new System.Drawing.Point(573, 85);
            this.pGameover.Name = "pGameover";
            this.pGameover.Size = new System.Drawing.Size(100, 50);
            this.pGameover.TabIndex = 6;
            this.pGameover.TabStop = false;
            // 
            // pTitle
            // 
            this.pTitle.Image = global::meteogame.Properties.Resources.p_title;
            this.pTitle.Location = new System.Drawing.Point(573, 154);
            this.pTitle.Name = "pTitle";
            this.pTitle.Size = new System.Drawing.Size(100, 50);
            this.pTitle.TabIndex = 5;
            this.pTitle.TabStop = false;
            // 
            // pEXP
            // 
            this.pEXP.Image = global::meteogame.Properties.Resources.p_explosion;
            this.pEXP.Location = new System.Drawing.Point(321, 314);
            this.pEXP.Name = "pEXP";
            this.pEXP.Size = new System.Drawing.Size(366, 115);
            this.pEXP.TabIndex = 4;
            this.pEXP.TabStop = false;
            // 
            // pMsg
            // 
            this.pMsg.Image = global::meteogame.Properties.Resources.p_msg;
            this.pMsg.Location = new System.Drawing.Point(573, 242);
            this.pMsg.Name = "pMsg";
            this.pMsg.Size = new System.Drawing.Size(100, 50);
            this.pMsg.TabIndex = 3;
            this.pMsg.TabStop = false;
            // 
            // pBase
            // 
            this.pBase.Location = new System.Drawing.Point(33, 44);
            this.pBase.Name = "pBase";
            this.pBase.Size = new System.Drawing.Size(242, 229);
            this.pBase.TabIndex = 2;
            this.pBase.TabStop = false;
            // 
            // pMeteor
            // 
            this.pMeteor.Image = global::meteogame.Properties.Resources.p_meteor;
            this.pMeteor.Location = new System.Drawing.Point(374, 214);
            this.pMeteor.Name = "pMeteor";
            this.pMeteor.Size = new System.Drawing.Size(97, 78);
            this.pMeteor.TabIndex = 1;
            this.pMeteor.TabStop = false;
            // 
            // pBG
            // 
            this.pBG.Image = global::meteogame.Properties.Resources.p_bg;
            this.pBG.Location = new System.Drawing.Point(328, 44);
            this.pBG.Name = "pBG";
            this.pBG.Size = new System.Drawing.Size(191, 143);
            this.pBG.TabIndex = 0;
            this.pBG.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 441);
            this.Controls.Add(this.pPlayer);
            this.Controls.Add(this.pGameover);
            this.Controls.Add(this.pTitle);
            this.Controls.Add(this.pEXP);
            this.Controls.Add(this.pMsg);
            this.Controls.Add(this.pBase);
            this.Controls.Add(this.pMeteor);
            this.Controls.Add(this.pBG);
            this.Name = "Form1";
            this.Text = "メテオ";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGameover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pEXP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMsg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMeteor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBG;
        private System.Windows.Forms.PictureBox pMeteor;
        private System.Windows.Forms.PictureBox pBase;
        private System.Windows.Forms.PictureBox pMsg;
        private System.Windows.Forms.PictureBox pEXP;
        private System.Windows.Forms.PictureBox pTitle;
        private System.Windows.Forms.PictureBox pGameover;
        private System.Windows.Forms.PictureBox pPlayer;
    }
}

