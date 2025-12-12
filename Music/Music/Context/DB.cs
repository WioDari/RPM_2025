using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Context
{
    public static class DB
    {
        public static MusicContext Context = null!;
        public static void LoadContext()
        {
            Context = new();
        }
    }
}
