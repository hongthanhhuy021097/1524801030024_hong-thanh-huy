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
using System.Web.Http;
using System.Web.Script.Serialization;

namespace HelloWeb.Web.API
{
    [RoutePrefix("api/slide")]
    [Authorize(Roles = "UserAdmin")]
    public class SlideController : ApiControllerBase
    {
        ICommonService _conmonService;

        public SlideController(IErrorService errorService, ICommonService conmonService) : base(errorService)
        {
            this._conmonService = conmonService;
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, SlideViewModel slideVm)
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
                    var dbSlide = _conmonService.GetSlideById(slideVm.ID);

                    EntityExtensions.UpdateSlide(dbSlide,slideVm);
                    _conmonService.Update(dbSlide);
                    _conmonService.Save();

                    var responseData = Mapper.Map<Slide, SlideViewModel>(dbSlide);
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
                    foreach (var item in listId)
                    {
                        _conmonService.Delete(new Slide(),item);
                        count++;
                    }
                    _conmonService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, count);
                }
                catch
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, count);
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
                var model = _conmonService.GetSlides();

                var responseData = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(model);

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
                var model = _conmonService.GetSlideById(id);

                var responseData = Mapper.Map<Slide, SlideViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, SlideViewModel SlideVm)
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
                    var newSlide = new Slide();
                    EntityExtensions.UpdateSlide(newSlide, SlideVm);
                    _conmonService.Add(newSlide);
                    _conmonService.Save();

                    var responseData = Mapper.Map<Slide, SlideViewModel>(newSlide);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteSlideById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                try
                {
                    _conmonService.Delete(new Slide(),id);
                    _conmonService.Save();
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
        public HttpResponseMessage GetAllProduct(HttpRequestMessage request, String keyword, int page, int pageSize = 5)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;

                var totalRow = 0;
                try
                {
                    var result = _conmonService.GetSlidesByKeyword(keyword);

                    totalRow = result.Count();

                    var query = result.OrderByDescending(x => x.DisplayOrder).Skip(page * pageSize).Take(pageSize);

                    var responseData = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(query);

                    PaginationSet<SlideViewModel> pag = new PaginationSet<SlideViewModel>()
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
