using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingConsoleApp.Models
{

    public class BowlingResponse
    {
        public string PlayerName { get; set; }

        public int GameId { get; set; }

        public List<Frame> Frames { get; set; }

        public int TotalScore { get; set; }
    }

    public class Frame
    {
        public int FrameNum { get; set; }

        public List<Throw> Throws { get; set; }

        public int TotalScore { get; set; }
    }

    public class Throw
    {
        public int ThrowNum { get; set; }

        public string Score { get; set; }
    }
}
