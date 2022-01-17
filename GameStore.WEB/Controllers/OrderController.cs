using System.IO;
using System.Linq;
using System.Net.Mime;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Payment;
using GameStore.BLL.Services.Payment.Models;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Models;
using GameStore.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderController(
            IOrderService orderService,
            ICartService cartService,
            IMapper mapper)
        {
            _orderService = orderService;
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("order/changeQuantity")]
        public decimal ChangeQuantity(int itemIndex, short newQuantity) => 
            _mapper.Map<OrderViewModel>(_cartService.UpdateQuantity(HttpContext.Session, itemIndex, newQuantity)).TotalPrice;

        [HttpGet("order")]
        public ActionResult<OrderViewModel> CreateOrder()
        {
            return View(_mapper.Map<OrderViewModel>(_cartService.GetAll(HttpContext.Session)));
        }

        [HttpPost("order")]
        public ActionResult CreateOrder(string paymentMethod) => View(paymentMethod);

        public ActionResult PayForOrder(string paymentMethod)
        {
            var order = _mapper.Map<OrderViewModel>(_cartService.GetAll(HttpContext.Session));

            var result = Payments.GetInstanceByName(paymentMethod, _orderService).Pay(_mapper.Map<OrderDto>(order));

            if (!result.IsValid)
            {
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Key, e.Value));

                return View("CreateOrder", _mapper.Map<OrderViewModel>(result.Value));
            }

            HttpContext.Session = _cartService.RemoveAll(HttpContext.Session);

            return View("Success", _mapper.Map<OrderViewModel>(result.Value));
        }

        [HttpPost]
        public ActionResult Visa(VisaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(PaymentTypes.Visa, model);
            }

            model.Order = _mapper.Map<OrderViewModel>(_cartService.GetAll(HttpContext.Session));
            var visaPayment = new Visa(_orderService);
            visaPayment.AddPaymentInfo(_mapper.Map<VisaModelDto>(model));

            return PayForOrder(PaymentTypes.Visa);
        }

        [HttpGet("order/invoice/{id}")]
        public ActionResult Invoice(int id)
        {
            var order = _mapper.Map<OrderViewModel>(_orderService.GetById(id));

            return View(order);
        }

        [HttpGet("order/download/{id}")]
        public FileStreamResult Download(int id)
        {
            var order = _mapper.Map<OrderViewModel>(_orderService.GetById(id));
            var fileName = "Order" + id;

            return new FileStreamResult(new MemoryStream(PdfService.GenerateFile(
                _mapper.Map<IPdfObjectData>(order))), MediaTypeNames.Application.Pdf)
            {
                FileDownloadName = fileName + ".pdf"
            };
        }
    }
}
