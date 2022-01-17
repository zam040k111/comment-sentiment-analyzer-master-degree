
using System.Collections.Generic;

namespace GameStore.WEB.Interfaces
{
    public interface IPdfObjectData
    {
        List<string> ColumnNames { get; set; }

        List<float> ColumnWidths { get; set; }

        List<List<string>> Rows { get; set; }

        List<string> Headers { get; set; }
        
        string Footer { get; set; }
    }
}
