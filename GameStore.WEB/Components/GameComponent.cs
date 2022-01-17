using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Components
{
    public class GameComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Invoke() => (_unitOfWork.GameRepository.Count() + _unitOfWork.ProductRepository.Count()).ToString();
    }
}
