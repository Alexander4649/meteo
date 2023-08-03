using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        int PW, PH; //自機の幅と高さを変数に
        Point Cpos; //カーソル座標
        int[] enX = new int[10]; //隕石の座標
        int[] enY = new int[10];
        int[] RR = new int[10]; //隕石大きさ
        int[] IMX = new int[5]; //アイテムの座標
        int[] IMY = new int[5];
        int ITM; //アイテムの半径
        Random  rand = new Random();
        Boolean hitFlg; // true:当たった
        Boolean getFlg; //true:当たった➡ゲットした
        int ecnt; //爆発演出用
        long msgcnt; //メッセージ用カウンタ
        Boolean titleFlg; //true:タイトル表示中
        long score; //スコア
        long highscore = 0; //ハイスコア,初回のみ0に
        Font myFont = new Font("Arial", 16);

        public Form1()
        {
            InitializeComponent();
            Size = Size - ClientSize + new Size(480, 320);
            //現在のフォームのサイズ = (現在のフォームのサイズ - 周囲の余白を除いたサイズ) + 設定したい画面サイズ(ここでは480x320)
            //(現在のフォームのサイズ - 周囲の余白を除いたサイズ)を計算することで周囲の余白のサイズを計算しています。また、その値に設定したい画面サイズ(480x320)を足してあげることで余白を考慮した画面サイズを設定しています。
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //隠し
            pMeteor.Hide();
            pItem.Hide();
            pPlayer.Hide();
            pBG.Hide();
            pEXP.Hide();
            pGameover.Hide();
            pMsg.Hide();
            pTitle.Hide();

            pBase.Width = ClientSize.Width; //pBaseをフォームのクライアント領域と同じに　幅
            pBase.Height = ClientSize.Height; //pBaseをフォームのクライアント領域と同じに　高さ

            initGame(); //初期処理
        }

        private void initGame()
        {
            PW = 41; //自機の幅
            PH = 51; //自機の高さ
            ITM = 70/2;

            //隕石大きさ・量と落ちてくる場所の指定
            for (int i = 0; i < 10; i++)
            {
                enX[i] = rand.Next(1, 450); //隕石の初期配置座標
                enY[i] = rand.Next(1, 900) - 1000; //落下の間隔にバラつきを持たせる
                RR[i] = rand.Next(10, 50); //隕石の大きさをランダムに
            }

            //アイテムの落下場所指定
            for(int i = 0; i < 5;i++)
            {
                IMX[i] = rand.Next(1, 450);
                IMY[i] = rand.Next(1, 900) - 1000;
            }

            hitFlg = false; //false:当たっていない
            getFlg = false; //false:ゲットしてない
            ecnt = 0;
            msgcnt = 0;
            titleFlg = true; //true:タイトル表示中
            score = 0;
        }

        //爆発演出
        private void playerExplosion()
        {
            //爆発演出の描画は、全てここで行う
            gg.DrawImage(pBG.Image, 0, 0, 480, 320);

            //爆発後隕石をその場所に残す
            for (int i = 0;i < 10;i++)
            {
                gg.DrawImage(pMeteor.Image, enX[i], enY[i], RR[i] * 2, RR[i] * 2);
            }

            //爆発後アイテムをその場所に残す
            for (int i = 0;i < 5; i++)
            {
                gg.DrawImage(pItem.Image, IMX[i], IMY[i]);
            }

            if(ecnt < 16) //16フレームのアニメにする,カウンタと表示する画像の位置を決める役目
            {
                GraphicsUnit units = GraphicsUnit.Pixel;
                //画像の一部をずらしながら表示する
                gg.DrawImage(pEXP.Image, Cpos.X + PW / 2 - 50, 220 + PH / 2 - 50, 
                    new Rectangle(ecnt / 2 * 100, 0, 100, 100), units); //0,100,200~600,700と画像の位置をずらす。
                                                                        //new Rectangle( x, y, 幅, 高さ ) で表示する位置と、切り出すサイズを指定
                ecnt++;
            }
            msgcnt++;
            if(msgcnt > 60)
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

            pBase.Image = canvas;
        }

        //アイテム獲得演出(アイテムを消す)
        private void playerGetitem()
        {
            gg.DrawImage(pBG.Image, 0, 0, 480, 320);
            pBase.Image = canvas;
        }

        //タイトル表示
        private void dispTitle()
        {
            msgcnt++;
            //タイトル表示中の描画は全てここで行う
            gg.DrawImage(pBG.Image, 0, 0, 480, 320);
            gg.DrawImage(pTitle.Image, 70, 80, 350, 60);
            if(msgcnt % 60 > 20)
            {
                gg.DrawImage(pMsg.Image, 110, 190, 271, 26);
            }

            pBase.Image = canvas;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (titleFlg)
            {
                dispTitle();
                return; //タイトル表示中はこの先の処理をしない
            }
            if(hitFlg)
            {
                playerExplosion();
                return; //自機と隕石が当たった後、この先の処理をしない
            }
            if (getFlg)
            {
                playerGetitem();
            }

            gg.DrawImage(pBG.Image, 0, 0, 480, 320);

            //隕石の移動
            for(int i = 0;i < 10;i++)
            {
                if ((RR[i]) > 20)
                {
                    enY[i] += 3; //下へ移動、大きいと早くなる
                }else if ((RR[i]) < 20)
                {
                    enY[i] += 7;
                }
                gg.DrawImage(pMeteor.Image, enX[i], enY[i], RR[i] * 2, RR[i] * 2); //描画
                if (enY[i] > pBase.Height) //pBase.Height よりも大きいときは、隕石が画面外へ出たことになるので画面の上に戻す処理
                {
                    enX[i] = rand.Next(1, 450);
                    enY[i] = rand.Next(1, 300) - 400;
                }
            }

            //アイテムの移動
            for (int i = 0; i < 5; i++)
            {
                IMY[i] += 1; //アイテム落下スペード
                gg.DrawImage(pItem.Image, IMX[i], IMY[i]); //アイテムアイコン表示

                if (IMY[i] > pBase.Height)
                {
                    IMX[i] = rand.Next(1, 450);
                    IMY[i] = rand.Next(1, 300) - 400;
                }
            }

            Cpos = PointToClient(Cursor.Position);

            if(Cpos.X < 0)
            {
                Cpos.X = 0;
            }
            if(Cpos.X > ClientSize.Width - PW)
            {
                Cpos.X = ClientSize.Width - PW;
            }

            gg.DrawImage(pPlayer.Image, Cpos.X, 220, PW, PH); //自機の位置表示　自機の絵, カーソルx座標

            //スコア表示
            score++;
            gg.DrawString("SCORE: " + score.ToString(), myFont, Brushes.White, 10, 10);

            //ハイスコア更新中表示
            if (highscore < score)
            {
                highscore = score;
                gg.DrawString(score.ToString()+"★更新中", myFont, Brushes.White, 97, 10);
            }

            pBase.Image = canvas;
            hitCheck(); //当たり判定
        }

        //背景をクリックした時
        private void pBase_Click(object sender, EventArgs e)
        {
            if (titleFlg)
            {
                if(msgcnt > 20)
                {
                    msgcnt = 0;
                    titleFlg = false; //タイトル非表示
                }
                return; //タイトル表示中はこの先の処理をしない
            }
            if(msgcnt > 80)
            {
                initGame();
            }
        }

        //自機と隕石の当たり判定
        private void hitCheck()
        {
            int pcx = Cpos.X + (PW / 2); //自機の中心座標
            int pcy = 220 + (PH / 2);    //自機の中心座標
            int ecx, ecy, dis; //自機と隕石の距離計算用
            int imx, imy, toz; //自機とアイテムの距離計算用
            for (int i = 0; i < 10; i++)
            {
                ecx = enX[i] + RR[i];
                ecy = enY[i] + RR[i];
                dis = (ecx - pcx) * (ecx - pcx) + (ecy - pcy) * (ecy - pcy); //隕石と自機の距離を算出
                if(dis < RR[i] * RR[i])
                {
                    hitFlg = true; //true:自機と隕石が当たった
                    break; //forから抜ける
                }
            }

            
            for(int i = 0; i < 5; i++)
            {
                imx = IMX[i] + ITM;
                imy = IMY[i] + ITM;
                toz = (imx - pcx) * (imx - pcx) + (imy - pcy) * (imy - pcy); //自機とアイテムの距離を算出
                if(toz < ITM * ITM)
                {
                    getFlg = true; //true：アイテムに当たった
                    break;
                }
            }
        }
    }
}
