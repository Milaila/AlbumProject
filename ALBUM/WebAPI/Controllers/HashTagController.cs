using System.Net;
using System.Web.Http;
using BLL.Interfaces;
using BLL.DTO;
using System.Web.Http.Description;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/hashTags")]
    [Authorize(Roles = Roles.UserRole)]
    public class HashTagsController : ApiController
    {
        private readonly IHashTagService _hashTagService;

        public HashTagsController(IHashTagService hashTagService, IProfileService profileService)
        {
            _hashTagService = hashTagService;
        }

        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(HashTagDTO))]
        [Authorize(Roles = Roles.UserRole)]
        public IHttpActionResult GetHashTag(int id)
        {
            var tag = _hashTagService.GetHashTagById(id);
            if (tag != null)
                return Ok(tag);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(HashTagDTO[]))]
        public IHttpActionResult GetAllHashTags()
        {
            var tags = _hashTagService.GetAllHashTags();
            if (tags != null)
                return Ok(tags);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("images/{id}")]
        [ResponseType(typeof(HashTagDTO[]))]
        public IHttpActionResult GetTagsByImage(int id)
        {
            var tags = _hashTagService.GetHashTagsByImage(id);
            if (tags != null)
                return Ok(tags);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get tag with such name
        /// </summary>
        /// <param name="name">Full tag name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("byFullName/{name}")]
        [ResponseType(typeof(HashTagDTO))]
        public IHttpActionResult GetByFullName(string name)
        {
            var tag = _hashTagService.GetHashTagByName(name);
            if (tag != null)
                return Ok(tag);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get all tags that contain such name
        /// </summary>
        /// <param name="name">Part of tag</param>
        /// <returns></returns>
        [HttpGet]
        [Route("byName/{name}")]
        [ResponseType(typeof(HashTagDTO[]))]
        public IHttpActionResult GetByName(string name)
        {
            var tags = _hashTagService.GetHashTagsByPartOfName(name);
            if (tags != null)
                return Ok(tags);
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add new hash tag
        /// </summary>
        /// <param name="hashTag">Id of hash tag</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(int))]
        [Route("")]
        public IHttpActionResult AddHashTag([FromBody]HashTagDTO hashTag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int id = _hashTagService.AddHashTag(hashTag);
            return Ok(id);
        }

        [HttpPut]
        [ResponseType(typeof(HashTagDTO))]
        [Route("")]
        public IHttpActionResult PutHashTag([FromBody]HashTagDTO hashTag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int id = _hashTagService.PutHashTag(hashTag);
            return Ok(id);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public IHttpActionResult DeleteHashTag(int id)
        {
            bool isRemoved = _hashTagService.DeleteHashTagById(id);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
        }
    }
}