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
        Point Bpos; //ビームのカーソル座標
        int[] enX = new int[10]; //隕石の座標
        int[] enY = new int[10];
        int[] RR = new int[10]; //隕石大きさ
        int[] IMX = new int[5]; //アイテムの座標
        int[] IMY = new int[5];
        int ITM; //アイテムの半径
        int[] BMX = new int[10]; //ビームの座標
        int[] BMY = new int[10];
        int BMW ,BMH; //ビームの幅と高さ
        Random  rand = new Random();
        Boolean hitFlg; // true:当たった
        Boolean[] getFlg; //true:当たった➡ゲットした
        Boolean weakBeamFlg = false; //クリックしたらビーム発射なので最初はfalse状態にする
        //Boolean[] MteorBreakFlg; //true:隕石とビームが当たった
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
            weakBeam.Hide();
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
            ITM = 40/2;//アイテムの大きさ
            BMW = 20; //ビームの幅を20
            BMH = 20; //ビームの高さを10

            //隕石大きさ・量と落ちてくる場所の指定
            for (int i = 0; i < 10; i++)
            {
                enX[i] = rand.Next(1, 450); //隕石の初期配置座標
                enY[i] = rand.Next(1, 900) - 1000; //落下の間隔にバラつきを持たせる
                RR[i] = rand.Next(20, 50); //隕石の大きさをランダムに
            }

            //アイテムの落下場所指定
            for(int i = 0; i < 5;i++)
            {
                IMX[i] = rand.Next(1, 450);
                IMY[i] = rand.Next(1, 900) - 1000;
            }

            hitFlg = false; //false:当たっていない
            getFlg = new bool[] { false, false, false, false, false }; //false:ゲットしてない
            //MteorBreakFlg = new bool[] { false, false, false, false, false };
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
                gg.DrawImage(pItem.Image, IMX[i], IMY[i], ITM * 2, ITM * 2);
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
        private void playerGetitem(int i)
        {
            //ヒットしたアイテムのフラグを落とす
            getFlg[i] = false;
            // ポイント加算
            score += 10000;
            // ヒットしたアイテムの座標を初期化
            IMX[i] = rand.Next(1, 450);
            IMY[i] = rand.Next(1, 900) - 1000;
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
            for (int i = 0; i < getFlg.Length; i++) //getFlgの配列分ループ処理する。今回で言うと5個分
            {
                if (getFlg[i])
                {
                    playerGetitem(i);
                }
            }

            /*
            for (int i = 0; i < MteorBreakFlg.Length; i++)
            {
                if (MteorBreakFlg) //ビームが隕石に当たったら➡ビームフラグがたったら
                {
                    enY[i] += 10000;//隕石は画面から消える
                    MteorBreakFlg = false; //ビームフラグをfalseにして初期化
                    MteorBreak(i)
                }
            }
                //break; //ビームが画面外へ出たらforから抜ける
            */

            gg.DrawImage(pBG.Image, 0, 0, 480, 320);

            //隕石の移動
            for(int i = 0;i < 10;i++)
            {
                if ((RR[i]) > 30) //隕石の大きさが30以上の場合
                {
                    enY[i] += 2; //下へ移動、大きくて遅い状態
                }else if ((RR[i]) < 30) //隕石の大きさが30以下の場合
                {
                    enY[i] += 4; //小さくて早い状態
                }
                gg.DrawImage(pMeteor.Image, enX[i], enY[i], RR[i] * 2, RR[i] * 2); //描画 RRは半径なので2倍にする
                if (enY[i] > pBase.Height) //pBase.Height よりも大きいときは、隕石が画面外へ出たことになるので画面の上に戻す処理
                {
                    enX[i] = rand.Next(1, 450);
                    enY[i] = rand.Next(1, 300) - 400;
                }
            }
            
            //アイテムの移動
            for (int i = 0; i < 2; i++)
            {
                IMY[i] += 2; //アイテム落下スペード
                gg.DrawImage(pItem.Image, IMX[i], IMY[i], ITM*2, ITM* 2); //アイテムアイコン表示

                if (IMY[i] > pBase.Height)
                {
                    IMX[i] = rand.Next(1, 450);
                    IMY[i] = rand.Next(1, 300) - 400;
                }
            }

            //ビームの座標位置 == プレイヤーの位置と同じ ※マウスクリックした時のみ表示させる
            //Bpos = PointToClient(Cursor.Position); //ビームの座標位置をカーソルに合わせる
            Bpos = PointToClient(System.Windows.Forms.Cursor.Position);

            if (Bpos.X < 0)
            {
                Bpos.X = 0;
            }
            if (Bpos.Y < 0)
            {
                Bpos.Y = 0;
            }
            if (Bpos.X > ClientSize.Width - BMW)
            {
                Bpos.X = ClientSize.Width - BMW;
            }
            if (Bpos.Y > ClientSize.Width - BMH)
            {
                Bpos.Y = ClientSize.Width - BMH;
            }

            //自機の座標位置
            //Cpos = PointToClient(Cursor.Position);
            Cpos = PointToClient(System.Windows.Forms.Cursor.Position);
            //mx = PointToClient(System.Windows.Forms.Cursor.Position).X;
            //my = PointToClient(System.Windows.Forms.Cursor.Position).Y;

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
            if (Cpos.Y > ClientSize.Width - PH)
            {
                Cpos.Y = ClientSize.Width - PH;
            }

            gg.DrawImage(pPlayer.Image, Cpos.X, 220, PW, PH); //自機の位置表示　自機の絵, カーソルx座標、Y座標220に固定
            //gg.DrawImage(pPlayer.Image, Cpos.X, Cpos.Y, PW, PH); //自機を左右自動に移動させる
            

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
            Beamove();
        }

        //ビームの初期値
        private void initBeam()
        {
            weakBeam.Top = 215; //ビーム発射Y座標
            weakBeam.Left = Bpos.X + (PW / 2) - (BMW / 2); //自機の真ん中を算出➡カーソル位置 + 自機の幅/2 - ビームの幅/2
        }

        //ビームの移動(突破ゲームの応用)
        private void Beamove()
        {
            if (weakBeamFlg)
            {
                weakBeam.Top -= 10;
                gg.DrawImage(weakBeam.Image, new Rectangle (weakBeam.Left, weakBeam.Top, BMW, BMH));

                if (weakBeam.Top + BMH < 0) //枠外に出たら
                {
                    weakBeamFlg = false; //ビーム発射フラグをfalseに
                    initBeam(); //ビームを初期値に戻す
                }
            }
        }

        //マウスクリック時にビーム発射フラグを立てる
        private void pBase_MouseClick(object sender, MouseEventArgs e)
        {
            weakBeamFlg = true; //ビーム発射フラグを立てる
        }

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
            int pcx = Cpos.X + (PW / 2); //自機の中心座標,マウスカーソル + 自機の幅 /2
            int pcy = 220 + (PH / 2);    //自機の中心座標,Y座標220 + 自機の高さ /2
            int ecx, ecy, dis; //自機と隕石の距離計算用
            int imx, imy, toz; //自機とアイテムの距離計算用
            int bmx, bmy, bmz; //ビームと隕石の距離計算用

            //自機と隕石接触
            for (int i = 0; i < 10; i++)
            {
                ecx = enX[i] + RR[i]; //隕石X座標 + 隕石の大きさ
                ecy = enY[i] + RR[i]; //隕石Y座標 + 隕石の大きさ
                dis = (ecx - pcx) * (ecx - pcx) + (ecy - pcy) * (ecy - pcy); //隕石と自機の距離を算出
                if(dis < RR[i] * RR[i])
                {
                    hitFlg = true; //true:自機と隕石が当たった
                    break; //forから抜ける
                }
            }

            //自機とアイテム接触
            for (int i = 0; i < 5; i++)
            {
                imx = IMX[i] + ITM;
                imy = IMY[i] + ITM;
                toz = (imx - pcx) * (imx - pcx) + (imy - pcy) * (imy - pcy); //自機とアイテムの距離を算出
                if(toz < ITM * ITM)
                {
                    getFlg[i] = true; //true：アイテムに当たった
                                      //フラグを使わなくてもアイテム獲得演出は表現できる↓2行
                                      //IMY[i] += 10000; //アイテムのY座標を規格外数値にして枠外に出して初期化する(アイテム取得風に見せれる)
                                      //score += 1000; //スコア加算
                }
            }
            
            //ビームと隕石接触
            for (int i = 0; i < 10; i++)
            {
                bmx = enX[i] + RR[i]; //隕石X座標 + 隕石の大きさ
                bmy = enY[i] + RR[i]; //隕石Y座標 + 隕石の大きさ
                bmz = (bmx - weakBeam.Left) * (bmx - weakBeam.Left) + (bmy - weakBeam.Top) * (bmy - weakBeam.Top); //(隕石X-ビーム幅)×(隕石X-ビーム幅) + (隕石Y-ビーム高さ) × (隕石Y-ビーム高さ)
                if (bmz < RR[i] * RR[i]) //隕石とビームの距離が隕石の大きさより小さい場合(当たったとゆう解釈)
                {
                    //貫通ビーム(貫通アイテム所持している時の判定文
                    //if (getFlg[i] == true)
                    //{
                        //enY[i] += 1000;//隕石は画面から消える
                    //}
                    //単発ビーム
                    
                    //else
                    //{
                        enY[i] += 1000;//隕石は画面から消える
                        weakBeam.Top = -1000; //ビームが消える
                    //}
                    
                }
            }
        }
    }
}
