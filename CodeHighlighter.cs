using System;
using System.Drawing;
using System.Collections.Generic;

namespace GoiConTimLamQua
{
    public class CodeToken
    {
        public string Text;
        public Color TextColor;

        public CodeToken(string text, Color color)
        {
            this.Text = text;
            this.TextColor = color;
        }
    }

    public class CodeLine
    {
        public int LineNumber;
        public List<CodeToken> Tokens;

        public CodeLine(int num)
        {
            this.LineNumber = num;
            this.Tokens = new List<CodeToken>();
        }

        public void Add(string text, Color color)
        {
            this.Tokens.Add(new CodeToken(text, color));
        }
    }

    public static class CodeRepository
    {
        public static readonly Color ColorKeyword = Color.FromArgb(86, 156, 214);      // #569CD6 VS Code Keyword
        public static readonly Color ColorFunction = Color.FromArgb(220, 220, 170);    // #DCDCAA VS Code Function
        public static readonly Color ColorVariable = Color.FromArgb(156, 220, 254);    // #9CDCFE VS Code Variable
        public static readonly Color ColorString = Color.FromArgb(206, 145, 120);      // #CE9178 VS Code String
        public static readonly Color ColorNumber = Color.FromArgb(181, 206, 168);      // #B5CEA8 VS Code Number
        public static readonly Color ColorPunctuation = Color.FromArgb(212, 212, 212); // #D4D4D4 VS Code Punctuation
        public static readonly Color ColorGutter = Color.FromArgb(133, 133, 133);      // #858585 Line Numbers
        public static readonly Color ColorArrow = Color.FromArgb(79, 193, 255);        // #4FC1FF Debug Arrow
        public static readonly Color ColorLineHighlight = Color.FromArgb(42, 45, 58);   // #2A2D3A Active Line Highlight

