using GameStore.ML.Interfaces;
using GameStore.ML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GameStore.ML.Services
{
    public class SentimentService : ISentimentService
    {
        private readonly PredictionEngine<Input, Output> _engine;
        private readonly string _modelPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + _modelDir;
        private const string _modelDir = "\\MLModels\\Sentiment";
        private const string _modelData = "imdb_word_index.csv";

        public SentimentService()
        {
            var mlContext = new MLContext();

            var lookupMap = mlContext.Data.LoadFromTextFile(
                Path.Combine(_modelPath, _modelData),
                columns: new[] { new TextLoader.Column("Words", DataKind.String, 0), new TextLoader.Column("Ids", DataKind.Int32, 1), },
                separatorChar: ',');

            Action<VariableLength, FixedLength> ResizeFeaturesAction = (s, f) =>
            {
                var features = s.VariableLengthFeatures;
                Array.Resize(ref features, 600);
                f.Features = features;
            };

            var tensorFlowModel = mlContext.Model.LoadTensorFlowModel(_modelPath);
            var schema = tensorFlowModel.GetModelSchema();
            var featuresType = (VectorDataViewType)schema["Features"].Type;
            var predictionType = (VectorDataViewType)schema["Prediction/Softmax"].Type;

            var pipeline = mlContext.Transforms.Text.TokenizeIntoWords("TokenizedWords", "ReviewText")
                .Append(mlContext.Transforms.Conversion.MapValue("VariableLengthFeatures", lookupMap,
                    lookupMap.Schema["Words"], lookupMap.Schema["Ids"], "TokenizedWords"))
                .Append(mlContext.Transforms.CustomMapping(ResizeFeaturesAction, "Resize"))
                .Append(tensorFlowModel.ScoreTensorFlowModel("Prediction/Softmax", "Features"))
                .Append(mlContext.Transforms.CopyColumns("Prediction", "Prediction/Softmax"));

            var dataView = mlContext.Data.LoadFromEnumerable(new List<Input>());
            var model = pipeline.Fit(dataView);
            _engine = mlContext.Model.CreatePredictionEngine<Input, Output>(model);
        }

        public float[] PredictSentiment(params string[] text)
        {
            var predictions = text.ToList().Select(i => _engine.Predict(new Input { ReviewText = i }));

            return predictions.Select(i => i.Prediction[1]).ToArray();
        }

        public Output[] PredictSentiment(params Input[] text)
        {
            var predictions = text.ToList().Select(i => _engine.Predict(i));

            return predictions.ToArray();
        }
    }
}
