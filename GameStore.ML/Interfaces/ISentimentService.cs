using GameStore.ML.Models;

namespace GameStore.ML.Interfaces
{
    public interface ISentimentService
    {
        float[] PredictSentiment(params string[] text);
        Output[] PredictSentiment(params Input[] text);
    }
}