        public static List<CodeLine> GetScriptLines(string crushName)
        {
            if (string.IsNullOrEmpty(crushName)) crushName = "Huyền";

            List<CodeLine> lines = new List<CodeLine>();

            // 1: async function goiConTim(em) {
            CodeLine l1 = new CodeLine(1);
            l1.Add("async function ", ColorKeyword);
            l1.Add("goiConTim", ColorFunction);
            l1.Add("(", ColorPunctuation);
            l1.Add(crushName, ColorVariable);
            l1.Add(") {", ColorPunctuation);
            lines.Add(l1);

            // 2:   const chanThanh = gather(20);
            CodeLine l2 = new CodeLine(2);
            l2.Add("  const ", ColorKeyword);
            l2.Add("chanThanh", ColorVariable);
            l2.Add(" = ", ColorPunctuation);
            l2.Add("gather", ColorFunction);
            l2.Add("(", ColorPunctuation);
            l2.Add("20", ColorNumber);
            l2.Add(");", ColorPunctuation);
            lines.Add(l2);

            // 3:   const moi = await doi(chanThanh);
            CodeLine l3 = new CodeLine(3);
            l3.Add("  const ", ColorKeyword);
            l3.Add("moi", ColorVariable);
            l3.Add(" = ", ColorPunctuation);
            l3.Add("await ", ColorKeyword);
            l3.Add("doi", ColorFunction);
            l3.Add("(", ColorPunctuation);
            l3.Add("chanThanh", ColorVariable);
            l3.Add(");", ColorPunctuation);
            lines.Add(l3);

            // 4:   if (!em.dongY) return cho();
            CodeLine l4 = new CodeLine(4);
            l4.Add("  if ", ColorKeyword);
            l4.Add("(!", ColorPunctuation);
            l4.Add(crushName, ColorVariable);
            l4.Add(".", ColorPunctuation);
            l4.Add("dongY", ColorVariable);
            l4.Add(") ", ColorPunctuation);
            l4.Add("return ", ColorKeyword);
            l4.Add("cho", ColorFunction);
            l4.Add("();", ColorPunctuation);
            lines.Add(l4);

            // 5: [Empty]
            lines.Add(new CodeLine(5));

            // 6:   const hoa = await em.trao("hoa");
            CodeLine l6 = new CodeLine(6);
            l6.Add("  const ", ColorKeyword);
            l6.Add("hoa", ColorVariable);
            l6.Add(" = ", ColorPunctuation);
            l6.Add("await ", ColorKeyword);
            l6.Add(crushName, ColorVariable);
            l6.Add(".", ColorPunctuation);
            l6.Add("trao", ColorFunction);
            l6.Add("(\"", ColorPunctuation);
            l6.Add("hoa", ColorString);
            l6.Add("\");", ColorPunctuation);
            lines.Add(l6);

            // 7:   const qua = goi(conTim, hoa);
            CodeLine l7 = new CodeLine(7);
            l7.Add("  const ", ColorKeyword);
            l7.Add("qua", ColorVariable);
            l7.Add(" = ", ColorPunctuation);
            l7.Add("goi", ColorFunction);
            l7.Add("(", ColorPunctuation);
            l7.Add("conTim", ColorVariable);
            l7.Add(", ", ColorPunctuation);
            l7.Add("hoa", ColorVariable);
            l7.Add(");", ColorPunctuation);
            lines.Add(l7);

            // 8: [Empty]
            lines.Add(new CodeLine(8));

            // 9:   while (hoa.tanUa) {
            CodeLine l9 = new CodeLine(9);
            l9.Add("  while ", ColorKeyword);
            l9.Add("(", ColorPunctuation);
            l9.Add("hoa", ColorVariable);
            l9.Add(".", ColorPunctuation);
            l9.Add("tanUa", ColorVariable);
            l9.Add(") {", ColorPunctuation);
            lines.Add(l9);

            // 10:     qua.nhipDap += tick();
            CodeLine l10 = new CodeLine(10);
            l10.Add("    qua", ColorVariable);
            l10.Add(".", ColorPunctuation);
            l10.Add("nhipDap", ColorVariable);
            l10.Add(" += ", ColorPunctuation);
            l10.Add("tick", ColorFunction);
            l10.Add("();", ColorPunctuation);
            lines.Add(l10);

            // 11:   }
            CodeLine l11 = new CodeLine(11);
            l11.Add("  }", ColorPunctuation);
            lines.Add(l11);

            // 12: [Empty]
            lines.Add(new CodeLine(12));

            // 13:   await em.veDay();
            CodeLine l13 = new CodeLine(13);
            l13.Add("  await ", ColorKeyword);
            l13.Add(crushName, ColorVariable);
            l13.Add(".", ColorPunctuation);
            l13.Add("veDay", ColorFunction);
            l13.Add("();", ColorPunctuation);
            lines.Add(l13);

            // 14:   const tay = giu(em, chatHon);
            CodeLine l14 = new CodeLine(14);
            l14.Add("  const ", ColorKeyword);
            l14.Add("tay", ColorVariable);
            l14.Add(" = ", ColorPunctuation);
            l14.Add("giu", ColorFunction);
            l14.Add("(", ColorPunctuation);
            l14.Add(crushName, ColorVariable);
            l14.Add(", ", ColorPunctuation);
            l14.Add("chatHon", ColorVariable);
            l14.Add(");", ColorPunctuation);
            lines.Add(l14);

            // 15:   if (cachRoi) return khong(tay);
            CodeLine l15 = new CodeLine(15);
            l15.Add("  if ", ColorKeyword);
            l15.Add("(", ColorPunctuation);
            l15.Add("cachRoi", ColorVariable);
            l15.Add(") ", ColorPunctuation);
            l15.Add("return ", ColorKeyword);
            l15.Add("khong", ColorFunction);
            l15.Add("(", ColorPunctuation);
            l15.Add("tay", ColorVariable);
            l15.Add(");", ColorPunctuation);
            lines.Add(l15);

            // 16:   return nangNiu(em, suotDoi);
            CodeLine l16 = new CodeLine(16);
            l16.Add("  return ", ColorKeyword);
            l16.Add("nangNiu", ColorFunction);
            l16.Add("(", ColorPunctuation);
            l16.Add(crushName, ColorVariable);
            l16.Add(", ", ColorPunctuation);
            l16.Add("suotDoi", ColorVariable);
            l16.Add(");", ColorPunctuation);
            lines.Add(l16);

            // 17: }
            CodeLine l17 = new CodeLine(17);
            l17.Add("}", ColorPunctuation);
            lines.Add(l17);

            return lines;
        }
    }
}
