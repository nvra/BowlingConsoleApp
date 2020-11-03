using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingConsoleApp.Models
{

    public class BowlingResponse
    {
        public string playerName { get; set; }

        public int gameId { get; set; }

        public List<Frame> frames { get; set; }

        public int totalScore { get; set; }
    }

    public class Frame
    {
        public int frameNum { get; set; }

        public List<Throw> throws { get; set; }

        public int totalScore { get; set; }
    }

    public class Throw
    {
        public int throwNum { get; set; }

        public string score { get; set; }
    }
}
