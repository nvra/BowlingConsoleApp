using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingConsoleApp.Models
{

    public class FramethrowRequest
    {
        public int GameId { get; set; }

        public int FrameNum { get; set; }

        public int ThrowNum { get; set; }

        public int Score { get; set; }

        public char Foul { get; set; }
    }
}
