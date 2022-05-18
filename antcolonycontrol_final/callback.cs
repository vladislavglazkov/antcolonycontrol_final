using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntAlgo
{
    public interface EventsCallback
    {
        public void Handle(double time, Ant ant, int type);
    }
}
