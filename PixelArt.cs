using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace GoiConTimLamQua
{
    public static class PixelArt
    {
        // Color Palette
        public static readonly Color ColorSkyTop = Color.FromArgb(10, 14, 26);
        public static readonly Color ColorSkyBottom = Color.FromArgb(18, 26, 44);
        public static readonly Color ColorGround = Color.FromArgb(12, 18, 14);
        public static readonly Color ColorGrass = Color.FromArgb(34, 60, 38);
        public static readonly Color ColorMoon = Color.FromArgb(235, 238, 245);
        public static readonly Color ColorMoonCrater = Color.FromArgb(180, 186, 198);

        public static readonly Color ColorBoyBody = Color.FromArgb(239, 83, 80);
        public static readonly Color ColorBoyDark = Color.FromArgb(198, 40, 40);
        public static readonly Color ColorBoyCap = Color.FromArgb(33, 150, 243);
        public static readonly Color ColorBoyCapBrim = Color.FromArgb(25, 118, 210);

        public static readonly Color ColorGirlBody = Color.FromArgb(240, 98, 146);
        public static readonly Color ColorGirlDark = Color.FromArgb(194, 24, 91);
        public static readonly Color ColorGirlBow = Color.FromArgb(255, 213, 79);
        public static readonly Color ColorGirlBowCenter = Color.FromArgb(255, 179, 0);

        public static readonly Color ColorEyeBlack = Color.FromArgb(20, 20, 25);
        public static readonly Color ColorEyeWhite = Color.FromArgb(255, 255, 255);
        public static readonly Color ColorBlush = Color.FromArgb(255, 128, 171);

        public static readonly Color ColorHeart = Color.FromArgb(255, 64, 129);
        public static readonly Color ColorHeartDark = Color.FromArgb(216, 27, 96);
        public static readonly Color ColorHeartShine = Color.FromArgb(255, 170, 200);

        public static readonly Color ColorFlowerPetal = Color.FromArgb(255, 214, 0);
        public static readonly Color ColorFlowerCenter = Color.FromArgb(161, 98, 7);
        public static readonly Color ColorFlowerStem = Color.FromArgb(76, 175, 80);

        // Helper to build Bitmap from ASCII pattern
        public static Bitmap CreateSprite(string[] rows, Dictionary<char, Color> palette, int pixelSize)
        {
            int h = rows.Length;
            int w = rows[0].Length;
            Bitmap bmp = new Bitmap(w * pixelSize, h * pixelSize);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                for (int y = 0; y < h; y++)
                {
                    string row = rows[y];
                    for (int x = 0; x < w; x++)
                    {
                        char c = row[x];
                        if (palette.ContainsKey(c) && palette[c] != Color.Transparent)
                        {
                            using (SolidBrush b = new SolidBrush(palette[c]))
                            {
                                g.FillRectangle(b, x * pixelSize, y * pixelSize, pixelSize, pixelSize);
                            }
                        }
                    }
                }
            }
            return bmp;
        }

        // Boy normal sprite
        public static Bitmap GetBoySprite(int pixelSize, bool step)
        {
            string leg1 = step ? "FF" : "..";
            string leg2 = step ? ".." : "FF";

            string[] map = new string[]
            {
                "......CCCC........",
                ".....CCCCCC.......",
                "....CCCCCCCC......",
                "...RRRRRRRRRR.....",
                "..RRRRRRRRRRRR....",
                "..RRWBRRRRWBRR....",
                "..RRBBRRRRBBRR....",
                "..RRRRRRRRRRRR....",
                "..RRRRRRRRRRRR....",
                "...DDDDDDDDDD.....",
                "...." + leg1 + "...." + leg2 + "......"
            };

            Dictionary<char, Color> pal = new Dictionary<char, Color>();
            pal['.'] = Color.Transparent;
            pal['C'] = ColorBoyCap;
            pal['R'] = ColorBoyBody;
            pal['D'] = ColorBoyDark;
            pal['W'] = ColorEyeWhite;
            pal['B'] = ColorEyeBlack;
            pal['F'] = ColorBoyDark;

            return CreateSprite(map, pal, pixelSize);
        }

        // Girl normal sprite
        public static Bitmap GetGirlSprite(int pixelSize, bool step)
        {
            string leg1 = step ? "FF" : "..";
            string leg2 = step ? ".." : "FF";

            string[] map = new string[]
            {
                ".......YYOYY......",
                ".......YYOYY......",
                "....PPPPPPPPPP....",
                "...PPPPPPPPPPPP...",
                "..PPPPPPPPPPPPPP..",
                "..PPWRPPPPPPWRPP..",
                "..PPRRPPPPPPRRPP..",
                "..PPPPPPPPPPPPPP..",
                "..PPPPPPPPPPPPPP..",
                "...MMMMMMMMMMMM...",
                "...." + leg1 + "...." + leg2 + "......"
            };

            Dictionary<char, Color> pal = new Dictionary<char, Color>();
            pal['.'] = Color.Transparent;
            pal['Y'] = ColorGirlBow;
            pal['O'] = ColorGirlBowCenter;
            pal['P'] = ColorGirlBody;
            pal['M'] = ColorGirlDark;
            pal['W'] = ColorEyeWhite;
            pal['R'] = ColorEyeBlack;
            pal['F'] = ColorGirlDark;

            return CreateSprite(map, pal, pixelSize);
        }

        // Hugging couple sprite
        public static Bitmap GetHuggingSprite(int pixelSize, float pulse)
        {
            string[] map = new string[]
            {
                "....CCCC...........YYOYY....",
                "...CCCCCC..........YYOYY....",
                "..CCCCCCCC.......PPPPPPPP...",
                "..RRRRRRRRRR...PPPPPPPPPP...",
                ".RRRRRRRRRRRR.PPPPPPPPPPPP..",
                ".RR^^RRRRLRPPPPPPP^^PPPPPP..",
                ".RRRRRRRRLRPPPPPPBLPPPPPPP..",
                ".RRRRRRRRRRRR.PPPPPPPPPPPP..",
                "..DDDDDDDDDD...MMMMMMMMMM...",
                "....FF..FF.......FF..FF....."
            };

            Dictionary<char, Color> pal = new Dictionary<char, Color>();
            pal['.'] = Color.Transparent;
            pal['C'] = ColorBoyCap;
            pal['R'] = ColorBoyBody;
            pal['D'] = ColorBoyDark;
            pal['Y'] = ColorGirlBow;
            pal['O'] = ColorGirlBowCenter;
            pal['P'] = ColorGirlBody;
            pal['M'] = ColorGirlDark;
            pal['^'] = ColorEyeBlack; // happy closed eyes
            pal['B'] = ColorBlush;
            pal['L'] = Color.FromArgb(255, 180, 200); // embrace join
            pal['F'] = Color.FromArgb(120, 20, 40);

            return CreateSprite(map, pal, pixelSize);
        }

        // Pixel Heart
        public static Bitmap GetHeartSprite(int pixelSize, bool shiny)
        {
            string[] map = new string[]
            {
                "..RR..RR..",
                ".RSRRRRRR.",
                "RRSRRRRRRR",
                "RRRRRRRRRR",
                ".RRRRRRRR.",
                "..RRRRRR..",
                "...RRRR...",
                "....RR...."
            };

            Dictionary<char, Color> pal = new Dictionary<char, Color>();
            pal['.'] = Color.Transparent;
            pal['R'] = ColorHeart;
            pal['S'] = shiny ? ColorHeartShine : ColorHeart;

            return CreateSprite(map, pal, pixelSize);
        }

        // Pixel Sunflower / Blossom
        public static Bitmap GetFlowerSprite(int pixelSize, bool withered)
        {
            string[] map;
            if (!withered)
            {
                map = new string[]
                {
                    "...YY...",
                    ".YYYYYY.",
                    ".YYCCYY.",
                    ".YYYYYY.",
                    "...YY...",
                    "...SS...",
                    "...SS...",
                    ".SSSS...",
                    "...SS..."
                };
            }
            else
            {
                map = new string[]
                {
                    "........",
                    "....YY..",
                    "...YYC..",
                    "....YY..",
                    "...S....",
                    "...S....",
                    "..S.....",
                    ".SS.....",
                    "..S....."
                };
            }

            Dictionary<char, Color> pal = new Dictionary<char, Color>();
            pal['.'] = Color.Transparent;
            pal['Y'] = withered ? Color.FromArgb(180, 150, 40) : ColorFlowerPetal;
            pal['C'] = withered ? Color.FromArgb(100, 70, 20) : ColorFlowerCenter;
            pal['S'] = withered ? Color.FromArgb(70, 110, 60) : ColorFlowerStem;

            return CreateSprite(map, pal, pixelSize);
        }

        // Moon sprite
        public static Bitmap GetMoonSprite(int pixelSize)
        {
            string[] map = new string[]
            {
                ".....MMMMMM.....",
                "...MMMMMMMMMM...",
                "..MMCCMMMMMMMM..",
                ".MMMCCMMMMCCMMM.",
                ".MMMMMMMMMCCMMM.",
                "MMMMMMMMMMMMMMMM",
                "MMCCMMMMMMMMMMMM",
                "MMCCMMMMMMCCMMMM",
                "MMMMMMMMMMCCMMMM",
                "MMMMMMMMMMMMMMMM",
                ".MMMMCCMMMMMMMM.",
                ".MMMMCCMMMMMMMM.",
                "..MMMMMMMMMMMM..",
                "...MMMMMMMMMM...",
                ".....MMMMMM....."
            };

            Dictionary<char, Color> pal = new Dictionary<char, Color>();
            pal['.'] = Color.Transparent;
            pal['M'] = ColorMoon;
            pal['C'] = ColorMoonCrater;

            return CreateSprite(map, pal, pixelSize);
        }
    }
}
