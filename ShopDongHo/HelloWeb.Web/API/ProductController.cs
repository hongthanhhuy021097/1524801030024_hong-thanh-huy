using AutoMapper;
using HelloWeb.Model.Models;
using HelloWeb.Service;
using HelloWeb.Web.Infrastructure.Core;
using HelloWeb.Web.Infrastructure.Extentsions;
using HelloWeb.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace HelloWeb.Web.API
{
    [RoutePrefix("api/product")]
    [Authorize(Roles = "UserAdmin")]
    public class ProductController : ApiControllerBase
    {
        IProductService _productService;

        public ProductController (IErrorService errorService , IProductService productService)
            :base(errorService)
        {
            this._productService = productService;
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProduct = _productService.GetById(productVm.ID);
                    dbProduct.UpdateProduct(productVm);
                    dbProduct.UpdatedDate = DateTime.Now;

                    _productService.Update(dbProduct);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deleteAll")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteProductByAll(HttpRequestMessage request, string listItem)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                var count = 0;
                try
                {
                    var listId = new JavaScriptSerializer().Deserialize<List<int>>(listItem);
                    foreach(var item in listId)
                    {
                        _productService.Delete(item);
                        count++;
                    }
                    _productService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, count);
                }
                catch {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,count);
                }
                return response;
            });
        }

        

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetById(id);

                var responseData = Mapper.Map<Product, ProductViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProduct = new Product();
                    EntityExtensions.UpdateProduct(newProduct,productVm);
                    newProduct.CreatedDate = DateTime.Now;
                    _productService.Add(newProduct);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteProductById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                try
                {
                    _productService.Delete(id);
                    _productService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }
                catch
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAllProduct(HttpRequestMessage request,String keyword, int page,  int pageSize =5)
        {
            return CreateHttpResponse(request , ()=> {
                HttpResponseMessage response = null;
                
                var totalRow = 0;
                try { 
                    var result = _productService.GetAll(keyword);

                    totalRow = result.Count();

                    var query = result.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                    var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                    PaginationSet<ProductViewModel> pag = new PaginationSet<ProductViewModel>() {
                        Items = responseData,
                        Page = page,
                        TotalCount = totalRow,
                        TotalPages = (int)Math.Ceiling((Decimal)totalRow/pageSize)
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