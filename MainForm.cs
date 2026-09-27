using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace GoiConTimLamQua
{
    public class MainForm : Form
    {
        private System.Windows.Forms.Timer animTimer;
        private System.Windows.Media.MediaPlayer mediaPlayer;
        private bool isAudioLoaded = false;
        private bool isPlaying = true;
        private double manualTime = 0.0;
        private DateTime lastTickTime;

        // Customization
        private string crushName = "Huyền";
        private string customBoySpeech = "Hiếu thích Huyền";
        private string customGirlSpeech = "ừ, Huyền đồng ý";
        private bool isKaraokeEnabled = true;

        // Code Cache
        private List<CodeLine> cachedCodeLines;

        // Animation Particles
        private List<Star> stars;
        private List<FloatingHeart> hearts;
        private Random rand = new Random();

        // Marquee Offset
        private float marqueeX = 0f;
        private string marqueeText = "Gói Con Tim Làm Quà • Hiếu ❤️ Huyền • Phạm Minh Hiếu gửi tặng Huyền • karaoke mode • ";

        // Dragging Seekbar
        private bool isScrubbing = false;

        // Fonts
        private Font codeFont;
        private Font bubbleFont;
        private Font statusFont;
        private Font marqueeFont;
        private Font tiktokFont;

        public MainForm()
        {
            this.Text = "Gói Con Tim Làm Quà - Hiếu Tặng Huyền ❤️ (C# Edition)";
            this.ClientSize = new Size(460, 840);
            this.MinimumSize = new Size(380, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(24, 26, 32);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            InitFonts();
            InitParticles();
            cachedCodeLines = CodeRepository.GetScriptLines(crushName);

            InitAudio();

            animTimer = new System.Windows.Forms.Timer();
            animTimer.Interval = 16; // ~60 FPS
            animTimer.Tick += AnimTimer_Tick;
            lastTickTime = DateTime.Now;
            animTimer.Start();

            this.KeyDown += MainForm_KeyDown;
            this.MouseDown += MainForm_MouseDown;
            this.MouseMove += MainForm_MouseMove;
            this.MouseUp += MainForm_MouseUp;
        }

        private void InitFonts()
        {
            codeFont = new Font("Consolas", 10.5f, FontStyle.Regular);
            bubbleFont = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            statusFont = new Font("Consolas", 8.5f, FontStyle.Regular);
            marqueeFont = new Font("Segoe UI", 9.0f, FontStyle.Regular);
            tiktokFont = new Font("Segoe UI", 8.0f, FontStyle.Regular);
        }

        private void InitParticles()
        {
            stars = new List<Star>();
            for (int i = 0; i < 45; i++)
            {
                stars.Add(new Star
                {
                    XRatio = (float)rand.NextDouble(),
                    YRatio = (float)(rand.NextDouble() * 0.85),
                    Phase = (float)(rand.NextDouble() * Math.PI * 2),
                    Size = rand.Next(1, 4),
                    Speed = (float)(1.5 + rand.NextDouble() * 3.0)
                });
            }
            hearts = new List<FloatingHeart>();
        }

        private void InitAudio()
        {
            try
            {
                mediaPlayer = new System.Windows.Media.MediaPlayer();
                string[] audioCandidates = new string[] { "Music.mp3", "audio.m4a", "reference.mp4", "audio.mp3" };
                string chosen = null;

                foreach (string candidate in audioCandidates)
                {
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, candidate);
                    if (File.Exists(fullPath))
                    {
                        chosen = fullPath;
                        break;
                    }
                }

                if (chosen != null)
                {
                    mediaPlayer.Open(new Uri(chosen));
                    mediaPlayer.MediaEnded += delegate {
                        mediaPlayer.Position = TimeSpan.Zero;
                        mediaPlayer.Play();
                    };
                    mediaPlayer.Play();
                    isAudioLoaded = true;
                }
            }
            catch
            {
                isAudioLoaded = false;
            }
        }

        private double GetCurrentSeconds()
        {
            if (isAudioLoaded && mediaPlayer != null)
            {
                try
                {
                    double sec = mediaPlayer.Position.TotalSeconds;
                    if (sec > TimelineController.TotalDuration)
                    {
                        mediaPlayer.Position = TimeSpan.Zero;
                        return 0.0;
                    }
                    return sec;
                }
                catch
                {
                    return manualTime;
                }
            }
            return manualTime;
        }

        private void SeekTo(double sec)
        {
            if (sec < 0) sec = 0;
            if (sec > TimelineController.TotalDuration) sec = TimelineController.TotalDuration;

            manualTime = sec;
            if (isAudioLoaded && mediaPlayer != null)
            {
                try
                {
                    mediaPlayer.Position = TimeSpan.FromSeconds(sec);
                }
                catch { }
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            double delta = (now - lastTickTime).TotalSeconds;
            lastTickTime = now;

            if (isPlaying && (!isAudioLoaded || mediaPlayer == null))
            {
                manualTime += delta;
                if (manualTime > TimelineController.TotalDuration)
                {
                    manualTime = 0.0;
                }
            }

            // Marquee scroll
            marqueeX -= (float)(delta * 40.0);
            if (marqueeX < -500f) marqueeX = 0f;

            // Update floating hearts
            double currentSec = GetCurrentSeconds();
            TimelineState state = TimelineController.Evaluate(currentSec, customBoySpeech, customGirlSpeech);

            if (state.ShowHearts && rand.Next(100) < (state.BigHeartShower ? 35 : 12))
            {
                float midX = this.ClientSize.Width * 0.5f + (float)((rand.NextDouble() - 0.5) * 80.0);
                float bottomY = this.ClientSize.Height * 0.35f;
                hearts.Add(new FloatingHeart
                {
                    X = midX,
                    Y = bottomY,
                    SpeedY = (float)(40.0 + rand.NextDouble() * 50.0),
                    Drift = (float)(rand.NextDouble() * Math.PI * 2),
                    Scale = (float)(0.8 + rand.NextDouble() * 0.8),
                    Life = 1.0f
                });
            }

            for (int i = hearts.Count - 1; i >= 0; i--)
            {
                FloatingHeart h = hearts[i];
                h.Y -= (float)(h.SpeedY * delta);
                h.Drift += (float)(delta * 3.0);
                h.X += (float)(Math.Sin(h.Drift) * 20.0 * delta);
                h.Life -= (float)(delta * 0.55);
                if (h.Life <= 0f || h.Y < 20f)
                {
                    hearts.RemoveAt(i);
                }
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            double sec = GetCurrentSeconds();
            TimelineState state = TimelineController.Evaluate(sec, customBoySpeech, customGirlSpeech);

            // Layout split:
            // Top Scene: 0 to sceneH
            // Marquee bar: sceneH to sceneH + marqueeH
            // Code Panel: sceneH + marqueeH to h - statusH
            // Status bar: h - statusH to h
            int sceneH = (int)(h * 0.40);
            int marqueeH = 28;
            int statusH = 34;
            int codePanelTop = sceneH + marqueeH;
            int codePanelH = h - codePanelTop - statusH;

            // 1. Draw Top Scene
            DrawTopScene(g, 0, 0, w, sceneH, sec, state);

            // 2. Draw Marquee Divider
            DrawMarqueeBar(g, 0, sceneH, w, marqueeH, state);

            // 3. Draw Code IDE Panel
            DrawCodePanel(g, 0, codePanelTop, w, codePanelH, state);

            // 4. Draw Bottom Status & Controls
            DrawStatusBar(g, 0, h - statusH, w, statusH, sec, state);
        }

        private void DrawTopScene(Graphics g, int x, int y, int w, int h, double sec, TimelineState state)
        {
            // Night Sky Gradient
            using (LinearGradientBrush skyBrush = new LinearGradientBrush(
                new Point(0, y), new Point(0, y + h),
                PixelArt.ColorSkyTop, PixelArt.ColorSkyBottom))
            {
                g.FillRectangle(skyBrush, x, y, w, h);
            }

            // Twinkling Stars
            for (int i = 0; i < stars.Count; i++)
            {
                Star st = stars[i];
                float sx = x + st.XRatio * w;
                float sy = y + st.YRatio * (h - 30);
                float alpha = (float)(0.35 + 0.65 * (Math.Sin(sec * st.Speed + st.Phase) * 0.5 + 0.5));
                int aVal = (int)(alpha * 255);
                if (aVal < 40) aVal = 40;
                if (aVal > 255) aVal = 255;

                using (SolidBrush starBrush = new SolidBrush(System.Drawing.Color.FromArgb(aVal, 255, 255, 255)))
                {
                    g.FillRectangle(starBrush, sx, sy, st.Size, st.Size);
                }
            }

            // Pixel Moon (top-right)
            using (Bitmap moon = PixelArt.GetMoonSprite(2))
            {
                int moonX = w - moon.Width - 30;
                int moonY = y + 20;

                // Subtle halo glow
                using (SolidBrush halo = new SolidBrush(System.Drawing.Color.FromArgb(20, 255, 255, 255)))
                {
                    g.FillEllipse(halo, moonX - 10, moonY - 10, moon.Width + 20, moon.Height + 20);
                }
                g.DrawImage(moon, moonX, moonY);
            }

            // Ground & Grass
            int groundY = y + h - 22;
            using (SolidBrush groundBrush = new SolidBrush(PixelArt.ColorGround))
            {
                g.FillRectangle(groundBrush, x, groundY, w, 22);
            }
            using (Pen grassPen = new Pen(PixelArt.ColorGrass, 2f))
            {
                g.DrawLine(grassPen, x, groundY, x + w, groundY);
                // Little pixel grass tufts
                for (int gx = 20; gx < w; gx += 45)
                {
                    g.DrawLine(grassPen, gx, groundY, gx - 2, groundY - 5);
                    g.DrawLine(grassPen, gx + 2, groundY, gx + 4, groundY - 6);
                }
            }

            // Characters position calculations
            float boyStartX = w * 0.28f;
            float girlStartX = w * 0.72f;
            float meetX = w * 0.50f;

            float boyX = boyStartX + (meetX - 25f - boyStartX) * state.WalkProgress;
            float girlX = girlStartX + (meetX + 25f - girlStartX) * state.WalkProgress;
            float charY = groundY - 48;

            int pixelScale = 3;
            bool step = ((int)(sec * 6) % 2) == 0;

            if (state.BoyAction == BoyPose.Hugging)
            {
                // Draw Hugging Sprite
                using (Bitmap hugBmp = PixelArt.GetHuggingSprite(pixelScale, (float)Math.Sin(sec * 4.0)))
                {
                    float hugX = meetX - hugBmp.Width * 0.5f;
                    g.DrawImage(hugBmp, hugX, charY + 4);

                    // If Boy is speaking during hug
                    if (!string.IsNullOrEmpty(state.BoySpeech))
                    {
                        DrawSpeechBubble(g, state.BoySpeech, hugX + 20, charY - 25, true);
                    }
                    // If Girl is speaking during hug
                    if (!string.IsNullOrEmpty(state.GirlSpeech))
                    {
                        DrawSpeechBubble(g, state.GirlSpeech, hugX + hugBmp.Width - 20, charY - 25, false);
                    }
                }
            }
            else
            {
                // Boy Sprite
                using (Bitmap boyBmp = PixelArt.GetBoySprite(pixelScale, state.BoyAction == BoyPose.Walking ? step : false))
                {
                    g.DrawImage(boyBmp, boyX - boyBmp.Width * 0.5f, charY);

                    // Holding Heart
                    if (state.BoyAction == BoyPose.HoldingHeart)
                    {
                        bool shiny = ((int)(sec * 8) % 2) == 0;
                        using (Bitmap heartBmp = PixelArt.GetHeartSprite(2, shiny))
                        {
                            g.DrawImage(heartBmp, boyX + 18, charY - 10);
                        }
                    }

                    // Boy Speech Bubble
                    if (!string.IsNullOrEmpty(state.BoySpeech))
                    {
                        DrawSpeechBubble(g, state.BoySpeech, boyX, charY - 22, true);
                    }
                }

                // Girl Sprite
                using (Bitmap girlBmp = PixelArt.GetGirlSprite(pixelScale, state.GirlAction == GirlPose.Walking ? step : false))
                {
                    g.DrawImage(girlBmp, girlX - girlBmp.Width * 0.5f, charY);

                    // Holding Flower
                    if (state.GirlAction == GirlPose.HoldingFlower || state.GirlAction == GirlPose.FlowerWithering)
                    {
                        bool withered = (state.GirlAction == GirlPose.FlowerWithering);
                        using (Bitmap flowerBmp = PixelArt.GetFlowerSprite(2, withered))
                        {
                            g.DrawImage(flowerBmp, girlX - 22, charY - 10);
                        }
                    }

                    // Girl Speech Bubble
                    if (!string.IsNullOrEmpty(state.GirlSpeech))
                    {
                        DrawSpeechBubble(g, state.GirlSpeech, girlX, charY - 22, false);
                    }
                }
            }

            // Draw Floating Hearts
            for (int i = 0; i < hearts.Count; i++)
            {
                FloatingHeart fh = hearts[i];
                int hAlpha = (int)(fh.Life * 255);
                if (hAlpha > 255) hAlpha = 255;
                if (hAlpha < 10) continue;

                using (Bitmap hb = PixelArt.GetHeartSprite(Math.Max(1, (int)(2 * fh.Scale)), false))
                {
                    g.DrawImage(hb, fh.X, fh.Y);
                }
            }

            // TikTok Comment Card Sticker (Top-Left corner)
            DrawTikTokCommentCard(g, 16, y + 14);
        }

        private void DrawSpeechBubble(Graphics g, string text, float targetX, float targetY, bool pointLeft)
        {
            SizeF sz = g.MeasureString(text, bubbleFont);
            float padX = 10f;
            float padY = 5f;
            float bubbleW = sz.Width + padX * 2;
            float bubbleH = sz.Height + padY * 2;

            float bx = targetX - bubbleW * 0.5f;
            float by = targetY - bubbleH;

            if (bx < 10) bx = 10;
            if (bx + bubbleW > this.ClientSize.Width - 10) bx = this.ClientSize.Width - 10 - bubbleW;

            // Draw bubble rounded box
            using (GraphicsPath path = new GraphicsPath())
            {
                float r = 8f;
                path.AddArc(bx, by, r * 2, r * 2, 180, 90);
                path.AddArc(bx + bubbleW - r * 2, by, r * 2, r * 2, 270, 90);
                path.AddArc(bx + bubbleW - r * 2, by + bubbleH - r * 2, r * 2, r * 2, 0, 90);
                path.AddArc(bx, by + bubbleH - r * 2, r * 2, r * 2, 90, 90);
                path.CloseFigure();

                using (SolidBrush fillBrush = new SolidBrush(System.Drawing.Color.FromArgb(245, 255, 255, 255)))
                {
                    g.FillPath(fillBrush, path);
                }
                using (Pen borderPen = new Pen(System.Drawing.Color.FromArgb(180, 200, 210), 1f))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            // Tail pointing to speaker
            PointF[] tail = new PointF[]
            {
                new PointF(targetX - 4, by + bubbleH),
                new PointF(targetX + 4, by + bubbleH),
                new PointF(targetX, by + bubbleH + 6)
            };
            using (SolidBrush tailBrush = new SolidBrush(System.Drawing.Color.White))
            {
                g.FillPolygon(tailBrush, tail);
            }

            // Text
            using (SolidBrush txtBrush = new SolidBrush(System.Drawing.Color.FromArgb(30, 30, 35)))
            {
                g.DrawString(text, bubbleFont, txtBrush, bx + padX, by + padY);
            }
        }

        private void DrawTikTokCommentCard(Graphics g, float cx, float cy)
        {
            float cardW = 190f;
            float cardH = 46f;

            using (GraphicsPath path = new GraphicsPath())
            {
                float r = 6f;
                path.AddArc(cx, cy, r * 2, r * 2, 180, 90);
                path.AddArc(cx + cardW - r * 2, cy, r * 2, r * 2, 270, 90);
                path.AddArc(cx + cardW - r * 2, cy + cardH - r * 2, r * 2, r * 2, 0, 90);
                path.AddArc(cx, cy + cardH - r * 2, r * 2, r * 2, 90, 90);
                path.CloseFigure();

                using (SolidBrush b = new SolidBrush(System.Drawing.Color.FromArgb(230, 255, 255, 255)))
                {
                    g.FillPath(b, path);
                }
                using (Pen p = new Pen(System.Drawing.Color.FromArgb(80, 0, 0, 0), 1f))
                {
                    g.DrawPath(p, path);
                }
            }

            // Small profile circle
            using (SolidBrush avatarBrush = new SolidBrush(System.Drawing.Color.FromArgb(255, 64, 129)))
            {
                g.FillEllipse(avatarBrush, cx + 8, cy + 8, 16, 16);
            }

            using (SolidBrush titleBrush = new SolidBrush(System.Drawing.Color.FromArgb(120, 120, 120)))
            {
                g.DrawString("Reply to @yuhtahn's comment", tiktokFont, titleBrush, cx + 28, cy + 6);
            }

            using (SolidBrush textBrush = new SolidBrush(System.Drawing.Color.FromArgb(20, 20, 20)))
            {
                using (Font boldFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
                {
                    g.DrawString("if (Huyền đồng ý)", boldFont, textBrush, cx + 28, cy + 22);
                }
            }
        }

        private void DrawMarqueeBar(Graphics g, int x, int y, int w, int h, TimelineState state)
        {
            using (SolidBrush bg = new SolidBrush(System.Drawing.Color.FromArgb(18, 20, 26)))
            {
                g.FillRectangle(bg, x, y, w, h);
            }
            using (Pen borderPen = new Pen(System.Drawing.Color.FromArgb(40, 45, 60), 1f))
            {
                g.DrawLine(borderPen, x, y, x + w, y);
                g.DrawLine(borderPen, x, y + h - 1, x + w, y + h - 1);
            }

            // Clip marquee inside
            Region oldClip = g.Clip;
            g.SetClip(new Rectangle(x, y, w, h));

            string fullText = marqueeText + marqueeText + marqueeText;
            using (SolidBrush textBrush = new SolidBrush(System.Drawing.Color.FromArgb(220, 225, 235)))
            {
                g.DrawString(fullText, marqueeFont, textBrush, x + marqueeX, y + 5);
            }

            g.Clip = oldClip;
        }

        private void DrawCodePanel(Graphics g, int x, int y, int w, int h, TimelineState state)
        {
            using (SolidBrush bg = new SolidBrush(System.Drawing.Color.FromArgb(30, 30, 30)))
            {
                g.FillRectangle(bg, x, y, w, h);
            }

            int lineH = (int)(codeFont.GetHeight(g) + 3);
            int startY = y + 10;
            int gutterW = 34;

            for (int i = 0; i < cachedCodeLines.Count; i++)
            {
                CodeLine line = cachedCodeLines[i];
                int curY = startY + i * lineH;
                if (curY + lineH > y + h) break;

                bool isActive = (line.LineNumber == state.ActiveLine);

                // Highlight active line
                if (isActive)
                {
                    using (SolidBrush hl = new SolidBrush(CodeRepository.ColorLineHighlight))
                    {
                        g.FillRectangle(hl, x, curY, w, lineH);
                    }
                    // Debugger Arrow ▶
                    using (SolidBrush arrowBrush = new SolidBrush(CodeRepository.ColorArrow))
                    {
                        PointF[] arrow = new PointF[]
                        {
                            new PointF(x + 5, curY + 3),
                            new PointF(x + 13, curY + lineH * 0.5f),
                            new PointF(x + 5, curY + lineH - 3)
                        };
                        g.FillPolygon(arrowBrush, arrow);
                    }
                }

                // Gutter line number
                using (SolidBrush gutterBrush = new SolidBrush(CodeRepository.ColorGutter))
                {
                    string numStr = line.LineNumber.ToString();
                    g.DrawString(numStr, codeFont, gutterBrush, x + 16, curY);
                }

                // Code tokens
                float tokenX = x + gutterW + 6;
                for (int t = 0; t < line.Tokens.Count; t++)
                {
                    CodeToken token = line.Tokens[t];
                    using (SolidBrush tb = new SolidBrush(token.TextColor))
                    {
                        g.DrawString(token.Text, codeFont, tb, tokenX, curY);
                    }
                    SizeF sz = g.MeasureString(token.Text, codeFont, new PointF(0, 0), StringFormat.GenericTypographic);
                    tokenX += sz.Width;
                }
            }

            // Karaoke Subtitle Bar floating at bottom of code panel if enabled
            if (isKaraokeEnabled && !string.IsNullOrEmpty(state.Lyrics))
            {
                int subH = 30;
                int subY = y + h - subH - 6;

                SizeF lsz = g.MeasureString(state.Lyrics, bubbleFont);
                float boxW = lsz.Width + 24;
                float boxX = x + (w - boxW) * 0.5f;

                using (SolidBrush sbg = new SolidBrush(System.Drawing.Color.FromArgb(200, 20, 22, 28)))
                {
                    g.FillRectangle(sbg, boxX, subY, boxW, subH);
                }
                using (Pen sp = new Pen(System.Drawing.Color.FromArgb(255, 64, 129), 1f))
                {
                    g.DrawRectangle(sp, boxX, subY, boxW, subH);
                }
                using (SolidBrush stxt = new SolidBrush(System.Drawing.Color.FromArgb(255, 230, 240)))
                {
                    g.DrawString(state.Lyrics, bubbleFont, stxt, boxX + 12, subY + 6);
                }
            }
        }

        private void DrawStatusBar(Graphics g, int x, int y, int w, int h, double sec, TimelineState state)
        {
            using (SolidBrush bg = new SolidBrush(System.Drawing.Color.FromArgb(16, 18, 23)))
            {
                g.FillRectangle(bg, x, y, w, h);
            }
            using (Pen borderPen = new Pen(System.Drawing.Color.FromArgb(40, 45, 58), 1f))
            {
                g.DrawLine(borderPen, x, y, x + w, y);
            }

            // Progress bar line at top of status bar
            float progress = (float)(sec / TimelineController.TotalDuration);
            if (progress > 1f) progress = 1f;

            using (SolidBrush barBg = new SolidBrush(System.Drawing.Color.FromArgb(35, 40, 55)))
            {
                g.FillRectangle(barBg, x, y, w, 3);
            }
            using (SolidBrush barFill = new SolidBrush(System.Drawing.Color.FromArgb(255, 64, 129)))
            {
                g.FillRectangle(barFill, x, y, w * progress, 3);
            }

            // Status Text
            int min = (int)sec / 60;
            int s = (int)sec % 60;
            int totMin = (int)TimelineController.TotalDuration / 60;
            int totS = (int)TimelineController.TotalDuration % 60;

            string playIcon = isPlaying ? "❚❚" : "▶";
            string statusStr = string.Format("{0} ♪ {1:D2}:{2:D2} / {3:D2}:{4:D2} • karaoke: {5} (ctrl+t)",
                playIcon, min, s, totMin, totS, isKaraokeEnabled ? "on" : "off");

            using (SolidBrush textBrush = new SolidBrush(System.Drawing.Color.FromArgb(180, 185, 200)))
            {
                g.DrawString(statusStr, statusFont, textBrush, x + 10, y + 9);
            }

            // Shortcut hint on right
            string hint = "[C] Tùy chỉnh | [Space] Phát";
            SizeF hsz = g.MeasureString(hint, statusFont);
            using (SolidBrush hintBrush = new SolidBrush(System.Drawing.Color.FromArgb(110, 120, 140)))
            {
                g.DrawString(hint, statusFont, hintBrush, x + w - hsz.Width - 10, y + 9);
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            int h = this.ClientSize.Height;
            int statusH = 34;
            int statusY = h - statusH;

            // Check if clicked in status bar or progress bar
            if (e.Y >= statusY)
            {
                isScrubbing = true;
                double newSec = (double)e.X / this.ClientSize.Width * TimelineController.TotalDuration;
                SeekTo(newSec);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isScrubbing)
            {
                double newSec = (double)e.X / this.ClientSize.Width * TimelineController.TotalDuration;
                SeekTo(newSec);
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isScrubbing = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                TogglePlayPause();
            }
            else if (e.KeyCode == Keys.R)
            {
                SeekTo(0.0);
            }
            else if (e.KeyCode == Keys.T)
            {
                isKaraokeEnabled = !isKaraokeEnabled;
            }
            else if (e.KeyCode == Keys.C)
            {
                OpenCustomizationDialog();
            }
            else if (e.KeyCode == Keys.F11 || e.KeyCode == Keys.F)
            {
                ToggleFullScreen();
            }
            else if (e.KeyCode == Keys.Left)
            {
                SeekTo(GetCurrentSeconds() - 3.0);
            }
            else if (e.KeyCode == Keys.Right)
            {
                SeekTo(GetCurrentSeconds() + 3.0);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    ToggleFullScreen();
                }
            }
        }

        private void TogglePlayPause()
        {
            isPlaying = !isPlaying;
            if (isAudioLoaded && mediaPlayer != null)
            {
                try
                {
                    if (isPlaying)
                        mediaPlayer.Play();
                    else
                        mediaPlayer.Pause();
                }
                catch { }
            }
        }

        private void ToggleFullScreen()
        {
            if (this.FormBorderStyle != FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                this.ClientSize = new Size(460, 840);
            }
        }

        private void OpenCustomizationDialog()
        {
            using (Form dlg = new Form())
            {
                dlg.Text = "Tùy Chỉnh Lời Tỏ Tình & Tên Crush";
                dlg.Size = new Size(380, 260);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = System.Drawing.Color.FromArgb(30, 32, 40);
                dlg.ForeColor = System.Drawing.Color.White;

                Label lbl1 = new Label { Text = "Tên người nhận (Crush):", Location = new Point(20, 20), AutoSize = true };
                TextBox txtCrush = new TextBox { Text = crushName, Location = new Point(20, 42), Width = 320 };

                Label lbl2 = new Label { Text = "Lời tỏ tình của Hiếu:", Location = new Point(20, 75), AutoSize = true };
                TextBox txtSpeech1 = new TextBox { Text = customBoySpeech, Location = new Point(20, 97), Width = 320 };

                Label lbl3 = new Label { Text = "Lời đồng ý của Huyền:", Location = new Point(20, 130), AutoSize = true };
                TextBox txtSpeech2 = new TextBox { Text = customGirlSpeech, Location = new Point(20, 152), Width = 320 };

                Button btnSave = new Button { Text = "Lưu & Tỏ tình ❤️", Location = new Point(220, 185), Width = 120, Height = 30, BackColor = System.Drawing.Color.FromArgb(255, 64, 129), FlatStyle = FlatStyle.Flat };
                btnSave.Click += delegate {
                    if (!string.IsNullOrEmpty(txtCrush.Text.Trim()))
                    {
                        crushName = txtCrush.Text.Trim();
                        cachedCodeLines = CodeRepository.GetScriptLines(crushName);
                    }
                    customBoySpeech = txtSpeech1.Text.Trim();
                    customGirlSpeech = txtSpeech2.Text.Trim();
                    dlg.DialogResult = DialogResult.OK;
                    dlg.Close();
                };

                dlg.Controls.Add(lbl1);
                dlg.Controls.Add(txtCrush);
                dlg.Controls.Add(lbl2);
                dlg.Controls.Add(txtSpeech1);
                dlg.Controls.Add(lbl3);
                dlg.Controls.Add(txtSpeech2);
                dlg.Controls.Add(btnSave);

                dlg.ShowDialog(this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (animTimer != null) animTimer.Dispose();
                if (mediaPlayer != null) mediaPlayer.Close();
                if (codeFont != null) codeFont.Dispose();
                if (bubbleFont != null) bubbleFont.Dispose();
                if (statusFont != null) statusFont.Dispose();
                if (marqueeFont != null) marqueeFont.Dispose();
                if (tiktokFont != null) tiktokFont.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class Star
    {
        public float XRatio;
        public float YRatio;
        public float Phase;
        public int Size;
        public float Speed;
    }

    public class FloatingHeart
    {
        public float X;
        public float Y;
        public float SpeedY;
        public float Drift;
        public float Scale;
        public float Life;
    }
}
