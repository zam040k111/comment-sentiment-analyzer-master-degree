using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Result<PublisherDto> Add(PublisherDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var publisher = _mapper.Map<Publisher>(itemDto);
            _unitOfWork.PublisherRepository.Add(publisher);
            _unitOfWork.Save();
            result.Value = _mapper.Map<PublisherDto>(publisher);

            return result;
        }

        public Result<PublisherDto> Update(PublisherDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var publisher = _mapper.Map<Publisher>(itemDto);
            _unitOfWork.PublisherRepository.Update(publisher);
            _unitOfWork.Save();
            result.Value = _mapper.Map<PublisherDto>(publisher);

            return result;
        }

        public void Delete(int id)
        {
            var publisher = _unitOfWork.PublisherRepository.GetSingle(pbr => pbr, predicates: i => i.Id == id);

            if (publisher == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.PublisherRepository.Delete(publisher);
            _unitOfWork.Save();
        }

        public PublisherDto GetById(int id)
        {
            var result = _unitOfWork.PublisherRepository.GetSingle(pbr => pbr, predicates: p => p.Id == id);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<PublisherDto>(result);
        }

        public List<PublisherDto> GetAll(bool includeIsDeleted = false)
        {
            var result = _unitOfWork.PublisherRepository.GetAll(includeDeleted: includeIsDeleted);

            return _mapper.Map<List<PublisherDto>>(result);
        }

        private Result<PublisherDto> CheckValidity(PublisherDto itemDto)
        {
            var result = new Result<PublisherDto> { Value = itemDto };
            var publisher = _unitOfWork.PublisherRepository
                .GetSingle(pbr => pbr, predicates: gm => gm.CompanyName.Equals(itemDto.CompanyName), includeDeleted: true);

            if (publisher != null && itemDto.Id != publisher.Id)
            {
                result.Errors.Add(itemDto.GetPropName(p => p.CompanyName), itemDto.GetMessage(m => m.CompanyName));
            }

            return result;
        }
    }
}
