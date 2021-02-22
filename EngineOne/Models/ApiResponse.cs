using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineOne.Models
{
    public class ApiResponse
    {

            public List<Picture> pictures { get; set; }
            public int page { get; set; }
            public int pageCount { get; set; }
            public bool hasMore { get; set; }

    }
}
