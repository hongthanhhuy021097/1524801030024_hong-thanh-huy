using HelloWeb.Data.Infrastructure;
using HelloWeb.Data.Repositories;
using HelloWeb.Model.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWeb.Service
{
    public interface IFeedbackService
    {
        Feedback Create(Feedback feedback);

        IEnumerable<Feedback> getAll();

        void Save();
    }

    public class FeedbackService : IFeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        private IUnitOfWork _unitOfWork;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
        }

        public Feedback Create(Feedback feedback)
        {
            return _feedbackRepository.Add(feedback);
        }

        public IEnumerable<Feedback> getAll()
        {
            return _feedbackRepository.GetAll();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
