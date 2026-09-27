using System;

namespace GoiConTimLamQua
{
    public enum BoyPose
    {
        Idle,
        HoldingHeart,
        Walking,
        Hugging
    }

    public enum GirlPose
    {
        Idle,
        HoldingFlower,
        FlowerWithering,
        Walking,
        Hugging
    }

    public class TimelineState
    {
        public int ActiveLine;
        public string BoySpeech;
        public string GirlSpeech;
        public BoyPose BoyAction;
        public GirlPose GirlAction;
        public bool ShowHearts;
        public bool BigHeartShower;
        public string Lyrics;
        public float WalkProgress; // 0 (far apart) to 1 (together)
    }

    public static class TimelineController
    {
        public static readonly double TotalDuration = 46.0;

        public static TimelineState Evaluate(double seconds, string customBoySpeech1, string customGirlSpeechFinal)
        {
            TimelineState s = new TimelineState();
            s.ActiveLine = 2;
            s.BoyAction = BoyPose.Idle;
            s.GirlAction = GirlPose.Idle;
            s.ShowHearts = false;
            s.BigHeartShower = false;
            s.WalkProgress = 0f;

            if (string.IsNullOrEmpty(customBoySpeech1)) customBoySpeech1 = "anh thích Huyền";
            if (string.IsNullOrEmpty(customGirlSpeechFinal)) customGirlSpeechFinal = "ừ, Huyền đồng ý";

            if (seconds < 2.2)
            {
                s.ActiveLine = 2;
                s.BoySpeech = customBoySpeech1;
                s.Lyrics = "Gom chân thành đôi mươi...";
            }
            else if (seconds < 6.5)
            {
                s.ActiveLine = 3;
                s.BoySpeech = "Huyền cười đẹp lắm";
                s.Lyrics = "...để đổi lấy đôi môi em cười...";
            }
            else if (seconds < 13.8)
            {
                s.ActiveLine = 4;
                s.BoySpeech = "Huyền đồng ý nhé?";
                s.Lyrics = "Chỉ cần em... đồng ý...";
            }
            else if (seconds < 17.8)
            {
                s.ActiveLine = 6;
                s.GirlAction = GirlPose.HoldingFlower;
                s.Lyrics = "Em trao nhành hoa...";
            }
            else if (seconds < 21.0)
            {
                s.ActiveLine = 7;
                s.BoyAction = BoyPose.HoldingHeart;
                s.GirlAction = GirlPose.HoldingFlower;
                s.BoySpeech = "Hiếu gói tim tặng Huyền";
                s.Lyrics = "...Hiếu gói con tim làm quà...";
            }
            else if (seconds < 23.2)
            {
                s.ActiveLine = 9;
                s.GirlAction = GirlPose.FlowerWithering;
                s.BoySpeech = "sợ hoa tàn mất...";
                s.Lyrics = "...nhưng sợ hoa sẽ tàn úa...";
            }
            else if (seconds < 28.0)
            {
                s.ActiveLine = 10;
                s.ShowHearts = true;
                s.Lyrics = "...theo nhịp đập thời gian...";
            }
            else if (seconds < 31.0)
            {
                s.ActiveLine = 13;
                s.BoyAction = BoyPose.Walking;
                s.GirlAction = GirlPose.Walking;
                s.BoySpeech = "về đây với Hiếu";
                s.Lyrics = "Xin Huyền về đây với Hiếu...";

                // Animate walking together from 28.0 to 31.0
                float p = (float)((seconds - 28.0) / 3.0);
                if (p < 0f) p = 0f;
                if (p > 1f) p = 1f;
                s.WalkProgress = p;
            }
            else if (seconds < 36.2)
            {
                s.ActiveLine = 14;
                s.BoyAction = BoyPose.Hugging;
                s.GirlAction = GirlPose.Hugging;
                s.ShowHearts = true;
                s.WalkProgress = 1f;
                s.Lyrics = "...Hiếu giữ Huyền bên trong vòng tay...";
            }
            else if (seconds < 39.8)
            {
                s.ActiveLine = 15;
                s.BoyAction = BoyPose.Hugging;
                s.GirlAction = GirlPose.Hugging;
                s.ShowHearts = true;
                s.WalkProgress = 1f;
                s.BoySpeech = "ở bên Hiếu mãi nhé?";
                s.Lyrics = "...đâu sợ mai cách rời...";
            }
            else
            {
                s.ActiveLine = 16;
                s.BoyAction = BoyPose.Hugging;
                s.GirlAction = GirlPose.Hugging;
                s.ShowHearts = true;
                s.BigHeartShower = true;
                s.WalkProgress = 1f;
                s.GirlSpeech = customGirlSpeechFinal;
                s.Lyrics = "...Có Huyền rồi, Hiếu nâng niu suốt đời...";
            }

            return s;
        }
    }
}
