using HelloWeb.Common;
using HelloWeb.Data.Infrastructure;
using HelloWeb.Data.Repositories;
using HelloWeb.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWeb.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlides();
        Slide GetSlideById(int id);
        void Update(Slide dbSlide);
        void Save();
        void Delete(Object obj,int item);
        void Add(Object obj);
        IEnumerable<Slide> GetSlidesByKeyword(string keyword);
        SystemConfig GetSystemConfig(string code);
    }
    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;
        ISystemConfigRepository _systemConfigRepository;
        IUnitOfWork _unitOfWork;
        ISlideRepository _slideRepository;
        public CommonService(IFooterRepository footerRepository, IUnitOfWork unitOfWork,ISlideRepository slideRepository, ISystemConfigRepository systemConfigRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _slideRepository = slideRepository;
            _systemConfigRepository = systemConfigRepository;
        }

        public void Add(object obj)
        {
            if(obj is Slide)
            {
                _slideRepository.Add((Slide)obj);
            }
        }

        public void Delete(Object obj,int item)
        {
            if(obj is Slide)
            {
                _slideRepository.Delete(item);
            }
            
        }

        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }

        public Slide GetSlideById(int id)
        {
            return _slideRepository.GetSingleById(id);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x => x.Status);
        }

        public IEnumerable<Slide> GetSlidesByKeyword(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _slideRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _slideRepository.GetAll();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Slide dbSlide)
        {
            _slideRepository.Update(dbSlide);
        }

        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }
    }
}
