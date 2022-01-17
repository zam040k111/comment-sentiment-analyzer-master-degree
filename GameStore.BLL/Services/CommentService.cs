using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.ML.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISentimentService _sentimentService;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, ISentimentService sentimentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sentimentService = sentimentService;
        }

        public CommentDto Add(CommentDto itemDto)
        {
            var comment = _mapper.Map<Comment>(itemDto);

            if (comment.ParentComment != null)
            {
                comment.GameId = comment.ParentComment.GameId;
                comment.ParentComment = null;
            }

            comment.Score = _sentimentService.PredictSentiment(comment.Body)[0];
            _unitOfWork.CommentRepository.Add(comment);
            _unitOfWork.Save();
            UpdateGameScore(comment.GameKey);

            return _mapper.Map<CommentDto>(comment);
        }

        public CommentDto Update(CommentDto itemDto)
        {
            var comment = _mapper.Map<Comment>(itemDto);

            if (comment.ParentComment != null)
            {
                comment.GameId = comment.ParentComment.GameId;
            }

            comment.Score = _sentimentService.PredictSentiment(comment.Body)[0];
            _unitOfWork.CommentRepository.Update(comment);
            _unitOfWork.Save();
            UpdateGameScore(comment.GameKey);

            return _mapper.Map<CommentDto>(comment);
        }

        public void Delete(int id)
        {
            var comment = _unitOfWork.CommentRepository.GetSingle(com => com, predicates: comment => comment.Id == id);

            if (comment == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.CommentRepository.Delete(comment);
            _unitOfWork.Save();
            UpdateGameScore(comment.GameKey);
        }

        public CommentDto GetById(int id)
        {
            var result = _unitOfWork.CommentRepository
                .GetSingle(com => com, predicates: comment => comment.Id == id, include: commentInclude => commentInclude
                    .Include(comment => comment.Game)
                    .Include(comment => comment.ParentComment));

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<CommentDto>(result);
        }

        public List<CommentDto> GetAll(bool includeIsDeleted = false)
        {
            var result = _unitOfWork.CommentRepository
                .GetAll(commentInclude => commentInclude
                    .Include(comment => comment.Game)
                    .Include(comment => comment.ParentComment),
                    includeIsDeleted).ToList();
            
            return _mapper.Map<List<CommentDto>>(result);
        }

        public List<CommentDto> GetAllByGameKey(string key, bool includeIsDeleted = false)
        {
            var result = _unitOfWork.CommentRepository
                .GetAll(commentInclude => commentInclude
                    .Include(comment => comment.Game)
                    .Include(comment => comment.ParentComment),
                    includeIsDeleted,
                    comment => comment.GameKey.Equals(key)).ToList();

            var unscoredComments = result.Where(i => i.Score == 0);

            if (unscoredComments.Any())
            {
                foreach (var item in unscoredComments)
                {
                    item.Score = _sentimentService.PredictSentiment(item.Body)[0];
                    _unitOfWork.CommentRepository.Update(item);
                }

                _unitOfWork.Save();
            }

            return _mapper.Map<List<CommentDto>>(result);
        }

        public void UpdateGameScore(string key)
        {
            var game = _unitOfWork.GameRepository.GetSingle(game => game, predicates: i => i.Key == key);

            if (game != null)
            {
                var filteredComments = GetAllByGameKey(key).Where(i => i.Body.Length > 15);
                game.Score = filteredComments.Any() ? filteredComments.Average(i => i.Score) : 0;
                _unitOfWork.GameRepository.Update(game);
                _unitOfWork.Save();
            }
        }
    }
}
