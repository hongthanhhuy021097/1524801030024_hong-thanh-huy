using AutoMapper;
using HelloWeb.Model.Models;
using HelloWeb.Service;
using HelloWeb.Web.Infrastructure.Core;
using HelloWeb.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelloWeb.Web.API
{
    [RoutePrefix("api/order")]
    [Authorize(Roles = "UserAdmin")]
    public class OrderController : ApiControllerBase
    {
        IOrderService _orderService;


        public OrderController(IErrorService errorService, IOrderService orderService) :base(errorService)
        {
            this._orderService = orderService;
        }

        [Route("getOrderById/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetOrderById(HttpRequestMessage request,int id)
        {
            return CreateHttpResponse(request, () => {
                var model = _orderService.GetOrderById(id);
                var responseData = Mapper.Map<Order, OrderViewModel>(model);
                return request.CreateResponse(HttpStatusCode.OK,responseData);
            });
        }

        [Route("getOrderDetailbyid")]
        [HttpGet]
        public HttpResponseMessage GetOrderDetailById(HttpRequestMessage request, int id, int page, int pageSize = 5)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var result = _orderService.GetOrderDetailById(id);
                var totalRow = result.Count();

                var query = result.OrderByDescending(x => x.Price).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(query);

                PaginationSet<OrderDetailViewModel> pag = new PaginationSet<OrderDetailViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((Decimal)totalRow / pageSize)
                };
                response = request.CreateResponse(HttpStatusCode.OK, pag);
                return response;
            });
        }

        [Route("getAll")]
        [HttpGet]
        public HttpResponseMessage getAllOrder(HttpRequestMessage request, String keyWord, int page, int pageSize = 5)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                try
                { 
                    var model = _orderService.getAll(keyWord);
                    var totalRow = model.Count();
                    var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                    var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query);
                    PaginationSet<OrderViewModel> pag = new PaginationSet<OrderViewModel>()
                    {
                        Items = responseData,
                        Page = page,
                        TotalCount = totalRow,
                        TotalPages = (int)Math.Ceiling((Decimal)totalRow / pageSize)
                    };

                    response = request.CreateResponse(HttpStatusCode.OK, pag);
                }
                catch
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, "ERROR");
                }
            return response;
            });
        }

    }
}
