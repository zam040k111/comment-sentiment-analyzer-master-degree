using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.BLL.Services
{
    public class PlatformTypeService : IPlatformTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlatformTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Result<PlatformTypeDto> Add(PlatformTypeDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid || result.IsRestored)
            {
                return result;
            }

            var platform = _mapper.Map<PlatformType>(itemDto);

            _unitOfWork.PlatformTypeRepository.Add(platform);
            _unitOfWork.Save();
            result.Value = _mapper.Map<PlatformTypeDto>(platform);

            return result;
        }

        public Result<PlatformTypeDto> Update(PlatformTypeDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var newPlatformType = _mapper.Map<PlatformType>(itemDto);

            _unitOfWork.GamePlatformTypeRepository.TryUpdateManyToMany(_unitOfWork.GamePlatformTypeRepository
                    .GetAll(predicates: platformType => platformType.PlatformTypeId == newPlatformType.Id),
                newPlatformType.GamePlatformTypes.Select(gamePlatformType => new GamePlatformType
                {
                    GameId = gamePlatformType.GameId,
                    PlatformTypeId = newPlatformType.Id
                }), platformType => platformType.GameId);

            _unitOfWork.PlatformTypeRepository.Update(newPlatformType);
            _unitOfWork.Save();
            result.Value = _mapper.Map<PlatformTypeDto>(newPlatformType);

            return result;
        }

        public void Delete(int id)
        {
            var platformTypeItem = _unitOfWork.PlatformTypeRepository
                .GetSingle(type => type, include: platformTypeInclude => platformTypeInclude
                        .Include(platformType => platformType.GamePlatformTypes),
                    predicates: platformType => platformType.Id == id);

            if (platformTypeItem == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.PlatformTypeRepository.Delete(platformTypeItem);
            _unitOfWork.Save();
        }

        public PlatformTypeDto GetById(int id)
        {
            var result = _unitOfWork.PlatformTypeRepository
                .GetSingle(type => type, include: platformTypeInclude => platformTypeInclude
                        .Include(platformType => platformType.GamePlatformTypes),
                    predicates: platformType => platformType.Id == id);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<PlatformTypeDto>(result);
        }

        public List<PlatformTypeDto> GetAll(bool includeIsDeleted = false)
        {
            var result = _unitOfWork.PlatformTypeRepository
                .GetAll(platformTypeInclude => platformTypeInclude
                    .Include(platformType => platformType.GamePlatformTypes),
                    includeIsDeleted).ToList();

            return _mapper.Map<List<PlatformTypeDto>>(result);
        }

        public List<PlatformTypeDto> GetByGame(int gameId)
        {
            var result = _unitOfWork.PlatformTypeRepository
                .GetAll(platformTypeInclude => platformTypeInclude
                        .Include(platformType => platformType.GamePlatformTypes),
                    predicates: platformType => platformType.GamePlatformTypes
                        .Any(gamePlatformType => gamePlatformType.GameId == gameId)).ToList();

            return _mapper.Map<List<PlatformTypeDto>>(result);
        }

        private Result<PlatformTypeDto> CheckValidity(PlatformTypeDto itemDto)
        {
            var result = new Result<PlatformTypeDto> { Value = itemDto };
            var platform = _unitOfWork.PlatformTypeRepository
                .GetSingle(type => type, predicates: gm => gm.Type.Equals(itemDto.Type), includeDeleted: true);

            if (platform != null && itemDto.Id != platform.Id)
            {
                if (platform.IsDeleted)
                {
                    _unitOfWork.PlatformTypeRepository.Restore(platform);
                    result.IsRestored = true;
                    return result;
                }

                result.Errors.Add(itemDto.GetPropName(p => p.Type), itemDto.GetMessage(m => m.Type));
            }

            return result;
        }
    }
}