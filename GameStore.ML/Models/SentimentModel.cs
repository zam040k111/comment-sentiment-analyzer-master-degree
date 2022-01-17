using GameStore.ML.Interfaces;
using Microsoft.ML.Data;

namespace GameStore.ML.Models
{
    public class Input
    {
        public string ReviewText { get; set; }
    }

    public class Output
    {
        [VectorType(2)]
        public float[] Prediction { get; set; }
    }

    public class VariableLength
    {
        [VectorType]
        public int[] VariableLengthFeatures { get; set; }
    }

    public class FixedLength
    {
        [VectorType(600)]
        public int[] Features { get; set; }
    }
}
