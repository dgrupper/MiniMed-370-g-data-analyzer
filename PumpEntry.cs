using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMed_370_g_data_analyzer
{
    class PumpEntry
    {
        public string Index { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int BG { get; set; }
        public double ISIG { get; set; }

    }
}
