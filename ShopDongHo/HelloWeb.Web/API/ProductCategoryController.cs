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
    [RoutePrefix("api/productcategory")]
    [Authorize(Roles = "UserAdmin")]
    public class ProductCategoryController : ApiControllerBase
    {
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService,IProductCategoryService productCategoryService) 
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetProductCategoryById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request,()=> {
                HttpResponseMessage response = null;
                try
                { 
                    var result = _productCategoryService.GetById(id);
                    var query = Mapper.Map<ProductCategory, ProductCategoryViewModel>(result);
                    response = request.CreateResponse(HttpStatusCode.OK, query);
                }
                catch
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage UpdateProductCategory(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
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
                    var newProductCategory = _productCategoryService.GetById(productCategoryVm.ID);
                    EntityExtensions.UpdateProductCategory(newProductCategory, productCategoryVm);
                    _productCategoryService.Update(newProductCategory);
                    _productCategoryService.Save();
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        public HttpResponseMessage AddProductCategory(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response= null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProductCategory = new ProductCategory();
                    EntityExtensions.UpdateProductCategory(newProductCategory, productCategoryVm);
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
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
                var model = _productCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProductCategoryById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                try
                {
                    _productCategoryService.Delete(id);
                    _productCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }catch
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, id);
                }
                return response;
            });
        }

        [Route("deleteAll")]
        [HttpDelete]
        public HttpResponseMessage DeleteProductCategoryByAll(HttpRequestMessage request, string listItem)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                try
                {
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(listItem);
                    foreach (var item in listProductCategory)
                    {
                        _productCategoryService.Delete(item);
                        _productCategoryService.Save();
                    }

                    response = request.CreateResponse(HttpStatusCode.OK);
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
        public HttpResponseMessage GetAll(HttpRequestMessage requestMessage,String keyword, int page , int pageSize = 2)
        {
            return CreateHttpResponse(requestMessage, () =>
             {
                 int totalRow = 0;
                 
                 var model = _productCategoryService.GetAll(keyword);

                 totalRow = model.Count();
                 var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                 var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);
                 var paginationSet = new PaginationSet<ProductCategoryViewModel>()
                 {
                     Items = responseData,
                     Page = page,
                     TotalCount = totalRow,
                     TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                 };
                 var response = requestMessage.CreateResponse(HttpStatusCode.OK, paginationSet);
                 return response;
             });
        }
    }
}