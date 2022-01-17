using System;
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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Result<OrderDto> Add(OrderDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var order = _mapper.Map<Order>(itemDto);

            foreach (var orderDetails in order.OrderDetails)
            {
                orderDetails.Product.UnitsInStock -= orderDetails.Quantity;
                _unitOfWork.GameRepository.Update(orderDetails.Product);
                _unitOfWork.OrderDetailRepository.Add(orderDetails);
            }

            order.DateTime = DateTime.Now;
            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.Save();
            result.Value = _mapper.Map<OrderDto>(order);

            return result;
        }

        public Result<OrderDto> Update(OrderDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var order = _mapper.Map<Order>(itemDto);
            _unitOfWork.OrderRepository.Update(order);
            _unitOfWork.Save();
            result.Value = _mapper.Map<OrderDto>(order);

            return result;
        }

        public void Delete(int id)
        {
            var order = _unitOfWork.OrderRepository.GetSingle(ord => ord, predicates: ord => ord.Id == id);

            if (order == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.OrderRepository.Delete(order);
            _unitOfWork.Save();
        }

        public OrderDto GetById(int id)
        {
            var order = _mapper.Map<OrderDto>(_unitOfWork.OrderRepository.GetSingle(ord => ord, include: ord => ord
                    .Include(p => p.OrderDetails)
                    .ThenInclude(p => p.Product),
                predicates: ord => ord.Id == id));

            if (order == null)
            {
                throw new NotFoundException();
            }

            return order;
        }

        public List<OrderDto> GetAll(bool includeIsDeleted = false)
        {
            var order = _mapper.Map<List<OrderDto>>(_unitOfWork.OrderRepository.GetAll(includeDeleted: includeIsDeleted));

            return order;
        }

        public Result<OrderDto> Buy(OrderDto order)
        {
            //TODO: from both db
            order.OrderDetails.ForEach(od => od.Product =
                    _mapper.Map<GameDto>(_unitOfWork.GameRepository.GetSingle(game => game,
                        predicates: game => game.Id == od.Product.Id)));

            return Add(order);
        }

        public void AddPaymentInfo(IPaymentInfo info)
        {
            if (info is VisaModelDto visaModel)
            {
                _unitOfWork.VisaRepository.Add(_mapper.Map<VisaModel>(visaModel));
            }

            _unitOfWork.Save();
        }

        private Result<OrderDto> CheckValidity(OrderDto itemDto)
        {
            var result = new Result<OrderDto> {Value = itemDto};

            if (itemDto.OrderDetails != null && itemDto.OrderDetails.Any())
            {
                foreach (var orderDetails in itemDto.OrderDetails)
                {
                    if (orderDetails.Product.UnitsInStock - orderDetails.Quantity < 0)
                    {
                        result.Errors.Add(itemDto.GetPropName(p => p.TotalPrice), itemDto.GetMessage(m => m.TotalPrice));
                        return result;
                    }
                }
            }
            else
            {
                result.Errors.Add(itemDto.GetPropName(p => p.OrderDetails), itemDto.GetMessage(m => m.OrderDetails));
            }

            return result;
        }
    }
}
