using AutoMapper;
using HelloWeb.Model.Modes;
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
    [RoutePrefix("api/feedBacks")]
    [Authorize(Roles = "UserAdmin")]
    public class FeedBacksController : ApiControllerBase
    {
        IFeedbackService _feedbackService;
        public FeedBacksController(IErrorService errorService, IFeedbackService feedbackService) : base(errorService)
        {
            this._feedbackService = feedbackService;
        }

        [Route("getAll")]
        [HttpGet]
        public HttpResponseMessage getAllFeedBacks (HttpRequestMessage request,int page , int pageSize = 5)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _feedbackService.getAll();
                var totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(query);
                PaginationSet<FeedbackViewModel> pag = new PaginationSet<FeedbackViewModel>()
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

    }
}
