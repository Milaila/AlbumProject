using System.Web.Http;
using BLL.Interfaces;
using BLL.DTO;
using System.Net;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using WebAPI.Models;
using static WebAPI.Roles;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/profiles")]
    public class ProfilesController : ApiController
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        /// <summary>
        /// Get profile by its Id
        /// </summary>
        /// <returns>Profile details</returns>
        /// <param name="id">Profile id</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ProfileDTO))]
        [Authorize(Roles = UserRole)]
        [Route("{id}")]
        public IHttpActionResult GetProfile(int id)
        {
            ProfileDTO profile = _profileService.GetProfileById(id);
            if (profile == null)
                return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
            return Ok(profile);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO))]
        [Authorize(Roles = UserRole)]
        [Route("current")]
        public IHttpActionResult GetCurrentProfile()
        {
            ProfileDTO profile = GetCurrProfileDTO();
            if (profile == null)
                return new HttpActionResult(HttpStatusCode.NoContent, "User has no profile", request: Request);
            return Ok(profile);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO[]))]
        [Authorize(Roles = AdminRole)]
        [Route("")]
        public IHttpActionResult GetAllProfiles()
        {
            var profiles = _profileService.GetAllProfiles();
            if (profiles?.Count > 0)
                return Ok(profiles);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO))]
        [Authorize(Roles = AdminRole)]
        [Route("users/{id}")]
        public IHttpActionResult GetByUser(string userId)
        {
            var profile = _profileService.GetProfileByUser(userId);
            if (profile != null)
                return Ok(profile);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO[]))]
        [Route("byname/{name}")]
        public IHttpActionResult GetByName(string name)
        {
            var profiles = _profileService.GetProfilesByName(name);
            if (profiles?.Count > 0)
                return Ok(profiles);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO[]))]
        [Route("{id}/subscriptions")]
        [Authorize(Roles = UserRole)]
        public IHttpActionResult GetSubscriptionsByProfile(int id)
        {
            var profiles = _profileService.GetProfileSubscriptions(id);
            if (profiles?.Count > 0)
                return Ok(profiles);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO[]))]
        [Authorize(Roles = UserRole)]
        [Route("{id}/subscribers")]
        public IHttpActionResult GetSubscribersByProfile(int id)
        {
            var profiles = _profileService.GetProfileSubscriptions(id);
            if (profiles?.Count > 0)
                return Ok(profiles);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO[]))]
        [Authorize(Roles = UserRole)]
        [Route("subscriptions")]
        public IHttpActionResult GetCurrentSubscriptions()
        {
            int? id = GetCurrProfileDTO()?.Id;
            if (id == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            var profiles = _profileService.GetProfileSubscriptions(id.Value);
            if (profiles?.Count > 0)
                return Ok(profiles);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        [HttpGet]
        [ResponseType(typeof(ProfileDTO[]))]
        [Authorize(Roles = UserRole)]
        [Route("subscribers")]
        public IHttpActionResult GetCurrentSubscribers()
        {
            int? id = GetCurrProfileDTO()?.Id;
            if (id == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            var profiles = _profileService.GetProfileSubscriptions(id.Value);
            if (profiles?.Count > 0)
                return Ok(profiles);
            return new HttpActionResult(HttpStatusCode.NoContent, request: Request);
        }

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>Id of new profile</returns>
        [HttpPost]
        [ResponseType(typeof(int))]
        [Authorize(Roles = UserRole)]
        [Route("")]
        public IHttpActionResult AddMyProfile([FromBody]ProfilePostModel profile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ProfileDTO profileDTO = new ProfileDTO
            {
                Name = profile.Name,
                Description = profile.Description,
                ImageFileId = profile.ImageFileId,
                BirthDate = profile.BirthDate,
                UserId = User.Identity.GetUserId(), 
            };
            int id = _profileService.AddProfile(profileDTO);
            return Ok(id);
        }

        /// <summary>
        /// Create or update profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>Id of profile</returns>
        [HttpPut]
        [ResponseType(typeof(int))]
        [Authorize(Roles = UserRole)]
        [Route("")]
        public IHttpActionResult PutProfile([FromBody]ProfilePostModel profile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ProfileDTO profileDTO = new ProfileDTO
            {
                Id = GetCurrProfileDTO()?.Id ?? 0,
                Name = profile.Name,
                Description = profile.Description,
                ImageFileId = profile.ImageFileId,
                BirthDate = profile.BirthDate,
                UserId = User.Identity.GetUserId(),
            };
            int id = _profileService.UpdateOrCreateProfile(profileDTO);
            return Ok(id);
        }

        /// <summary>
        /// Create subscription
        /// </summary>
        /// <param name="subscriptionId">Id profile of the one you want to subscribe to</param>
        /// <returns>Id of profile</returns>
        [HttpPost]
        [Authorize(Roles = UserRole)]
        [Route("subscriptions/{subscriptionId}")]
        public IHttpActionResult AddSubscription(int subscriptionId)
        {
            int? profileId = GetCurrProfileDTO()?.Id;
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            _profileService.AddSubscription(subscriptionId, profileId.Value);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = UserRole)]
        [Route("subscriptions/{subscriptionId}")]
        public IHttpActionResult RemoveSubscription(int subscriptionId)
        {
            int? profileId = GetCurrProfileDTO()?.Id;
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            bool isRemoved = _profileService.RemoveProfileSubscription(subscriptionId, profileId.Value);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Authorize(Roles = UserRole)]
        [Route("subscribers/{subscriberId}")]
        public IHttpActionResult RemoveSubscriber(int subscriberId)
        {
            int? profileId = GetCurrProfileDTO()?.Id;
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            bool isRemoved = _profileService.RemoveProfileSubscriber(subscriberId, profileId.Value);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Authorize(Roles = AdminRole)]
        [Route("{id}")]
        public IHttpActionResult DeleteProfile(int id)
        {
            bool isRemoved = _profileService.DeleteProfileById(id);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Authorize(Roles = UserRole)]
        [Route("current")]
        public IHttpActionResult DeleteProfile()
        {
            int? profileId = GetCurrProfileDTO()?.Id;
            if (profileId == null)
                return new HttpActionResult(HttpStatusCode.NotFound, "User has no profile");
            bool isRemoved = _profileService.DeleteProfileById(profileId.Value);
            if (isRemoved)
                return Ok();
            return new HttpActionResult(HttpStatusCode.NoContent);
        }

        private ProfileDTO GetCurrProfileDTO()
        {
            string userId = User.Identity.GetUserId();
            if (userId == null)
                return null;
            return _profileService.GetProfileByUser(userId);
        }
    }
}