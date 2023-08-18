using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace meteogame
{
    public partial class Form1 : Form
    {
        static Bitmap canvas = new Bitmap(480, 320);
        Graphics gg = Graphics.FromImage(canvas);
        Random rand = new Random();

        Point Cpos;              //カーソル座標
        int PW, PH;              //自機の幅と高さ
        //int CW, CH;

        //隕石設定
        int[] enX = new int[10]; //隕石のX座標
        int[] enY = new int[10]; //隕石のY座標
        int[] RR = new int[10];  //隕石大きさ

        //アイテム設定
        int[] IMX = new int[5]; //アイテムのX座標
        int[] IMY = new int[5]; //アイテムのY座標
        int ITM;                //アイテムの半径

        //ビーム設定
        int[] BMX = new int[10]; //ビームのX座標
        int[] BMY = new int[10]; //ビームのY座標
        int BMW, BMH;            //ビームの幅と高さ

        //貫通アイテム設定
        int[] KIX = new int[5]; //貫通アイテムのX座標
        int[] KIY = new int[5]; //貫通アイテムのY座標
        int KIM;                //貫通アイテムの半径
        int KBW, KBH;           //貫通ビームの幅と高さ
        int kantsuCount;        //貫通アイテム獲得個数カウント

        //CPU設定
        int CPU, CPUX , CPUY;   //CPUの大きさ、X、Y座標
        int CPBX, CPBY;         //CPUビームのX、Y座標
        int CPBW, CPBH;         //CPUビームの幅、高さ
        int CPUB;
        int CPmove = +2;        //CPUの左右移動


        // Boolean 初期値：false
        Boolean titleFlg;        //true:タイトル表示中
        Boolean hitFlg;          //true:当たった
        Boolean[] getFlg;        //true:ゲットした
        Boolean weakBeamFlg;     //クリックしたらビーム発射なので最初はfalse状態にする
        Boolean[] kantsuItemFlg;
        Boolean kantsuCountFlg;
        Boolean strongBeamFlg;
        Boolean GameoverFlg;

        int ecnt;                //爆発演出用
        long msgcnt;             //メッセージ用カウンタ
        long score;              //スコア
        long highscore;          //ハイスコア,初回のみ0に

        Font myFont = new Font("Arial", 16);

        public Form1()
        {
            InitializeComponent();
            Size = Size - ClientSize + new Size(480, 320);
            //現在のフォームのサイズ = (現在のフォームのサイズ - 周囲の余白を除いたサイズ) + 設定したい画面サイズ(ここでは480x320)
            //(現在のフォームのサイズ - 周囲の余白を除いたサイズ)を計算することで周囲の余白のサイズを計算しています。また、その値に設定したい画面サイズ(480x320)を足してあげることで余白を考慮した画面サイズを設定しています。
        }

        ///////////////////////////////////////////
        ///Form1読み込み
        ///////////////////////////////////////////
        private void Form1_Load(object sender, EventArgs e)
        {
            //隠し
            pMeteor.Hide();
            pPlayer.Hide();
            pItem.Hide();
            weakBeam.Hide();
            kantsuItem.Hide();
            strongBeam.Hide();
            strongBeamyoko.Hide();
            pCPU.Hide();
            CPUBeam.Hide();
            pBG.Hide();
            pEXP.Hide();
            pGameover.Hide();
            pMsg.Hide();
            pTitle.Hide();

            pBase.Width = ClientSize.Width; //pBaseをフォームのクライアント領域と同じに　幅
            pBase.Height = ClientSize.Height; //pBaseをフォームのクライアント領域と同じに　高さ

            initGame(); //初期処理
        }

        ///////////////////////////////////////////
        ///ゲーム初期化
        ///////////////////////////////////////////
        private void initGame()
        {
            PW = 41; //自機の幅
            PH = 51; //自機の高さ
            ITM = 40 / 2;//アイテムの大きさ
            KIM = 40 / 2;//貫通アイテムの大きさ
            BMW = 20; //ビームの幅を20
            BMH = 20; //ビームの高さを10
            KBW = 40; //貫通ビームの幅を20
            KBH = 40; //貫通ビームの高さを10
            CPU = 80 / 2;//CPUの大きさ
            CPUX = pCPU.Left;
            CPUY = pCPU.Width - 55;
            CPBW = 50; //CPUビームの幅
            CPBH = 50; //CPUビームの高さ
            CPBX = 50;
            CPBY = 80;
            CPUB = 80 / 2;

            //隕石大きさ・量と落ちてくる場所の指定
            for (int i = 0; i < 10; i++)
            {
                enX[i] = rand.Next(1, 450);        //隕石の初期配置座標
                enY[i] = rand.Next(1, 900) - 1000; //落下の間隔にバラつきを持たせる
                RR[i] = rand.Next(20, 60);         //隕石の大きさをランダムに
            }

            //アイテムの落下場所指定
            for (int i = 0; i < 5; i++)
            {
                IMX[i] = rand.Next(1, 450);
                IMY[i] = rand.Next(1, 900) - 1000;
            }

            //貫通アイテムの落下場所指定
            for (int i = 0; i < 5; i++)
            {
                KIX[i] = rand.Next(1, 450);
                KIY[i] = rand.Next(1, 900) - 1000;
            }

            /*
            //CPUビームの落下場所指定
            for (int i = 0; i < 5; i++)
            {
                CPUBeam.Top = CH;                              //ビーム発射Y座標
                CPUBeam.Left = CW + CH + (CW / 2) - (CPW / 2); //自機の真ん中を算出➡ + 自機の幅/2 - ビームの幅/2
                gg.DrawImage(CPUBeam.Image, new Rectangle(CPUBeam.Left, CPUBeam.Top, CPW, CPH));
            }
            */

            hitFlg = false;
            titleFlg = true;
            getFlg = new bool[] { false, false, false, false, false };
            kantsuItemFlg = new bool[] { false, false, false, false, false };
            GameoverFlg = false;
            ecnt = 0;
            msgcnt = 0;
            score = 0;
            highscore = 0;
            kantsuCount = 0;
            initCPUBeam();
        }

        ///////////////////////////////////////////
        /// ペースクリック時
        ///////////////////////////////////////////
        private void pBase_Click(object sender, EventArgs e)
        {
            if (titleFlg)
            {
                if (msgcnt > 20)
                {
                    msgcnt = 0;
                    titleFlg = false; //タイトル非表示
                }
                return; //タイトル表示中はこの先の処理をしない
            }

            if (GameoverFlg)
            {
                initGame();
            }
                
            if (titleFlg == false)
            {
                if (weakBeam.Top + BMH < 0 && strongBeam.Top + KBH < 0)
                {
                    if (kantsuCount == 0)
                    {
                        initBeam();
                        weakBeamFlg = true;
                    }
                    else
                    {
                        initkantsuBeam();
                        strongBeamFlg = true;
                        kantsuCount--;
                    }
                }
            }

        }

        ///////////////////////////////////////////
        /// 爆発演出
        ///////////////////////////////////////////
        private void playerExplosion()
        {
            weakBeamFlg = false;
            //爆発演出の描画は、全てここで行う
            gg.DrawImage(pBG.Image, 0, 0, 480, 320);

            //爆発後隕石をその場所に残す
            for (int i = 0; i < 10; i++)
            {
                gg.DrawImage(pMeteor.Image, enX[i], enY[i], RR[i] * 2, RR[i] * 2);
            }

            //爆発後アイテムをその場所に残す
            for (int i = 0; i < 5; i++)
            {
                gg.DrawImage(pItem.Image, IMX[i], IMY[i], ITM * 2, ITM * 2);
            }

            //爆発後貫通アイテムをその場所に残す
            for (int i = 0; i < 5; i++)
            {
                gg.DrawImage(kantsuItem.Image, KIX[i], KIY[i], KIM * 2, KIM * 2);
            }

            //爆発後CPUをその場所に残す
            if(score > 100)
            {
                gg.DrawImage(pCPU.Image, CPUX, CPUY, CPU * 2, CPU * 2);
                gg.DrawImage(CPUBeam.Image, new Rectangle(CPBX, CPBY - 20, CPBW + 50, CPBH + 20));
            }

            if (ecnt < 16) //16フレームのアニメにする,カウンタと表示する画像の位置を決める役目
            {
                GraphicsUnit units = GraphicsUnit.Pixel;//画像の一部をずらしながら表示する

                //マウスカーソル位置で爆発演出
                gg.DrawImage(pEXP.Image, Cpos.X + PW / 2 - 50, Cpos.Y + PH / 2 - 50,
                    new Rectangle(ecnt / 2 * 100, 0, 100, 100), units);//new Rectangle( x, y, 幅, 高さ ) で表示する位置と、切り出すサイズを指定

                ecnt++;
            }
            msgcnt++;
            if (msgcnt > 60)
            {
                gg.DrawImage(pGameover.Image, 70, 80, 350, 60); //ゲームオーバー画像のサイズ指定を 350, 60 としています。
                if (msgcnt % 60 > 20) //変数 msgcnt の数がどんどん増えても、0から59 の間を算出
                {
                    gg.DrawImage(pMsg.Image, 110, 190, 271, 26);
                }
            }

            //スコア・ハイスコア表示    
            gg.DrawString("SCORE: " + score.ToString(), myFont, Brushes.White, 10, 10);
            gg.DrawString("HIGHSCORE: " + highscore.ToString(), myFont, Brushes.White, 10, 40);

            GameoverFlg = true; //爆発と同時にフラグを立てて確実にクリック時のinitGameを実行する為

            pBase.Image = canvas;
        }

        ///////////////////////////////////////////
        /// アイテム獲得演出
        ///////////////////////////////////////////
        private void playerGetitem(int i)
        {
            if (getFlg[i])
            {
                getFlg[i] = false;                //ヒットしたアイテムのフラグを落とす
                score += 5000;                    // ポイント加算
                IMX[i] = rand.Next(1, 450);       // ヒットしたアイテムの座標を初期化
                IMY[i] = rand.Next(1, 900) - 1000;
            }

            else if (kantsuItemFlg[i])
            {
                kantsuItemFlg[i] = false;
                KIX[i] = rand.Next(1, 450);
                KIY[i] = rand.Next(1, 900) - 1000;
            }
        }

        ///////////////////////////////////////////
        /// タイトル表示
        ///////////////////////////////////////////
        private void dispTitle()
        {
            msgcnt++;
            gg.DrawImage(pBG.Image, 0, 0, 480, 320);
            gg.DrawImage(pTitle.Image, 70, 80, 350, 60);
            if (msgcnt % 60 > 20)
            {
                gg.DrawImage(pMsg.Image, 110, 190, 271, 26);
            }
            pBase.Image = canvas;
        }

        ///////////////////////////////////////////
        /// タイマーイベント
        ///////////////////////////////////////////
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (titleFlg)
            {
                dispTitle();
                return; //タイトル表示中はこの先の処理をしない
            }
            if (hitFlg)
            {
                playerExplosion();
                return; //自機と隕石が当たった後、この先の処理をしない
            }
            for (int i = 0; i < getFlg.Length; i++) //getFlgの配列分ループ処理する。今回で言うと5個分
            {
                if (getFlg[i])
                {
                    playerGetitem(i);
                }
            }

            for (int i = 0; i < kantsuItemFlg.Length; i++) //getFlgの配列分ループ処理する。今回で言うと5個分
            {
                if (kantsuItemFlg[i])
                {
                    playerGetitem(i);
                }
            }

            if (kantsuCountFlg) //貫通アイテムは5個以下に設定される
            {
                kantsuCount++;
                kantsuCountFlg = false;
            }

            gg.DrawImage(pBG.Image, 0, 0, 480, 320);

            // 隕石の移動
            for (int i = 0; i < 10; i++)
            {
                if ((RR[i]) > 30) //隕石の大きさが30以上の場合
                {
                    enY[i] += 2; //下へ移動、大きくて遅い状態
                }
                else if ((RR[i]) < 30) //隕石の大きさが30以下の場合
                {
                    enY[i] += 6; //小さくて早い状態
                }
                gg.DrawImage(pMeteor.Image, enX[i], enY[i], RR[i] * 2, RR[i] * 2); //描画 RRは半径なので2倍にする
                if (enY[i] > pBase.Height) //pBase.Height よりも大きいときは、隕石が画面外へ出たことになるので画面の上に戻す処理
                {
                    enX[i] = rand.Next(1, 450);
                    enY[i] = rand.Next(1, 300) - 400;
                }
            }

            // アイテムの移動
            for (int i = 0; i < 1; i++)
            {
                IMY[i] += 2; //アイテム落下スペード
                gg.DrawImage(pItem.Image, IMX[i], IMY[i], ITM * 2, ITM * 2); //アイテムアイコン表示

                if (IMY[i] > pBase.Height)
                {
                    IMX[i] = rand.Next(1, 450);
                    IMY[i] = rand.Next(1, 300) - 400;
                }
            }

            //貫通アイテムの移動
            for (int i = 0; i < 2; i++)
            {
                KIY[i] += 2; //アイテム落下スペード
                gg.DrawImage(kantsuItem.Image, KIX[i], KIY[i], KIM * 2, KIM * 2); //アイテムアイコン表示

                if (KIY[i] > pBase.Height)
                {
                    KIX[i] = rand.Next(1, 450);
                    KIY[i] = rand.Next(1, 300) - 400;
                }
            }

            //自機の座標位置
            //Cpos = PointToClient(Cursor.Position);
            //Cpos = PointToClient(System.Windows.Forms.Cursor.Position);
            Cpos = this.PointToClient(Cursor.Position);


            /*
            string screen_x = Cursor.Position.X.ToString();
            string screen_y = Cursor.Position.Y.ToString();
           
            string client_x = client_point.X.ToString();
            string client_y = client_point.Y.ToString();
            Cpos = this.PointToScreen(System.Windows.Forms.Cursor.Position);
            Cpos.X = PointToClient(System.Windows.Forms.Cursor.Position).X;
            Cpos.Y = PointToClient(System.Windows.Forms.Cursor.Position).Y;
            */

            if (Cpos.X < 0)
            {
                Cpos.X = 0;
            }
            if (Cpos.Y < 0)
            {
                Cpos.Y = 0;
            }
            if (Cpos.X > ClientSize.Width - PW)
            {
                Cpos.X = ClientSize.Width - PW;
            }
            if (Cpos.Y > ClientSize.Height - PH)
            {
                Cpos.Y = ClientSize.Height - PH;
            }

            //自機を左右自由に移動させる
            gg.DrawImage(pPlayer.Image, Cpos.X, Cpos.Y, PW, PH);

            //スコア表示
            score++;
            gg.DrawString("SCORE: " + score.ToString(), myFont, Brushes.White, 10, 10);

            //ハイスコア更新中表示
            if (highscore < score)
            {
                highscore = score;
                gg.DrawString(score.ToString() + "★更新中", myFont, Brushes.White, 97, 10);
            }

            //貫通アイテム所持数表示
            if(kantsuCount > 0)
            {
                if(kantsuCount == 1)
                {
                    gg.DrawImage(strongBeamyoko.Image, 10,40, KIM * 2, KIM * 2);
                }
                else if (kantsuCount == 1 || kantsuCount == 2)
                {
                    gg.DrawImage(strongBeamyoko.Image, 10, 40, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 50, KIM * 2, KIM * 2);
                }
                else if (kantsuCount == 1 || kantsuCount == 2 || kantsuCount == 3)
                {
                    gg.DrawImage(strongBeamyoko.Image, 10, 40, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 50, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 60, KIM * 2, KIM * 2);
                }
                else if (kantsuCount == 1 || kantsuCount == 2 || kantsuCount == 3 || kantsuCount == 4)
                {
                    gg.DrawImage(strongBeamyoko.Image, 10, 40, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 50, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 60, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 70, KIM * 2, KIM * 2);
                }
                else if (kantsuCount == 1 || kantsuCount == 2 || kantsuCount == 3 || kantsuCount == 4 || kantsuCount == 5)
                {
                    gg.DrawImage(strongBeamyoko.Image, 10, 40, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 50, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 60, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 70, KIM * 2, KIM * 2);
                    gg.DrawImage(strongBeamyoko.Image, 10, 80, KIM * 2, KIM * 2);
                }
            }

            //スコアが10000超えたらCPU登場
            if (score > 100)
            {
                CPUmove();
                CPUAttack();
            }

            pBase.Image = canvas;

            hitCheck(); //当たり判定
            Beamove();
            kantsuBeamove();
        }

        ///////////////////////////////////////////
        /// ビームの初期座標位置
        ///////////////////////////////////////////
        private void initBeam()
        {
            weakBeam.Top = Cpos.Y;                         //ビーム発射Y座標
            weakBeam.Left = Cpos.X + (PW / 2) - (BMW / 2); //自機の真ん中を算出➡カーソル位置 + 自機の幅/2 - ビームの幅/2
        }

        ///////////////////////////////////////////
        /// 貫通ビームの初期座標位置
        ///////////////////////////////////////////
        private void initkantsuBeam()
        {
            strongBeam.Top = Cpos.Y; //ビーム発射Y座標
            strongBeam.Left = Cpos.X + (PW / 2) - (KBW / 2) + 3; //自機の真ん中を算出➡カーソル位置 + 自機の幅/2 - ビームの幅/2
        }

        ///////////////////////////////////////////
        /// ビームの移動
        ///////////////////////////////////////////
        private void Beamove()
        {
            weakBeam.Top -= 10; //ビーム速度
            gg.DrawImage(weakBeam.Image, new Rectangle(weakBeam.Left, weakBeam.Top, BMW, BMH));//Rectangle(int x, int y, int width, int height)
                                                                                               //左上隅が(x, y) として指定され、幅と高さが width 引数と height 引数によって指定される新しい Rectangle を構築します。
            if (weakBeam.Top + BMH < 0) //ビームが枠外に出たら、0より小さくなったら
            {
                weakBeamFlg = false; //ビーム発射フラグをfalseに
            }
        }

        ///////////////////////////////////////////
        /// 貫通ビームの移動
        ///////////////////////////////////////////
        private void kantsuBeamove()
        {
            strongBeam.Top -= 10; //ビーム速度
            gg.DrawImage(strongBeam.Image, new Rectangle(strongBeam.Left, strongBeam.Top, KBW, KBH));//Rectangle(int x, int y, int width, int height)
                                                                                                     //左上隅が(x, y) として指定され、幅と高さが width 引数と height 引数によって指定される新しい Rectangle を構築します。

            if (strongBeam.Top + KBH < 0) //ビームが枠外に出たら、0より小さくなったら
            {
                strongBeamFlg = false; //ビーム発射フラグをfalseに
            }
        }

        ///////////////////////////////////////////
        /// CPU左右移動
        ///////////////////////////////////////////
        private void CPUmove()
        {
            gg.DrawImage(pCPU.Image, CPUX, CPUY, CPU * 2, CPU * 2); //CPU登場位置
            CPUX += CPmove;  //CPUをCPmoveだけ動かす

            if (CPUX < 15)  //CPUの左端が、フォームの左端から-15に当たった時
            {
                CPmove = +2;  //進行方向を右へ+2にする
            }
            if (CPUX + 50 + CPUY > ClientSize.Width)  //CPUの右端が、フォームの右端に当たった時
            {
                CPmove = -2;  //進行方向を左へ-2にする
            }
        }

        ///////////////////////////////////////////
        /// CPUビーム初期位置設定
        ///////////////////////////////////////////
        private void initCPUBeam()
        {
            CPBX = CPUX;           //CPUビームのX座標=CPUのX座標
            CPBY = CPUY + CPU * 2; //CPUビームのY座標=CPUのY座標＋CPUの大きさ×２
        }
        
        ///////////////////////////////////////////
        /// CPUビーム攻撃
        ///////////////////////////////////////////
        private void CPUAttack()
        {
            CPBY += 5;
            gg.DrawImage(CPUBeam.Image, new Rectangle(CPBX, CPBY - 20, CPBW + 50, CPBH + 20)); //CPUビームの場所指定
            if (CPBY > ClientSize.Height)
            {
                initCPUBeam();
            }
        }

        ///////////////////////////////////////////
        /// 接触判定
        ///////////////////////////////////////////
        private void hitCheck()
        {
            int pcx = Cpos.X + PW - 5; //(PW /2) 自機の中心座標,マウスカーソル + 自機の幅 /2 
            int pcy = Cpos.Y + (PH / 2) + 10; //220 + (PH / 2);    //自機の中心座標,Y座標220 + 自機の高さ /2
            int pcix = Cpos.X + (PW / 2) + 10; // アイテム用
            int pczx = Cpos.Y + (PH / 2) + 10; // アイテム用
            int ecx, ecy, dis; //自機と隕石の距離計算用
            int cpx, cpy, cps; //自機とCPUの距離計算用
            int cbx, cby, cbs; //自機とCPUビームの距離計算用
            int imx, imy, toz; //自機とアイテムの距離計算用
            int kix, kiy, kiz; //自機と貫通アイテムの距離計算用
            int bmx, bmy, bmz; //ビームと隕石の距離計算用
            int kmx, kmy, kmz; //貫通ビームと隕石の距離計算用

            //自機と隕石接触
            for (int i = 0; i < 10; i++)
            {
                ecx = enX[i] + RR[i]; //隕石X座標 + 隕石の大きさ
                ecy = enY[i] + RR[i]; //隕石Y座標 + 隕石の大きさ
                dis = (ecx - pcx) * (ecx - pcx) + (ecy - pcy) * (ecy - pcy); //隕石と自機の距離を算出
                if (dis < RR[i] * RR[i])
                {
                    hitFlg = true; //true:自機と隕石が当たった
                    break; //forから抜ける
                }
            }

            //自機とCPUの接触
            if (score > 100)
            {
                cpx = CPUX + CPU; //CPUのX座標 + CPU大きさ
                cpy = CPUY + CPU; //CPUのY座標 + CPU大きさ
                cbx = CPBX + 10 + CPUB + 10; //CPUビームX座標 + CPUビーム大きさ
                cby = CPBY - 30 + CPUB; //CPUビームY座標 + CPUビーム大きさ
                cps = (cpx - pcx) * (cpx - pcx) + (cpy - pcy) * (cpy - pcy); //CPUと自機の距離を算出
                cbs = (cbx - pcx) * (cbx - pcx) + (cby - pcy) * (cby - pcy); //CPUビームと自機の距離を算出

                if (cps < CPU * CPU || cbs < CPUB * CPUB)
                {
                    hitFlg = true; //true:自機とCPUが当たった
                }
            }



            //自機とアイテム接触
            for (int i = 0; i < 5; i++)
            {
                imx = IMX[i] + ITM;//アイテムX座標 + アイテム半径
                imy = IMY[i] + ITM;//アイテムY座標 + アイテム半径
                toz = (imx - pcix) * (imx - pcix) + (imy - pczx) * (imy - pczx); //自機とアイテムの距離を算出
                if (toz < ITM * ITM)
                {
                    getFlg[i] = true; //true：アイテムに当たった   
                }
            }

            //フラグを使わなくてもアイテム獲得演出は表現できる↓2行
            //IMY[i] += 10000; //アイテムのY座標を規格外数値にして枠外に出して初期化する(アイテム取得風に見せれる)
            //score += 5000; //スコア加算

            //自機と貫通アイテム接触
            for (int i = 0; i < 5; i++)
            {
                kix = KIX[i] + KIM;
                kiy = KIY[i] + KIM;
                kiz = (kix - pcix) * (kix - pcix) + (kiy - pczx) * (kiy - pczx); //自機と貫通アイテムの距離を算出
                if (kiz < KIM * KIM)
                {
                    kantsuItemFlg[i] = true; //true：アイテムに当たった
                    if(kantsuCount < 5)      //貫通カウントが5以下の場合(貫通アイテムは5個以下に設定される)
                    {
                        kantsuCountFlg = true;//貫通カウントフラグをtrueにする
                    }
                }
            }

            //ビームと隕石接触
            if (weakBeamFlg)
            {
                for (int i = 0; i < 10; i++)
                {
                    bmx = enX[i] + RR[i];     //隕石X座標 + 隕石の大きさ
                    bmy = enY[i] + RR[i];     //隕石Y座標 + 隕石の大きさ
                    bmz = (bmx - weakBeam.Left) * (bmx - weakBeam.Left) + (bmy - weakBeam.Top) * (bmy - weakBeam.Top); //(隕石X-ビーム幅)×(隕石X-ビーム幅) + (隕石Y-ビーム高さ) × (隕石Y-ビーム高さ)
                    if (bmz < RR[i] * RR[i])  //隕石とビームの距離が隕石の大きさより小さい場合(当たったとゆう解釈)
                    {
                        enY[i] += 1000;       //隕石は画面から消える
                        weakBeam.Top -= 1000; //ビームが消える
                        score += 50;
                    }
                }
            }

            //貫通ビームと隕石接触
            if (strongBeamFlg)
            {
                for (int i = 0; i < 10; i++)
                {
                    kmx = enX[i] + RR[i]; //隕石X座標 + 隕石の大きさ
                    kmy = enY[i] + RR[i]; //隕石Y座標 + 隕石の大きさ
                    kmz = (kmx - strongBeam.Left) * (kmx - strongBeam.Left) + (kmy - strongBeam.Top) * (kmy - strongBeam.Top); //(隕石X-ビーム幅)×(隕石X-ビーム幅) + (隕石Y-ビーム高さ) × (隕石Y-ビーム高さ)
                    if (kmz < RR[i] * RR[i]) //隕石とビームの距離が隕石の大きさより小さい場合(当たったとゆう解釈)
                    {
                        enY[i] += 1000;//隕石は画面から消える
                        score += 50;
                    }
                }
            }
        }
    }
}
