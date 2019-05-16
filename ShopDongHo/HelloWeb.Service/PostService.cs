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
    public interface IPostService
    {
        void add(Post post);
        void update(Post post);
        void delete(int id);
        IEnumerable<Post> getAll();
        IEnumerable<Post> getAllPaging(int page, int pageSize, out int totalRow);
        Post getByID(int id);
        IEnumerable<Post> getAllByTagPaging(int page, int pageSize, out int totalRow);
        void saveChange();
    }
    public class PostService : IPostService
    {
        IPostRepository _postRepository;
        IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
        }

        public void add(Post post)
        {
            _postRepository.Add(post);
        }

        public void delete(int id)
        {
            _postRepository.Delete(id);
        }

        public IEnumerable<Post> getAll()
        {
            return _postRepository.GetAll(new string[] {"PostCategory"});
        }

        public IEnumerable<Post> getAllByTagPaging(int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public IEnumerable<Post> getAllPaging(int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public Post getByID(int id)
        {
            return _postRepository.GetSingleById(id);
        }

        public void saveChange()
        {
            _unitOfWork.Commit();
        }

        public void update(Post post)
        {
            _postRepository.Update(post);
        }
    }
}
