using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZC
{
    class TallyCounter                       // keeps track of how many times you did something, 1 click per
    {
        private int current = 0;
        private int max = 0;

        public TallyCounter(int max)         // set the maximum number of clicks before restarting
        {
            this.max = max;
        }
        public bool click()             // if this click exceeds the max, then the bell rings, otherwise not
        {
            current++;
            if (current >= max)
            {
                current = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void resetMax(int i)     // so you can reuse a clicker
        {
            this.max = i;
        }
    }
}
